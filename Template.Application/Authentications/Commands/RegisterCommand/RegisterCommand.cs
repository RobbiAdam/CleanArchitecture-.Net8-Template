using MediatR;

namespace Template.Application.Commands.Authentications.Register
{
    public sealed record RegisterCommand(
        string UserName,
        string Name,
        string Email,
        string Password) : IRequest<string>;

}
