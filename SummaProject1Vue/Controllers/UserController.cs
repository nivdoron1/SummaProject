using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace SummaProject1Vue.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<UsersController> _logger;

        public UsersController(ApplicationDbContext context, ILogger<UsersController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpPost("createOrUpdateUser")]
        public async Task<ActionResult<User>> CreateOrUpdateUser([FromForm] string Username, [FromForm] string Email, [FromForm] string BirthDate, [FromForm] IFormFile? photo = null)
        {
            try
            {
                var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == Email);
                byte[]? BytePhoto = null;

                if (photo != null)
                {
                    BytePhoto = await ConvertPhotoToByteArrayAsync(photo);
                }

                if (existingUser != null)
                {
                    // Update existing user
                    existingUser.Username = Username;
                    existingUser.BirthDate = BirthDate;
                    existingUser.Photo = BytePhoto;

                    _context.Users.Update(existingUser);
                    await _context.SaveChangesAsync();
                    return Ok(existingUser);
                }
                else
                {
                    // Create new user
                    User user = new User(Username, Email, BirthDate, BytePhoto);
                    _context.Users.Add(user);
                    await _context.SaveChangesAsync();
                    return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating or updating user");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUserById(int id)
        {
            try
            {
                var user = await _context.Users.FindAsync(id);

                if (user == null)
                {
                    return NotFound($"User with ID {id} not found.");
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching user");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("email/{email}")]
        public async Task<ActionResult<User>> GetUserByEmail(string email)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

                if (user == null)
                {
                    return NotFound($"User with email {email} not found.");
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching user by email");
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUserById(int id)
        {
            try
            {
                var user = await _context.Users.FindAsync(id);
                if (user == null)
                {
                    return NotFound($"User with ID {id} not found.");
                }

                _context.Users.Remove(user);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting user by ID");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("email/{email}")]
        public async Task<ActionResult> DeleteUserByEmail(string email)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
                if (user == null)
                {
                    return NotFound($"User with email {email} not found.");
                }

                _context.Users.Remove(user);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting user by email");
                return StatusCode(500, "Internal server error");
            }
        }

        private async Task<byte[]> ConvertPhotoToByteArrayAsync(IFormFile photo)
        {
            if (photo == null || photo.Length == 0)
            {
                throw new ArgumentException("Photo is null or empty.");
            }

            try
            {
                using var memoryStream = new MemoryStream();
                await photo.CopyToAsync(memoryStream);
                return memoryStream.ToArray();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error converting photo to byte array");
                throw new InvalidOperationException("Error converting photo to byte array", ex);
            }
        }

    }
}
