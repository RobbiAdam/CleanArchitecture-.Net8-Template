using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Template.Application.Common.Interfaces;
using Template.Application.Common.Interfaces.Authentication;
using Template.Application.Common.Interfaces.Repositories;
using Template.Contract.Authentications;

using Template.Domain.Entities;

namespace Template.Application.Services.Users
{
    public class AuthService : IAuthService
    {
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHash _passwordHasher;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthService
            (IUserRepository userRepository,
            IJwtTokenGenerator jwtTokenGenerator,
            IPasswordHash passwordHasher,
            IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository;
            _jwtTokenGenerator = jwtTokenGenerator;
            _passwordHasher = passwordHasher;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> ChangePasswordAsync(ChangePasswordRequest request)
        {
            var currentUser = GetCurrentUser();
            if (string.IsNullOrEmpty(currentUser))
            {
                throw new UnauthorizedAccessException("Invalid user");
            }

            var user = await _userRepository.GetByEmailAsync(currentUser);
            if (user == null)
            {
                throw new UnauthorizedAccessException("User not found");
            }

            if (!_passwordHasher.VerifyPassword(request.OldPassword, user.Password))
            {
                throw new Exception("Incorrect old password");
            }

            user.Password = _passwordHasher.HashPassword(request.NewPassword);
            await _userRepository.UpdateUserAsync(user);

            return true;
        }

        public async Task<string> LoginAsync(LoginRequest request)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email);

            if (user == null || !_passwordHasher.VerifyPassword(request.Password, user.Password))
            {
                throw new Exception("User or Password is Incorrect");
            }

            var roles = user.IsAdmin ? "admin" : "user";

            var token = _jwtTokenGenerator.GenerateToken(user.Id, user.UserName, user.Email, roles);

            return token;
        }

        public async Task<bool> RegisterAsync(RegisterRequest request)
        {            
            var newUser = new User
            {
                UserName = request.UserName,
                Name = request.Name,
                Email = request.Email,
                Password = _passwordHasher.HashPassword(request.Password),
            };
            if (await _userRepository.GetByEmailAsync(newUser.Email) != null)
            {
                throw new Exception("User already exists");
            }
            await _userRepository.AddUserAsync(newUser);
            return true;

        }
        private string GetCurrentUser()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            return httpContext?.User?.Identity is ClaimsIdentity identity
                ? identity.FindFirst(ClaimTypes.Email)?.Value ?? string.Empty
                : string.Empty;
        }
    }
}
