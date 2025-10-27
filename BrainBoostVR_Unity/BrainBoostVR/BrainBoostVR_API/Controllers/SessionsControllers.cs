using Microsoft.AspNetCore.Mvc;
using BrainBoostVR_API.Models;
using BrainBoostVR_API.Services;

namespace BrainBoostVR_API.Controllers
{
    [Route("api/sessions")]
    [ApiController]
    public class SessionsController : ControllerBase
    {
        private readonly FirebaseService _firebaseService;

        public SessionsController(FirebaseService firebaseService)
        {
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
        public async Task<IActionResult> CreateSession([FromBody] Session session)
        {
            if (!await IsAuthorizedAsync())
                return Unauthorized("Invalid Firebase token.");

            // insert dans DB (plus tard)
            return Ok(new { status = "success" });
        }

        [HttpGet("{userID}")]
        public async Task<IActionResult> GetSessions(string userID)
        {
            if (!await IsAuthorizedAsync())
                return Unauthorized("Invalid Firebase token.");

            // récupérer sessions de l'utilisateur (mock)
            return Ok(new[]
            {
                new { startTime = "2025-10-19T20:40:53", endTime = "2025-10-19T20:50:53", durationMinutes = 10.0 }
            });
        }
    }
}
