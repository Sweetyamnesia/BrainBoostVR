using BrainBoostVR_API.Data;
using BrainBoostVR_API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BrainBoostVR_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly BrainBoostDbContext _context;

        public UsersController(BrainBoostDbContext context)
        {
            _context = context;
        }

        // create-or-get endpoint (exemple)
        [HttpPost("create-or-get")]
        public async Task<IActionResult> CreateOrGetUser([FromBody] CreateUserDto dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.FirebaseUID))
                return BadRequest("Invalid payload");

            // Correction du lambda : u => u.FirebaseUID == dto.FirebaseUID
            var user = await _context.Users.FirstOrDefaultAsync(u => u.FirebaseUID == dto.FirebaseUID);

            if (user == null)
            {
                user = new User
                {
                    FirebaseUID = dto.FirebaseUID,
                    Name = dto.Name ?? string.Empty,
                    CreatedAt = DateTime.UtcNow
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return Ok(new { status = "created", firebaseUID = user.FirebaseUID });
            }

            return Ok(new { status = "exists", firebaseUID = user.FirebaseUID });
        }
    }
}
