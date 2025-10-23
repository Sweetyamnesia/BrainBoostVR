using Microsoft.AspNetCore.Mvc;
using BrainBoostVR_API.Data;
using BrainBoostVR_API.Models;
using Microsoft.EntityFrameworkCore;

namespace BrainBoostVR_API.Controllers
{
    [Route("api/scores")]
	[ApiController]

    public class ScoresController : ControllerBase
    {
        private readonly BrainBoostDbContext _context;

        public ScoresController(BrainBoostDbContext context)
        {
            _context = context;
        }

        // POST /api/scores
        [HttpPost]
        public async Task<IActionResult> SubmitScore([FromBody] Score score)
        {
            if (score == null)
                return BadRequest("Invalid score data.");

            _context.Scores.Add(score);
            await _context.SaveChangesAsync();

            return Ok(new { status = "success", scoreId = score.ScoreID });
        }

        // GET /api/scores/{userID}
        [HttpGet("{userID}")]
        public async Task<IActionResult> GetScores(int userID)
        {
            var scores = await _context.Scores
                .Where(s => s.UserID == userID)
                .ToListAsync();

            return Ok(scores);
        }
    }
}
