using MediatR;
using Template.Contract.Common.Bases;

namespace Template.Application.Commands.Authentications.Register
{
    public sealed record RegisterCommand(
        string UserName,
        string Name,
        string Email,
        string Password) : IRequest<BaseResponse<string>>;

}
