using Template.Application.Common.Interfaces;
using Template.Application.Common.Interfaces.Repositories;
using Template.Domain.Entities;
using MediatR;
using Template.Application.Commands.Authentications.Register;

namespace Template.Application.Authentications.Commands.Register
{
    internal sealed class RegisterCommandHandler : IRequestHandler<RegisterCommand, string>
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

        public async Task<string> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {

            if (await _userRepository.GetByEmailAsync(request.Email) != null)
            {
                throw new Exception("Email already registered");
            }
            var newUser = new User
            {
                UserName = request.UserName,
                Name = request.Name,
                Email = request.Email,
                Password = _passwordHasher.HashPassword(request.Password),
            };
            await _userRepository.AddUserAsync(newUser);

            return newUser.Id;
        }
    }
}
