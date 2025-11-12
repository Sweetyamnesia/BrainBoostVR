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
    [Route("api/scores")]
    [ApiController]
    public class ScoresController : ControllerBase
    {
        private readonly BrainBoostDbContext _context;
        private readonly FirebaseService _firebaseService;

        public ScoresController(BrainBoostDbContext context, FirebaseService firebaseService)
        {
            _context = context;
            _firebaseService = firebaseService;
        }

        // 🔹 Vérifie le token Firebase et retourne le FirebaseUID
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

        // 🔹 Enregistrer un score envoyé depuis Unity
        [HttpPost]
        public async Task<IActionResult> SubmitScore([FromBody] UnityScoreDto dto)
		{
            Console.WriteLine("[API] 🔹 Reçu POST /api/scores avec DTO: " + System.Text.Json.JsonSerializer.Serialize(dto));
			
			// 1️⃣ Vérification du token Firebase
            var firebaseUid = await VerifyAndGetUidAsync();
            if (firebaseUid == null)
                return Unauthorized("Invalid or missing Firebase token.");

            // 2️⃣ Vérifier que l'utilisateur existe via le Firebase UID vérifié
            var user = await _context.Users.FirstOrDefaultAsync(u => u.FirebaseUID == firebaseUid);
            if (user == null)
            {
                // Crée un utilisateur si c’est la première fois que ce FirebaseUID se connecte
                user = new User
                {
                    FirebaseUID = firebaseUid,
                    Name = "Unknown",
                    CreatedAt = DateTime.UtcNow
                };
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
            }

            // 3️⃣ Créer l'objet Score
            var score = new Score
            {
                UserID = user.UserID,
                Value = dto.Score,
                Errors = dto.Errors,
                TimeSpent = dto.TimeSpent,
                SessionUid = dto.SessionUid,
                Timestamp = DateTime.TryParse(dto.Timestamp, out var ts) ? ts : DateTime.UtcNow,

                // ExerciseID rendu nullable dans la classe Score (int?)
                ExerciseID = null
            };

            // 4️⃣ Sauvegarde avec gestion des erreurs
            try
            {
                _context.Scores.Add(score);
                await _context.SaveChangesAsync();

                Console.WriteLine($"[API][ScoresController] ✅ Score enregistré pour UID={firebaseUid}, ScoreID={score.ScoreID}");

                return Ok(new
                {
                    status = "success",
                    user = user.FirebaseUID,
                    scoreId = score.ScoreID,
                    savedAt = score.Timestamp
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[API][ScoresController] ❌ Erreur SaveChanges: {ex.Message}");
                return StatusCode(500, "Erreur lors de l'enregistrement du score");
            }
        }

        // 🔹 Récupérer les scores d'un utilisateur
        [HttpGet]
        public async Task<IActionResult> GetScores([FromQuery] string firebaseUID)
        {
            var firebaseUid = await VerifyAndGetUidAsync();
            if (firebaseUid == null)
                return Unauthorized("Invalid or missing Firebase token.");

            var user = await _context.Users.FirstOrDefaultAsync(u => u.FirebaseUID == firebaseUid);
            if (user == null)
                return NotFound("User not found");

            var scores = await _context.Scores
                .Where(s => s.UserID == user.UserID)
                .OrderByDescending(s => s.Timestamp)
                .ToListAsync();

            return Ok(scores);
        }
    }

    // 🔹 DTO utilisé pour la réception depuis Unity
    public class UnityScoreDto
    {
        public string FirebaseUID { get; set; } = string.Empty;
        public int Score { get; set; }
        public int Errors { get; set; }
        public float TimeSpent { get; set; }
        public string SessionUid { get; set; } = string.Empty;
        public string Timestamp { get; set; } = string.Empty;
    }
}
