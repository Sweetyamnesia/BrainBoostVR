using Microsoft.AspNetCore.Mvc;
using BrainBoostVR_API.Data;
using BrainBoostVR_API.Models;
using BrainBoostVR_API.Services;
using Microsoft.EntityFrameworkCore;

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

		// Vérifie le token Firebase et retourne le FirebaseUID
		private async Task<string?> VerifyAndGetUidAsync()
		{
			if (!Request.Headers.ContainsKey("Authorization")) return null;

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

		// Enregistrer un score envoyé depuis Unity
		[HttpPost]
		public async Task<IActionResult> SubmitScore([FromBody] UnityScoreDto dto)
		{
			var firebaseUid = await VerifyAndGetUidAsync();
			if (firebaseUid == null)
				return Unauthorized("Invalid or missing Firebase token.");

			// Vérifier que l'utilisateur existe
			var user = await _context.Users.FirstOrDefaultAsync(u => u.FirebaseUID == dto.FirebaseUID);
			if (user == null)
			{
				// Créer l'utilisateur s'il n'existe pas
				user = new User
				{
					FirebaseUID = dto.FirebaseUID,
					Name = "Unknown"
				};
				_context.Users.Add(user);
				await _context.SaveChangesAsync();
			}

			_context.Scores.Add(score);
			await _context.SaveChangesAsync();
			return Ok(new
			{
				status = "success",
				user = user.FirebaseUID,
				scoreId = score.ScoreID,
				savedAt = score.Timestamp
			});
		}

        // Récupérer les scores d'un utilisateur
		[HttpGet("{userID}")]
        public async Task<IActionResult> GetScores(string firebaseUID)
		{
			var firebaseUid = await VerifyAndGetUidAsync();
			if (firebaseUid == null)
				return Unauthorized("Invalid or missing Firebase token.");

			var user = await _context.Users.FirstOrDefaultAsync(u => u.FirebaseUID == firebaseUID);
			if (user == null)
				return NotFound("User not found");

			var scores = await _context.Scores
			.Where(s => s.UserID == userID)
			.OrderByDescending(s.Timestamp)
			.ToListAsync();
            
			return Ok(scores);
        }
    }
}
