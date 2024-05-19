using MediatR;
using Template.Application.Common.Interfaces;
using Template.Application.Common.Interfaces.Authentication;
using Template.Application.Common.Interfaces.Repositories;
using Template.Contract.Common.Bases;

namespace Template.Application.Authentications.Commands.LoginCommand
{
    internal sealed record LoginCommandHandler : IRequestHandler<LoginCommand, BaseResponse<string>>
    {
        private readonly IPasswordHash _passwordHasher;
        private readonly IUserRepository _userRepository;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public LoginCommandHandler(
            IJwtTokenGenerator jwtTokenGenerator,
            IUserRepository userRepository,
            IPasswordHash passwordHasher)
        {
            _jwtTokenGenerator = jwtTokenGenerator;
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }
        public async Task<BaseResponse<string>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<string>();
            var user = await _userRepository.GetByEmailAsync(request.Email);

            if (user == null || !_passwordHasher.VerifyPassword(request.Password, user.Password))
            {
                response.Success = false;
                response.Message = "Invalid Email or Password";
                return response;
            }

            try
            {
                var roles = user.IsAdmin ? "admin" : "user";
                var token = _jwtTokenGenerator.GenerateToken(user.Id, user.UserName, user.Email, roles);

                response.Success = true;
                response.Message = "Login Successful";
                response.Data = token;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }
    }
}
