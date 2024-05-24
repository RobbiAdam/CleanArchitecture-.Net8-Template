using MediatR;
using Template.Application.Common.Interfaces;
using Template.Application.Common.Interfaces.Repositories;
using Template.Domain.Common;
using Template.Domain.Users;

namespace Template.Application.UseCases.Authentications.Commands.RegisterCommand
{
    internal sealed class RegisterHandler : IRequestHandler<RegisterCommand, Result<string>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHash _passwordHasher;

        public RegisterHandler(
            IUserRepository userRepository,
            IPasswordHash passwordHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task<Result<string>> Handle(RegisterCommand request, CancellationToken ct)
        {
            if (await _userRepository.GetByEmailAsync(request.Email) != null)
            {
                return UserErrors.EmailAlreadyExist;
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
                string result = newUser.Id;
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
