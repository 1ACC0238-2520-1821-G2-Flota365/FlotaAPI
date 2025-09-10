using BackendWeb.Auth.Domain;
using BackendWeb.Auth.Application.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BackendWeb.Auth.Application
{
    public class AuthService
    {
        private readonly IUserRepository _repository;
        private readonly PasswordHasher<User> _passwordHasher;
        private readonly IConfiguration _configuration;

        public AuthService(IUserRepository repository, IConfiguration configuration)
        {
            _repository = repository;
            _passwordHasher = new PasswordHasher<User>();
            _configuration = configuration;
        }

        public async Task<UserDto> Register(RegisterRequest request)
        {
            if (await _repository.GetByEmailAsync(request.Email) != null)
                throw new InvalidOperationException("El correo ya está registrado.");

            var hashedPassword = _passwordHasher.HashPassword(null!, request.Password);
            
            var user = new User(request.FirstName, request.LastName, request.Email, hashedPassword, request.Role);
            await _repository.AddAsync(user);
            
            return MapToDto(user);
        }

        public async Task<UserDto> Login(LoginRequest request)
        {
            var user = await _repository.GetByEmailAsync(request.Email)
                ?? throw new UnauthorizedAccessException("Usuario no encontrado.");

            if (!user.IsActive)
                throw new UnauthorizedAccessException("El usuario está inactivo.");

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
            if (result == PasswordVerificationResult.Failed)
                throw new UnauthorizedAccessException("Contraseña incorrecta.");

            return MapToDto(user);
        }

        private string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<UserDto?> GetUserByIdAsync(int id)
        {
            var user = await _repository.GetByIdAsync(id);
            return user != null ? MapToDto(user) : null;
        }

        public async Task<UserDto?> GetUserByEmailAsync(string email)
        {
            var user = await _repository.GetByEmailAsync(email);
            return user != null ? MapToDto(user) : null;
        }

        public async Task<List<UserDto>> GetAllUsersAsync()
        {
            var users = await _repository.GetAllAsync();
            return users.Select(MapToDto).ToList();
        }

        public async Task<UserDto?> UpdateProfileAsync(int id, UpdateProfileRequest request)
        {
            var user = await _repository.GetByIdAsync(id);
            if (user == null) return null;

            user.UpdateProfile(request.FirstName, request.LastName);
            await _repository.UpdateAsync(user);

            return MapToDto(user);
        }

        public async Task<bool> ChangePasswordAsync(int id, ChangePasswordRequest request)
        {
            var user = await _repository.GetByIdAsync(id);
            if (user == null) return false;

            // Verify current password
            var verificationResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.CurrentPassword);
            if (verificationResult == PasswordVerificationResult.Failed)
                throw new UnauthorizedAccessException("Contraseña actual incorrecta.");

            // Hash new password and update
            var newHashedPassword = _passwordHasher.HashPassword(user, request.NewPassword);
            user.UpdatePassword(newHashedPassword);
            
            await _repository.UpdateAsync(user);
            
            return true;
        }

        public async Task<bool> DeactivateUserAsync(int id)
        {
            var user = await _repository.GetByIdAsync(id);
            if (user == null) return false;

            user.SetActive(false);
            await _repository.UpdateAsync(user);

            return true;
        }

        private static UserDto MapToDto(User user)
        {
            return new UserDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt
            };
        }
    }
}
