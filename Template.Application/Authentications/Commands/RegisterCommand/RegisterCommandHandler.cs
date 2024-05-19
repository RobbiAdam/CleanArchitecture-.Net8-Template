using MediatR;
using Template.Application.Common.Interfaces;
using Template.Application.Common.Interfaces.Repositories;
using Template.Contract.Common.Bases;
using Template.Domain.Entities;

namespace Template.Application.Authentications.Commands.RegisterCommand
{
    internal sealed class RegisterCommandHandler : IRequestHandler<RegisterCommand, BaseResponse<string>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHash _passwordHasher;

        public RegisterCommandHandler(
            IUserRepository userRepository,
            IPasswordHash passwordHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task<BaseResponse<string>> Handle(RegisterCommand request, CancellationToken ct)
        {
            var response = new BaseResponse<string>();

            if (await _userRepository.GetByEmailAsync(request.Email) != null)
            {
                response.Success = false;
                response.Message = "Email already exists";
                return response;
            }
            try
            {
                var newUser = new User
                {
                    UserName = request.UserName,
                    Name = request.Name,
                    Email = request.Email,
                    Password = _passwordHasher.HashPassword(request.Password),
                };
                await _userRepository.AddUserAsync(newUser);

                response.Success = true;
                response.Data = newUser.Id;
                response.Message = "User created successfully";
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }
    }
}
