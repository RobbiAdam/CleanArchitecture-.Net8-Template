using MediatR;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Template.Application.Common.Interfaces;
using Template.Application.Common.Interfaces.Repositories;
using Template.Domain.Common;
using Template.Domain.Users;

namespace Template.Application.UseCases.Authentications.Commands.ChangePasswordCommand
{
    internal sealed class ChangePasswordHandler : IRequestHandler<ChangePasswordCommand, Result<bool>>
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IPasswordHash _passwordHasher;
        private readonly IUserRepository _userRepository;

        public ChangePasswordHandler(
            IUserRepository userRepository,
            IPasswordHash passwordHasher,
            IHttpContextAccessor contextAccessor)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _contextAccessor = contextAccessor;
        }

        public async Task<Result<bool>> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {            

            var currentUser = GetCurrentUser();
            if (string.IsNullOrEmpty(currentUser))
            {                
                return UserErrors.InvalidUser;
            }
            try
            {
                var user = await _userRepository.GetByEmailAsync(currentUser);
                if (user == null)
                {
                    return UserErrors.UserNotFound;
                }

                if (!_passwordHasher.VerifyPassword(request.OldPassword, user.Password))
                {                    
                    return UserErrors.InvalidPassword;
                }

                user.Password = _passwordHasher.HashPassword(request.NewPassword);
                await _userRepository.UpdateUserAsync(user).ConfigureAwait(false);
                
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private string GetCurrentUser()
        {
            var httpContext = _contextAccessor.HttpContext;
            return httpContext?.User?.Identity is ClaimsIdentity identity
                ? identity.FindFirst(ClaimTypes.Email)?.Value ?? string.Empty
                : string.Empty;
        }
    }
}
