using MediatR;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Template.Application.Common.Interfaces;
using Template.Application.Common.Interfaces.Repositories;
using Template.Contract.Common.Bases;

namespace Template.Application.Authentications.Commands.ChangePasswordCommand
{
    internal sealed class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, BaseResponse<bool>>
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IPasswordHash _passwordHasher;
        private readonly IUserRepository _userRepository;

        public ChangePasswordCommandHandler(
            IUserRepository userRepository,
            IPasswordHash passwordHasher,
            IHttpContextAccessor contextAccessor)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _contextAccessor = contextAccessor;
        }

        public async Task<BaseResponse<bool>> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<bool>();

            var currentUser = GetCurrentUser();
            if (string.IsNullOrEmpty(currentUser))
            {
                response.Success = false;
                response.Message = "Invalid User";
                return response;
            }
            try
            {
                var user = await _userRepository.GetByEmailAsync(currentUser);
                if (user == null)
                {
                    response.Success = false;
                    response.Message = "User not found";
                    return response;
                }

                if (!_passwordHasher.VerifyPassword(request.OldPassword, user.Password))
                {
                    response.Success = false;
                    response.Message = "Invalid Password";
                    return response;
                }

                user.Password = _passwordHasher.HashPassword(request.NewPassword);
                await _userRepository.UpdateUserAsync(user);

                response.Success = true;
                response.Data = true;
                response.Message = "Password changed successfully";
                return response;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
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
