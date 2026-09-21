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

        public ScoresController(
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

            var authorization =
                Request.Headers["Authorization"].ToString();

            if (!authorization.StartsWith("Bearer "))
                return null;

            var token =
                authorization.Substring("Bearer ".Length).Trim();

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
        // Vérifie que FirebaseUID peut utiliser le profil
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
        // ENREGISTRER UN SCORE
        // POST /api/scores
        // ============================================================
        [HttpPost]
        public async Task<IActionResult> SubmitScore(
            [FromBody] UnityScoreDto dto)
        {
            if (dto == null)
                return BadRequest("Payload invalide.");

            Console.WriteLine(
                "[API] 🔹 Reçu POST /api/scores avec DTO: " +
                System.Text.Json.JsonSerializer.Serialize(dto)
            );

            // --------------------------------------------------------
            // 1. Vérification du token Firebase
            // --------------------------------------------------------
            var firebaseUid = await VerifyAndGetUidAsync();

            if (firebaseUid == null)
                return Unauthorized(
                    "Invalid or missing Firebase token."
                );

            // --------------------------------------------------------
            // 2. Vérification du profil sélectionné
            // --------------------------------------------------------
            if (dto.UserID <= 0)
                return BadRequest("UserID est obligatoire.");

            // --------------------------------------------------------
            // 3. Vérification que FirebaseUID peut utiliser ce profil
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
            // 4. Vérification que le profil existe
            // --------------------------------------------------------
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.UserID == dto.UserID);

            if (user == null)
                return NotFound("Profil utilisateur introuvable.");

            // --------------------------------------------------------
            // 5. Création du score
            // --------------------------------------------------------
            var score = new Score
            {
                UserID = user.UserID,
                Value = dto.Score,
                Errors = dto.Errors,
                TimeSpent = dto.TimeSpent,
                SessionUid = dto.SessionUid,

                Timestamp =
                    DateTime.TryParse(
                        dto.Timestamp,
                        out var ts)
                        ? ts
                        : DateTime.UtcNow,

                ExerciseID = dto.ExerciseID
            };

            // --------------------------------------------------------
            // 6. Sauvegarde
            // --------------------------------------------------------
            try
            {
                _context.Scores.Add(score);
                await _context.SaveChangesAsync();

                Console.WriteLine(
                    $"[API][ScoresController] ✅ Score enregistré " +
                    $"pour UserID={user.UserID}, " +
                    $"Pseudo={user.Name}, " +
                    $"ScoreID={score.ScoreID}"
                );

                return Ok(new
                {
                    status = "success",
                    userID = user.UserID,
                    name = user.Name,
                    scoreId = score.ScoreID,
                    savedAt = score.Timestamp
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    "[API][ScoresController] ❌ Erreur SaveChanges: " +
                    ex.Message
                );

                return StatusCode(
                    500,
                    "Erreur lors de l'enregistrement du score"
                );
            }
        }

        // ============================================================
        // RÉCUPÉRER LES SCORES D'UN PROFIL
        // GET /api/scores?userID=...
        // ============================================================
        [HttpGet]
        public async Task<IActionResult> GetScores(
            [FromQuery] int userID)
        {
            var firebaseUid = await VerifyAndGetUidAsync();

            if (firebaseUid == null)
                return Unauthorized(
                    "Invalid or missing Firebase token."
                );

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
            // Récupération des scores du profil
            // --------------------------------------------------------
            var scores = await _context.Scores
                .Where(s => s.UserID == userID)
                .OrderByDescending(s => s.Timestamp)
                .ToListAsync();

            return Ok(scores);
        }
    }

    // ================================================================
    // DTO utilisé pour la réception depuis Unity
    // ================================================================
    public class UnityScoreDto
    {
        public int UserID { get; set; }

        public string FirebaseUID { get; set; } = string.Empty;

        public int Score { get; set; }

        public int Errors { get; set; }

        public float TimeSpent { get; set; }

        public string SessionUid { get; set; } = string.Empty;

        public string Timestamp { get; set; } = string.Empty;

        public int ExerciseID { get; set; }
    }
}