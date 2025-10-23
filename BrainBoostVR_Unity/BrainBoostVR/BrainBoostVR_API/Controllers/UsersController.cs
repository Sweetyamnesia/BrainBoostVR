using Microsoft.AspNetCore.Mvc;
using BrainBoostVR_API.Models;

namespace BrainBoostVR_API.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        // POST /api/users
        [HttpPost]
        public IActionResult CreateUser([FromBody] User user)
        {
            // Ici : insert dans DB
            return Ok(new { status = "success", firebaseUID = user.FirebaseUID, name = user.Name });
        }

        // GET /api/users/{firebaseUID}
        [HttpGet("{firebaseUID}")]
        public IActionResult GetUser(string firebaseUID)
        {
            // Ici : récupérer l'utilisateur depuis la DB
            return Ok(new { firebaseUID = firebaseUID, name = "Nom d'exemple" });
        }
    }
}
