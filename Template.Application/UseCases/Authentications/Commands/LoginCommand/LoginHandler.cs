using MediatR;
using Template.Application.Common.Interfaces;
using Template.Application.Common.Interfaces.Authentication;
using Template.Application.Common.Interfaces.Repositories;
using Template.Domain.Common;
using Template.Domain.Users;

namespace Template.Application.UseCases.Authentications.Commands.LoginCommand
{
    internal sealed record LoginHandler : IRequestHandler<LoginCommand, Result<string>>
    {
        private readonly IPasswordHash _passwordHasher;
        private readonly IUserRepository _userRepository;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public LoginHandler(
            IJwtTokenGenerator jwtTokenGenerator,
            IUserRepository userRepository,
            IPasswordHash passwordHasher)
        {
            _jwtTokenGenerator = jwtTokenGenerator;
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }
        public async Task<Result<string>> Handle(LoginCommand request, CancellationToken ct)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email);

            if (user == null || !_passwordHasher.VerifyPassword(request.Password, user.Password))
            {                
                return UserErrors.InvalidEmailOrPassword;
            }
            try
            {
                var roles = user.IsAdmin ? "admin" : "user";
                var token = _jwtTokenGenerator.GenerateToken(user.Id, user.UserName, user.Email, roles);

                return token;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
