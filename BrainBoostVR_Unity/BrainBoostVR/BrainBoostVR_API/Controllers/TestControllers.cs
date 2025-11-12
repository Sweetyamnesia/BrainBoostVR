using Microsoft.AspNetCore.Mvc;
using System;

namespace BrainBoostVR_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            if (!HttpContext.Items.ContainsKey("FirebaseUid"))
                return Unauthorized("Firebase UID non trouvé");

            var uid = HttpContext.Items["FirebaseUid"].ToString();
            Console.WriteLine($"[Controller] Requête reçue pour UID : {uid}");

            return Ok(new
            {
                message = "Connexion API réussie 🎉",
                firebaseUid = uid
            });
        }
    }
}
