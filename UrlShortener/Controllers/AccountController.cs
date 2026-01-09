using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UrlShortener.Data;
using UrlShortener.Models.DTOs;

namespace UrlShortener.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AccountController> _logger;

        public AccountController(
            ApplicationDbContext context,
            ILogger<AccountController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorResponse("Invalid input"));
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(u =>
                    u.Login == request.Login &&
                    u.PasswordHash == request.Password);

            if (user == null)
            {
                _logger.LogWarning("Failed login attempt for user: {Login}", request.Login);
                return Unauthorized(new ErrorResponse("Invalid login or password"));
            }

            _logger.LogInformation("User {Login} logged in successfully", user.Login);

            return Ok(new LoginResponse
            {
                Id = user.Id,
                Login = user.Login,
                IsAdmin = user.IsAdmin,
                Message = "Login successful"
            });
        }

        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<LoginResponse>> Register([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorResponse("Invalid input"));
            }

            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Login == request.Login);

            if (existingUser != null)
            {
                return BadRequest(new ErrorResponse("User with this login already exists"));
            }

            var user = new Models.Entities.User
            {
                Login = request.Login,
                PasswordHash = request.Password,
                IsAdmin = false,
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            _logger.LogInformation("New user registered: {Login}", user.Login);

            return CreatedAtAction(nameof(Login), new LoginResponse
            {
                Id = user.Id,
                Login = user.Login,
                IsAdmin = user.IsAdmin,
                Message = "Registration successful"
            });
        }
    }
}
