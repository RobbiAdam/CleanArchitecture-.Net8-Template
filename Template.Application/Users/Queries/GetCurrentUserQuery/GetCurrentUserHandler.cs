using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Template.Application.Common.Interfaces.Repositories;
using Template.Contract.Common.Bases;
using Template.Contract.Response.Users;

namespace Template.Application.Users.Queries.GetCurrentUserQuery
{
    public class GetCurrentUserHandler : IRequestHandler<GetCurrentUserQuery, BaseResponse<GetCurrentUserResponse>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _contextAccessor;

        public GetCurrentUserHandler(IHttpContextAccessor contextAccessor, IUserRepository userRepository)
        {
            _contextAccessor = contextAccessor;
            _userRepository = userRepository;
        }

        public async Task<BaseResponse<GetCurrentUserResponse>> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<GetCurrentUserResponse>();
            var user = GetCurrentUser();
            try
            {
                var currentUser = await _userRepository.GetByEmailAsync(user);
                response.Success = currentUser != null;
                response.Message = response.Success ? "User found" : "User not found";
                response.Data = currentUser.Adapt<GetCurrentUserResponse>();
            }
            catch(Exception ex)
            {
                response.Success = false;
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
