using MediatR;
using Template.Contract.Responses.Users;
using Template.Domain.Common;

namespace Template.Application.UseCases.Users.Queries.GetCurrentUserQuery
{
    public record GetCurrentUserQuery() : IRequest<Result<GetCurrentUserResponse>>;

}
