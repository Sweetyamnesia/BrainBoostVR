using Microsoft.AspNetCore.Mvc;
using BrainBoostVR_API.Models;

namespace BrainBoostVR_API.Controllers
{
    [Route("api/sessions")]
    [ApiController]
    public class SessionsController : ControllerBase
    {
        // POST /api/sessions
        [HttpPost]
        public IActionResult CreateSession([FromBody] Session session)
        {
            // Ici : insert dans DB
            return Ok(new { status = "success" });
        }

        // GET /api/sessions/{firebaseUID}
        [HttpGet("{firebaseUID}")]
        public IActionResult GetSessions(string firebaseUID)
        {
            // Ici : récupérer les sessions de l'utilisateur
            return Ok(new[]
            {
                new { startTime = "2025-10-19T20:40:53", endTime = "2025-10-19T20:50:53", durationMinutes = 10.0 }
            });
        }
    }
}
