using MediatR;
using Template.Contract.Common.Bases;
using Template.Contract.Response.Users;

namespace Template.Application.Users.Queries.GetCurrentUserQuery
{
    public record GetCurrentUserQuery() : IRequest<BaseResponse<GetCurrentUserResponse>>;

}
