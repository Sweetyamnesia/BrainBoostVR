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

        public SessionsController(BrainBoostDbContext context, FirebaseService firebaseService)
        {
            _context = context;
            _firebaseService = firebaseService;
        }

		// Vérifie le token Firebase et retourne le FirebaseUID
		private async Task<string?> VerifyAndGetUidAsync()
		{
			if (!Request.Headers.ContainsKey("Authorization"))
				return null;

			var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
			try
			{
				return await _firebaseService.VerifyTokenAsync(token);
			}
			catch
			{
				return null;
			}
		}
		
		[HttpGet("check-pseudo")]
		public async Task<IActionResult> CheckPseudoExists([FromQuery] string pseudo)
		{
			if (string.IsNullOrWhiteSpace(pseudo))
				return BadRequest("Pseudo vide");

			bool exists = await _context.Sessions.AnyAsync(s => s.Pseudo == pseudo);
			return Ok(exists);
		}


        // Créer une nouvelle session
        [HttpPost]
        public async Task<IActionResult> CreateSession([FromBody] UnitySessionDto dto)
        {
            Console.WriteLine("[API] 🔹 Reçu POST /api/sessions avec DTO: " + System.Text.Json.JsonSerializer.Serialize(dto));

            var firebaseUid = await VerifyAndGetUidAsync();
            if (firebaseUid == null) return Unauthorized("Invalid or missing Firebase token.");

            var user = await _context.Users.FirstOrDefaultAsync(u => u.FirebaseUID == firebaseUid);
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

            var session = new Session
            {
                UserID = user.UserID,
                FirebaseUID = firebaseUid,
                SessionUid = dto.SessionUid, // ← mapping string UnitySessionDto.SessionUid
                StartTime = string.IsNullOrEmpty(dto.StartTime) ? DateTime.UtcNow : DateTime.Parse(dto.StartTime),
                EndTime = null,
                DurationMinutes = null,
                Score = dto.Score,
                Errors = dto.Errors
            };

            try
            {
                _context.Sessions.Add(session);
                await _context.SaveChangesAsync();
                Console.WriteLine($"[API] Session enregistrée OK (SessionUid={session.SessionUid})");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[API] ERREUR SaveChanges Sessions : {ex.Message}");
            }

            return Ok(new
            {
                status = "success",
                sessionUid = session.SessionUid, // ← on retourne le string côté Unity
                user = user.FirebaseUID,
                startedAt = session.StartTime
            });
        }

        // Terminer une session
        [HttpPost("complete")]
        public async Task<IActionResult> CompleteSession([FromBody] UnitySessionCompleteDto dto)
        {
            Console.WriteLine("[API] Reçu POST /api/sessions/complete");

            var firebaseUid = await VerifyAndGetUidAsync();
            if (firebaseUid == null) return Unauthorized("Invalid or missing Firebase token.");

            var user = await _context.Users.FirstOrDefaultAsync(u => u.FirebaseUID == firebaseUid);
            if (user == null) return NotFound("User not found");

            var session = await _context.Sessions
                .FirstOrDefaultAsync(s => s.SessionUid == dto.SessionUid && s.UserID == user.UserID); // ← match sur SessionUid

            if (session == null) return NotFound("Session not found");

            session.EndTime = DateTime.UtcNow;

            if (!session.StartTime.HasValue)
                session.StartTime = session.EndTime.Value;

            session.DurationMinutes = (float?)(session.EndTime - session.StartTime)?.TotalMinutes;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                status = "success",
                endedAt = session.EndTime,
                durationMinutes = session.DurationMinutes
            });
        }

        // Récupérer l’historique des sessions d’un utilisateur
        [HttpGet("history/{firebaseUID}")]
        public async Task<IActionResult> GetSessionHistory(string firebaseUID)
        {
            var uid = await VerifyAndGetUidAsync();
            if (uid == null || uid != firebaseUID)
                return Unauthorized("Invalid or missing Firebase token.");

            var user = await _context.Users.FirstOrDefaultAsync(u => u.FirebaseUID == firebaseUID);
            if (user == null) return NotFound("User not found");

            var sessions = await _context.Sessions
                .Where(s => s.UserID == user.UserID)
                .OrderByDescending(s => s.StartTime)
                .Select(s => new UnitySessionDto
                {
                    SessionUid = s.SessionUid, // ← mapping string
                    StartTime = s.StartTime.HasValue ? s.StartTime.Value.ToString("o") : "",
                    EndTime = s.EndTime.HasValue ? s.EndTime.Value.ToString("o") : "",
                    DurationMinutes = s.DurationMinutes ?? 0f,
                    Score = s.Score,
                    Errors = s.Errors
                })
                .ToArrayAsync();

            return Ok(sessions);
        }
    }

    // DTOs Unity
    public class UnitySessionDto
    {
        public string FirebaseUID { get; set; } = string.Empty;
        public string SessionUid { get; set; } = string.Empty; // → maps to SessionUid
        public float DurationMinutes { get; set; } = 0f;
        public string StartTime { get; set; } = string.Empty;
        public string EndTime { get; set; } = string.Empty;
        public int Score { get; set; } = 0;
        public int Errors { get; set; } = 0;
    }

    public class UnitySessionCompleteDto
    {
        public string FirebaseUID { get; set; } = string.Empty;
        public string SessionUid { get; set; } = string.Empty; // → maps to SessionUid
    }
}
