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

        private async Task<bool> IsAuthorizedAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization")) return false;

            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            try
            {
                await _firebaseService.VerifyTokenAsync(token);
                return true;
            }
            catch
            {
                return false;
            }
        }

        [HttpPost]
        public async Task<IActionResult> SubmitScore([FromBody] Score score)
        {
            if (!await IsAuthorizedAsync())
                return Unauthorized("Invalid Firebase token.");

            _context.Scores.Add(score);
            await _context.SaveChangesAsync();
            return Ok(new { status = "success", scoreId = score.ScoreID });
        }

        [HttpGet("{userID}")]
        public async Task<IActionResult> GetScores(int userID)
        {
            if (!await IsAuthorizedAsync())
                return Unauthorized("Invalid Firebase token.");

            var scores = await _context.Scores.Where(s => s.UserID == userID).ToListAsync();
            return Ok(scores);
        }
    }
}
