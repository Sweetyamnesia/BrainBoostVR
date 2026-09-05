using Microsoft.AspNetCore.Mvc;
using BrainBoostVR_API.Data;
using BrainBoostVR_API.Models;
using BrainBoostVR_API.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BrainBoostVR_API.Controllers
{
    [Route("api/sessions")]
    [ApiController]
    public class SessionsController : ControllerBase
    {
        private readonly BrainBoostDbContext _context;
        private readonly FirebaseService _firebaseService;

        public SessionsController(
            BrainBoostDbContext context,
            FirebaseService firebaseService)
        {
            _context = context;
            _firebaseService = firebaseService;
        }

        // ============================================================
        // Vérifie le token Firebase et retourne le FirebaseUID
        // ============================================================
        private async Task<string?> VerifyAndGetUidAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
                return null;

            var token = Request.Headers["Authorization"]
                .ToString()
                .Replace("Bearer ", "");

            try
            {
                return await _firebaseService.VerifyTokenAsync(token);
            }
            catch
            {
                return null;
            }
        }

        // ============================================================
        // Vérifie si un pseudo existe
        // ============================================================
        [HttpGet("check-pseudo")]
        public async Task<IActionResult> CheckPseudoExists(
            [FromQuery] string pseudo)
        {
            if (string.IsNullOrWhiteSpace(pseudo))
                return BadRequest("Pseudo vide");

            bool exists = await _context.Sessions
                .AnyAsync(s => s.Pseudo == pseudo);

            return Ok(exists);
        }

        // ============================================================
        // CRÉER UNE NOUVELLE SESSION
        // POST /api/sessions
        // ============================================================
        [HttpPost]
        public async Task<IActionResult> CreateSession(
            [FromBody] UnitySessionDto dto)
        {
            Console.WriteLine(
                "[API] 🔹 Reçu POST /api/sessions avec DTO: " +
                System.Text.Json.JsonSerializer.Serialize(dto)
            );

            var firebaseUid = await VerifyAndGetUidAsync();

            if (firebaseUid == null)
                return Unauthorized("Invalid or missing Firebase token.");

            // --------------------------------------------------------
            // Recherche de l'utilisateur
            // --------------------------------------------------------
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.FirebaseUID == firebaseUid);

            // Si l'utilisateur n'existe pas, on le crée
            if (user == null)
            {
                user = new User
                {
                    FirebaseUID = firebaseUid,
                    Name = "Unknown",
                    CreatedAt = DateTime.UtcNow
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();
            }

            // --------------------------------------------------------
            // Vérification du SessionUid
            // --------------------------------------------------------
            if (string.IsNullOrWhiteSpace(dto.SessionUid))
            {
                return BadRequest("SessionUid est obligatoire.");
            }

            // Évite de créer deux fois exactement la même session
            var existingSession = await _context.Sessions
                .FirstOrDefaultAsync(s =>
                    s.SessionUid == dto.SessionUid &&
                    s.UserID == user.UserID);

            if (existingSession != null)
            {
                Console.WriteLine(
                    $"[API] ⚠️ Session déjà existante : {existingSession.SessionUid}"
                );

                return Ok(new
                {
                    status = "already_exists",
                    sessionUid = existingSession.SessionUid,
                    user = user.FirebaseUID,
                    startedAt = existingSession.StartTime
                });
            }

            // --------------------------------------------------------
            // Détermination de l'heure de début
            // --------------------------------------------------------
            DateTime startTime;

            if (!string.IsNullOrWhiteSpace(dto.StartTime) &&
                DateTimeOffset.TryParse(dto.StartTime, out var parsedStartTime))
            {
                startTime = parsedStartTime.DateTime;
            }
            else
            {
                startTime = DateTime.UtcNow;
            }

            // --------------------------------------------------------
            // Création de la session
            // --------------------------------------------------------
            var session = new Session
            {
                UserID = user.UserID,
                FirebaseUID = firebaseUid,
                SessionUid = dto.SessionUid,

                StartTime = startTime,

                // Une session nouvellement créée n'est pas terminée
                EndTime = null,
                DurationMinutes = null,

                Score = dto.Score,
                Errors = dto.Errors
            };

            try
            {
                _context.Sessions.Add(session);
                await _context.SaveChangesAsync();

                Console.WriteLine(
                    $"[API] ✅ Session enregistrée OK " +
                    $"(SessionUid={session.SessionUid})"
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    "[API] ❌ ERREUR SaveChanges Sessions : " +
                    ex.Message
                );

                return StatusCode(
                    500,
                    "Erreur lors de l'enregistrement de la session."
                );
            }

            return Ok(new
            {
                status = "success",
                sessionUid = session.SessionUid,
                user = user.FirebaseUID,
                startedAt = session.StartTime
            });
        }

        // ============================================================
        // TERMINER UNE SESSION EXISTANTE
        // POST /api/sessions/complete
        // ============================================================
        [HttpPost("complete")]
        public async Task<IActionResult> CompleteSession(
            [FromBody] UnitySessionCompleteDto dto)
        {
            Console.WriteLine(
                "[API] 🔹 Reçu POST /api/sessions/complete"
            );

            var firebaseUid = await VerifyAndGetUidAsync();

            if (firebaseUid == null)
                return Unauthorized(
                    "Invalid or missing Firebase token."
                );

            // --------------------------------------------------------
            // Vérification du SessionUid
            // --------------------------------------------------------
            if (string.IsNullOrWhiteSpace(dto.SessionUid))
            {
                return BadRequest("SessionUid est obligatoire.");
            }

            // --------------------------------------------------------
            // Recherche de l'utilisateur
            // --------------------------------------------------------
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.FirebaseUID == firebaseUid);

            if (user == null)
                return NotFound("User not found");

            // --------------------------------------------------------
            // Recherche de la session existante
            // --------------------------------------------------------
            var session = await _context.Sessions
                .FirstOrDefaultAsync(s =>
                    s.SessionUid == dto.SessionUid &&
                    s.UserID == user.UserID);

            if (session == null)
            {
                Console.WriteLine(
                    $"[API] ❌ Session introuvable : {dto.SessionUid}"
                );

                return NotFound("Session not found");
            }

            // --------------------------------------------------------
            // Évite de terminer plusieurs fois la même session
            // --------------------------------------------------------
            if (session.EndTime.HasValue)
            {
                Console.WriteLine(
                    $"[API] ⚠️ Session déjà terminée : " +
                    $"{session.SessionUid}"
                );

                return Ok(new
                {
                    status = "already_completed",
                    sessionUid = session.SessionUid,
                    endedAt = session.EndTime,
                    durationMinutes = session.DurationMinutes
                });
            }

            // --------------------------------------------------------
            // Fin de session
            // --------------------------------------------------------
            session.EndTime = DateTime.Now;

            // Sécurité si StartTime est null
            if (!session.StartTime.HasValue)
            {
                session.StartTime = session.EndTime.Value;
            }

            // Calcul de la durée totale de la SESSION
            session.DurationMinutes =
                (float?)(session.EndTime - session.StartTime)
                ?.TotalMinutes;

            try
            {
                await _context.SaveChangesAsync();

                Console.WriteLine(
                    $"[API] ✅ Session terminée : " +
                    $"{session.SessionUid} | " +
                    $"Durée={session.DurationMinutes:F2} min"
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    "[API] ❌ ERREUR lors de la fin de session : " +
                    ex.Message
                );

                return StatusCode(
                    500,
                    "Erreur lors de la mise à jour de la session."
                );
            }

            return Ok(new
            {
                status = "success",
                sessionUid = session.SessionUid,
                endedAt = session.EndTime,
                durationMinutes = session.DurationMinutes
            });
        }

        // ============================================================
        // RÉCUPÉRER L'HISTORIQUE DES SESSIONS
        // GET /api/sessions/history/{firebaseUID}
        // ============================================================
        [HttpGet("history/{firebaseUID}")]
        public async Task<IActionResult> GetSessionHistory(
            string firebaseUID)
        {
            var uid = await VerifyAndGetUidAsync();

            if (uid == null || uid != firebaseUID)
            {
                return Unauthorized(
                    "Invalid or missing Firebase token."
                );
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.FirebaseUID == firebaseUID);

            if (user == null)
                return NotFound("User not found");

            var sessions = await _context.Sessions
                .Where(s => s.UserID == user.UserID)
                .OrderByDescending(s => s.StartTime)
                .Select(s => new UnitySessionDto
                {
                    FirebaseUID = s.FirebaseUID ?? "",

                    SessionUid = s.SessionUid,

                    StartTime = s.StartTime.HasValue
                        ? s.StartTime.Value.ToString("o")
                        : "",

                    EndTime = s.EndTime.HasValue
                        ? s.EndTime.Value.ToString("o")
                        : "",

                    DurationMinutes =
                        s.DurationMinutes ?? 0f,

                    Score = s.Score,
					Errors = s.Errors
                })
                .ToArrayAsync();

            return Ok(sessions);
        }
    }

    // ================================================================
    // DTO utilisé par Unity pour créer / récupérer une session
    // ================================================================
    public class UnitySessionDto
    {
        public string FirebaseUID { get; set; } = string.Empty;

        public string SessionUid { get; set; } = string.Empty;

        public float DurationMinutes { get; set; } = 0f;

        public string StartTime { get; set; } = string.Empty;

        public string EndTime { get; set; } = string.Empty;

        public int Score { get; set; } = 0;

        public int Errors { get; set; } = 0;
    }

    // ================================================================
    // DTO utilisé pour terminer une session
    // ================================================================
    public class UnitySessionCompleteDto
    {
        public string FirebaseUID { get; set; } = string.Empty;

        public string SessionUid { get; set; } = string.Empty;
    }
}