using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp;
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
        
        /// <summary>
        /// Creates or updates a user.
        /// </summary>
        /// <param name="Username">The username of the user.</param>
        /// <param name="Email">The email of the user.</param>
        /// <param name="BirthDate">The birth date of the user.</param>
        /// <param name="photo">The photo of the user (optional).</param>
        /// <returns>The created or updated user.</returns>
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

        /// <summary>
        /// Gets a user by ID.
        /// </summary>
        /// <param name="id">The ID of the user.</param>
        /// <returns>The user with the specified ID.</returns>
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
        
        /// <summary>
        /// Gets a user by email.
        /// </summary>
        /// <param name="email">The email of the user.</param>
        /// <returns>The user with the specified email.</returns>
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
        
        /// <summary>
        /// Deletes a user by ID.
        /// </summary>
        /// <param name="id">The ID of the user to delete.</param>
        /// <returns>A status indicating the result of the delete operation.</returns>
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
        
        /// <summary>
        /// Deletes a user by email.
        /// </summary>
        /// <param name="email">The email of the user to delete.</param>
        /// <returns>A status indicating the result of the delete operation.</returns>
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
        
        /// <summary>
        /// Gets all users.
        /// </summary>
        /// <returns>A dictionary of all users with their IDs and usernames.</returns>
        [HttpGet("allUsers")]
        public async Task<ActionResult<Dictionary<int, string>>> GetAllUsers()
        {
            try
            {
                var users = await _context.Users
                    .ToDictionaryAsync(u => u.Id, u => u.Username);

                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching all users");
                return StatusCode(500, "Internal server error");
            }
        }
        
        /// <summary>
        /// Converts a photo to a byte array.
        /// </summary>
        /// <param name="photo">The photo to convert.</param>
        /// <returns>A byte array representation of the photo.</returns>
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
                memoryStream.Position = 0;

                using var image = await Image.LoadAsync(memoryStream);
                var encoder = new JpegEncoder { Quality = 50 };

                using var compressedStream = new MemoryStream();
                await image.SaveAsync(compressedStream, encoder);
                return compressedStream.ToArray();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error converting photo to byte array");
                throw new InvalidOperationException("Error converting photo to byte array", ex);
            }
        }
    }
}
