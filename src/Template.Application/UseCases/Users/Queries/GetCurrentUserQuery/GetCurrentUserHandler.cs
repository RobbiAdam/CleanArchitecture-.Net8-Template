using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Template.Application.Common.Interfaces.Repositories;
using Template.Contract.Responses.Users;
using Template.Domain.Common;
using Template.Domain.Users;

namespace Template.Application.UseCases.Users.Queries.GetCurrentUserQuery
{
    public class GetCurrentUserHandler : IRequestHandler<GetCurrentUserQuery, Result<GetCurrentUserResponse>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _contextAccessor;

        public GetCurrentUserHandler(IHttpContextAccessor contextAccessor, IUserRepository userRepository)
        {
            _contextAccessor = contextAccessor;
            _userRepository = userRepository;
        }

        public async Task<Result<GetCurrentUserResponse>> Handle(GetCurrentUserQuery request, CancellationToken ct)
        {            
            var user = GetCurrentUser();
            try
            {
                var currentUser = await _userRepository.GetByEmailAsync(user);
                if (currentUser == null)
                {
                    return UserErrors.UserNotFound;
                }
                return currentUser.Adapt<GetCurrentUserResponse>();               
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
