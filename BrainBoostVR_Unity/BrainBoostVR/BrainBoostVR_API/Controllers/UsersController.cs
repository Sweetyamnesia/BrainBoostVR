using BrainBoostVR_API.Data;
using BrainBoostVR_API.Models;
using BrainBoostVR_API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BrainBoostVR_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly BrainBoostDbContext _context;
        private readonly FirebaseService _firebaseService;

        public UsersController(
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
        // Crée un nouveau profil ou signale qu'un pseudo existe déjà
        // POST /api/users/create-or-get
        // ============================================================
        [HttpPost("create-or-get")]
        public async Task<IActionResult> CreateOrGetUser(
            [FromBody] CreateUserDto dto)
        {
            if (dto == null)
                return BadRequest("Payload invalide.");

            if (string.IsNullOrWhiteSpace(dto.FirebaseUID))
                return BadRequest("FirebaseUID manquant.");

            if (string.IsNullOrWhiteSpace(dto.Name))
                return BadRequest("Pseudo manquant.");

            string pseudo = dto.Name.Trim();

            // --------------------------------------------------------
            // 1. Vérifie que le FirebaseUID envoyé correspond
            //    bien au token Firebase authentifié.
            // --------------------------------------------------------
            var firebaseUid = await VerifyAndGetUidAsync();

            if (firebaseUid == null)
                return Unauthorized(
                    "Invalid or missing Firebase token."
                );

            if (firebaseUid != dto.FirebaseUID)
            {
                return Unauthorized(
                    "Le FirebaseUID ne correspond pas au token Firebase."
                );
            }

            // --------------------------------------------------------
            // 2. Recherche du pseudo
            // --------------------------------------------------------
            var userByName = await _context.Users
                .FirstOrDefaultAsync(u => u.Name == pseudo);

            if (userByName != null)
            {
                return Conflict(new
                {
                    status = "pseudo_exists",
                    message = "Ce pseudo existe déjà.",
                    userID = userByName.UserID,
                    name = userByName.Name
                });
            }

            // --------------------------------------------------------
            // 3. Le pseudo n'existe pas :
            //    création du nouveau profil
            // --------------------------------------------------------
            var newUser = new User
            {
                // Conservé temporairement pour compatibilité
                // avec les anciennes données.
                FirebaseUID = firebaseUid,
                Name = pseudo,
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            // --------------------------------------------------------
            // 4. Création de l'association Firebase ↔ profil
            // --------------------------------------------------------
            var firebaseProfile = new FirebaseProfile
            {
                FirebaseUID = firebaseUid,
                UserID = newUser.UserID
            };

            _context.FirebaseProfiles.Add(firebaseProfile);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                status = "created",
                userID = newUser.UserID,
                firebaseUID = firebaseUid,
                name = newUser.Name,
                createdAt = newUser.CreatedAt
            });
        }

        // ============================================================
        // Associer un FirebaseUID à un profil existant
        //
        // POST /api/users/link-profile
        // ============================================================
        [HttpPost("link-profile")]
        public async Task<IActionResult> LinkProfile(
            [FromBody] LinkProfileDto dto)
        {
            if (dto == null)
                return BadRequest("Payload invalide.");

            if (dto.UserID <= 0)
                return BadRequest("UserID invalide.");

            if (string.IsNullOrWhiteSpace(dto.FirebaseUID))
                return BadRequest("FirebaseUID manquant.");

            // --------------------------------------------------------
            // 1. Vérification du token Firebase
            // --------------------------------------------------------
            var firebaseUid = await VerifyAndGetUidAsync();

            if (firebaseUid == null)
                return Unauthorized(
                    "Invalid or missing Firebase token."
                );

            // --------------------------------------------------------
            // 2. Le FirebaseUID envoyé doit correspondre
            //    au FirebaseUID réellement authentifié.
            // --------------------------------------------------------
            if (firebaseUid != dto.FirebaseUID)
            {
                return Unauthorized(
                    "Le FirebaseUID ne correspond pas au token Firebase."
                );
            }

            // --------------------------------------------------------
            // 3. Vérifie que le profil existe
            // --------------------------------------------------------
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.UserID == dto.UserID);

            if (user == null)
                return NotFound("Profil utilisateur introuvable.");

            // --------------------------------------------------------
            // 4. Vérifie si l'association existe déjà
            // --------------------------------------------------------
            var existingLink = await _context.FirebaseProfiles
                .FirstOrDefaultAsync(fp =>
                    fp.FirebaseUID == firebaseUid &&
                    fp.UserID == user.UserID);

            if (existingLink != null)
            {
                return Ok(new
                {
                    status = "already_linked",
                    userID = user.UserID,
                    firebaseUID = firebaseUid,
                    name = user.Name
                });
            }

            // --------------------------------------------------------
            // 5. Création de l'association Firebase ↔ profil
            // --------------------------------------------------------
            var firebaseProfile = new FirebaseProfile
            {
                FirebaseUID = firebaseUid,
                UserID = user.UserID
            };

            try
            {
                _context.FirebaseProfiles.Add(firebaseProfile);
                await _context.SaveChangesAsync();

                Console.WriteLine(
                    $"[API][UsersController] ✅ Profil lié : " +
                    $"FirebaseUID={firebaseUid}, " +
                    $"UserID={user.UserID}, " +
                    $"Pseudo={user.Name}"
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    "[API][UsersController] ❌ Erreur liaison profil : " +
                    ex.Message
                );

                return StatusCode(
                    500,
                    "Erreur lors de l'association du profil."
                );
            }

            return Ok(new
            {
                status = "linked",
                userID = user.UserID,
                firebaseUID = firebaseUid,
                name = user.Name
            });
        }
    }

    // ================================================================
    // DTO utilisé pour associer un profil existant
    // ================================================================
    public class LinkProfileDto
    {
        public string FirebaseUID { get; set; } = string.Empty;

        public int UserID { get; set; }
    }
}