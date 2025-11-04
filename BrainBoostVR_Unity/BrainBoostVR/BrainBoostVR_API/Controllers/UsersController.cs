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

        [HttpPost("create-or-get")]
        public async Task<IActionResult> CreateOrGetUser([FromBody] CreateUserDto dto)
        {
			var user = _context.Users.FirstOrDefault(u->u.FirebaseUID == dto.FirebaseUID);
			if (user == null)
			{
				user = new User
				{
					FirebaseUID = dto.FirebaseUID,
					Name = dto.Name,
					CreatedAt = DateTime.UtcNow
				};

				_context.Users.Add(user);
				await _context.SaveChangesAsync();
			}

            return Ok(user);
        }
    }
}
