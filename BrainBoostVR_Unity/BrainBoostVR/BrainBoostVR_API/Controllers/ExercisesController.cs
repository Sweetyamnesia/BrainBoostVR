using Microsoft.AspNetCore.Mvc;
using BrainBoostVR_API.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace BrainBoostVR_API.Controllers
{
    [Route("api/exercises")]
    [ApiController]
    public class ExercisesController : ControllerBase
    {
        private readonly BrainBoostDbContext _context;

        public ExercisesController(BrainBoostDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetExercises()
        {
            var exercises = await _context.Exercises
                .OrderBy(e => e.ExerciseID)
                .ToListAsync();

            return Ok(exercises);
        }
    }
}