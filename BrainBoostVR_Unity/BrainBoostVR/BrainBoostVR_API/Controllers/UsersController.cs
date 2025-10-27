using BrainBoostVR_API.Data;
using BrainBoostVR_API.Models;
using Microsoft.AspNetCore.Mvc;

namespace BrainBoostVR_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsersController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto dto)
        {
            var user = new User
            {
                FirebaseUID = dto.FirebaseUID,
                Name = dto.Name,
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(user);
        }
    }
}
