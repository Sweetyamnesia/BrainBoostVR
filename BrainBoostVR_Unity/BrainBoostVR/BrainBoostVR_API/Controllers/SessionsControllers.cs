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

            var authorization = Request.Headers["Authorization"].ToString();

            if (!authorization.StartsWith("Bearer "))
                return null;

            var token = authorization.Substring("Bearer ".Length).Trim();

            if (string.IsNullOrWhiteSpace(token))
                return null;

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
        // Vérifie si un FirebaseUID est autorisé à utiliser un profil
        // ============================================================
        private async Task<bool> IsProfileLinkedAsync(
            string firebaseUid,
            int userID)
        {
            return await _context.FirebaseProfiles
                .AnyAsync(fp =>
                    fp.FirebaseUID == firebaseUid &&
                    fp.UserID == userID);
        }

        // ============================================================
        // Vérifie si un pseudo existe dans Users
        // ============================================================
        [HttpGet("check-pseudo")]
        public async Task<IActionResult> CheckPseudoExists(
            [FromQuery] string pseudo)
        {
            if (string.IsNullOrWhiteSpace(pseudo))
                return BadRequest("Pseudo vide");

            var normalizedPseudo = pseudo.Trim();

            bool exists = await _context.Users
                .AnyAsync(u => u.Name == normalizedPseudo);

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
            if (dto == null)
                return BadRequest("Payload invalide.");

            Console.WriteLine(
                "[API] 🔹 Reçu POST /api/sessions avec DTO: " +
                System.Text.Json.JsonSerializer.Serialize(dto)
            );

            var firebaseUid = await VerifyAndGetUidAsync();

            if (firebaseUid == null)
                return Unauthorized("Invalid or missing Firebase token.");

            // --------------------------------------------------------
            // Vérification du profil sélectionné
            // --------------------------------------------------------
            if (dto.UserID <= 0)
                return BadRequest("UserID est obligatoire.");

            // --------------------------------------------------------
            // Vérification du SessionUid
            // --------------------------------------------------------
            if (string.IsNullOrWhiteSpace(dto.SessionUid))
                return BadRequest("SessionUid est obligatoire.");

            // --------------------------------------------------------
            // Vérifie que FirebaseUID peut utiliser ce profil
            // --------------------------------------------------------
            bool profileLinked = await IsProfileLinkedAsync(
                firebaseUid,
                dto.UserID);

            if (!profileLinked)
            {
                return Unauthorized(
                    "Ce profil n'est pas associé à cette identité Firebase."
                );
            }

            // --------------------------------------------------------
            // Recherche du profil
            // --------------------------------------------------------
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.UserID == dto.UserID);

            if (user == null)
                return NotFound("Profil utilisateur introuvable.");

            // --------------------------------------------------------
            // Évite de créer deux fois exactement la même session
            // --------------------------------------------------------
            var existingSession = await _context.Sessions
                .FirstOrDefaultAsync(s =>
                    s.SessionUid == dto.SessionUid &&
                    s.UserID == user.UserID);

            if (existingSession != null)
            {
                Console.WriteLine(
                    $"[API] ⚠️ Session déjà existante : " +
                    $"{existingSession.SessionUid}"
                );

                return Ok(new
                {
                    status = "already_exists",
                    sessionUid = existingSession.SessionUid,
                    userID = user.UserID,
                    firebaseUID = firebaseUid,
                    pseudo = user.Name,
                    startedAt = existingSession.StartTime
                });
            }

            // --------------------------------------------------------
            // Détermination de l'heure de début
            // --------------------------------------------------------
            DateTime startTime;

            if (!string.IsNullOrWhiteSpace(dto.StartTime) &&
                DateTimeOffset.TryParse(
                    dto.StartTime,
                    out var parsedStartTime))
            {
                startTime = parsedStartTime.UtcDateTime;
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

                // Ces informations sont enregistrées comme historique
                // au moment de la création de la session.
                FirebaseUID = firebaseUid,
                Pseudo = user.Name,

                SessionUid = dto.SessionUid,

                StartTime = startTime,

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
                    $"(SessionUid={session.SessionUid}, " +
                    $"UserID={session.UserID}, " +
                    $"Pseudo={session.Pseudo})"
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
                userID = user.UserID,
                firebaseUID = firebaseUid,
                pseudo = user.Name,
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
            if (dto == null)
                return BadRequest("Payload invalide.");

            Console.WriteLine(
                "[API] 🔹 Reçu POST /api/sessions/complete"
            );

            var firebaseUid = await VerifyAndGetUidAsync();

            if (firebaseUid == null)
                return Unauthorized(
                    "Invalid or missing Firebase token."
                );

            // --------------------------------------------------------
            // Vérification du profil sélectionné
            // --------------------------------------------------------
            if (dto.UserID <= 0)
                return BadRequest("UserID est obligatoire.");

            // --------------------------------------------------------
            // Vérification du SessionUid
            // --------------------------------------------------------
            if (string.IsNullOrWhiteSpace(dto.SessionUid))
                return BadRequest("SessionUid est obligatoire.");

            // --------------------------------------------------------
            // Vérifie que FirebaseUID peut utiliser ce profil
            // --------------------------------------------------------
            bool profileLinked = await IsProfileLinkedAsync(
                firebaseUid,
                dto.UserID);

            if (!profileLinked)
            {
                return Unauthorized(
                    "Ce profil n'est pas associé à cette identité Firebase."
                );
            }

            // --------------------------------------------------------
            // Recherche du profil
            // --------------------------------------------------------
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.UserID == dto.UserID);

            if (user == null)
                return NotFound("Profil utilisateur introuvable.");

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
            session.EndTime = DateTime.UtcNow;

			if (!session.StartTime.HasValue)
			{
				session.StartTime = session.EndTime.Value;
			}

			session.DurationMinutes =
				(float?)(session.EndTime - session.StartTime)
				?.TotalMinutes;

			// --------------------------------------------------------
			// Récupération du score associé à cette session
			// --------------------------------------------------------
			var sessionScore = await _context.Scores
				.Where(s =>
					s.UserID == dto.UserID &&
					s.SessionUid == dto.SessionUid)
				.OrderByDescending(s => s.Timestamp)
				.FirstOrDefaultAsync();

			if (sessionScore != null)
			{
				session.Score = sessionScore.Value ?? 0;
				session.Errors = sessionScore.Errors ?? 0;

				Console.WriteLine(
					$"[SESSION] Score récupéré pour la session : " +
					$"Score={session.Score}, " +
					$"Errors={session.Errors}"
				);
			}
			else
			{
				Console.WriteLine(
					"[SESSION] Aucun score trouvé pour cette session."
				);
			}

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
        // GET /api/sessions/history/{userID}
        // ============================================================
        [HttpGet("history/{userID}")]
        public async Task<IActionResult> GetSessionHistory(
            int userID)
        {
            var firebaseUid = await VerifyAndGetUidAsync();

            if (firebaseUid == null)
            {
                return Unauthorized(
                    "Invalid or missing Firebase token."
                );
            }

            if (userID <= 0)
                return BadRequest("UserID invalide.");

            // --------------------------------------------------------
            // Vérifie que FirebaseUID peut consulter ce profil
            // --------------------------------------------------------
            bool profileLinked = await IsProfileLinkedAsync(
                firebaseUid,
                userID);

            if (!profileLinked)
            {
                return Unauthorized(
                    "Ce profil n'est pas associé à cette identité Firebase."
                );
            }

            // --------------------------------------------------------
            // Vérifie que le profil existe
            // --------------------------------------------------------
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.UserID == userID);

            if (user == null)
                return NotFound("User not found");

            // --------------------------------------------------------
            // Historique du profil
            // --------------------------------------------------------
            var sessions = await _context.Sessions
                .Where(s => s.UserID == userID)
                .OrderByDescending(s => s.StartTime)
                .Select(s => new UnitySessionDto
                {
                    UserID = s.UserID,

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
        public int UserID { get; set; }

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
        public int UserID { get; set; }

        public string FirebaseUID { get; set; } = string.Empty;

        public string SessionUid { get; set; } = string.Empty;
    }
}