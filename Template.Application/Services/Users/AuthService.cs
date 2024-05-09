using Template.Application.Common.Interfaces;
using Template.Application.Common.Interfaces.Authentication;
using Template.Contract.Authentications;

using Template.Domain.Entities;

namespace Template.Application.Services.Users
{
    public class AuthService : IAuthService
    {
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHash _passwordHasher;

        public AuthService
            (IUserRepository userRepository, 
            IJwtTokenGenerator jwtTokenGenerator, 
            IPasswordHash passwordHasher)
        {
            _userRepository = userRepository;
            _jwtTokenGenerator = jwtTokenGenerator;
            _passwordHasher = passwordHasher;
        }

        public async Task<string> LoginAsync(LoginRequest request)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email);

            if (user == null || !_passwordHasher.VerifyPassword(request.Password, user.Password))
            {
                throw new Exception("User or Password is Incorrect");
            }

            var roles = user.IsAdmin ? "admin" : "user";

            var token = _jwtTokenGenerator.GenerateToken(user.Id.ToString(), user.UserName, user.Email, roles);

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
            try
            {
                await _userRepository.AddUserAsync(newUser);
            }
             catch
            {
                throw new Exception("User already exists");
            }
            return true;

        }
    }
}
