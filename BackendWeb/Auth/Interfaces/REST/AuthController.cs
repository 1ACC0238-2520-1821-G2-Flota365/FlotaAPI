using BackendWeb.Auth.Application;
using BackendWeb.Auth.Application.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace BackendWeb.Auth.Interfaces.REST
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _service;

        public AuthController(AuthService service) => _service = service;

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register([FromBody] RegisterRequest request)
        {
            try
            {
                var user = await _service.Register(request);
                return CreatedAtAction(nameof(Register), new { id = user.Id }, user);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login([FromBody] LoginRequest request)
        {
            try
            {
                var user = await _service.Login(request);
                return Ok(user);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }

        [HttpGet("profile/{id}")]
        public async Task<ActionResult<UserDto>> GetProfileById(int id)
        {
            var user = await _service.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound(new { message = "Usuario no encontrado." });
            }
            return Ok(user);
        }

        [HttpPut("profile/{id}")]
        public async Task<ActionResult<UserDto>> UpdateProfile(int id, [FromBody] UpdateProfileRequest request)
        {
            var user = await _service.UpdateProfileAsync(id, request);
            if (user == null)
            {
                return NotFound(new { message = "Usuario no encontrado." });
            }
            return Ok(user);
        }

        [HttpGet("profile")]
        public async Task<ActionResult<UserDto>> GetProfileByEmail([FromQuery] string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest(new { message = "El email es requerido." });
            }

            var user = await _service.GetUserByEmailAsync(email);
            if (user == null)
            {
                return NotFound(new { message = "Usuario no encontrado." });
            }
            return Ok(user);
        }

        [HttpPost("change-password/{id}")]
        public async Task<ActionResult> ChangePassword(int id, [FromBody] ChangePasswordRequest request)
        {
            try
            {
                var result = await _service.ChangePasswordAsync(id, request);
                if (!result)
                {
                    return NotFound(new { message = "Usuario no encontrado." });
                }
                return Ok(new { message = "Contraseña actualizada exitosamente." });
            }
            catch (UnauthorizedAccessException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("users")]
        public async Task<ActionResult<List<UserDto>>> GetAllUsers()
        {
            // TODO: Add authorization to check if user is admin
            var users = await _service.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpDelete("users/{id}")]
        public async Task<ActionResult> DeactivateUser(int id)
        {
            // TODO: Add authorization to check if user is admin
            var result = await _service.DeactivateUserAsync(id);
            if (!result)
            {
                return NotFound(new { message = "Usuario no encontrado." });
            }
            return Ok(new { message = "Usuario desactivado exitosamente." });
        }

        [HttpGet("health")]
        public ActionResult GetHealth()
        {
            return Ok(new 
            { 
                status = "healthy", 
                timestamp = DateTime.UtcNow,
                service = "Auth Service",
                version = "1.0.0"
            });
        }
    }
}
