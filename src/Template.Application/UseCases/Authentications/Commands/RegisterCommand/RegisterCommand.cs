using MediatR;
using Template.Domain.Common;

namespace Template.Application.UseCases.Authentications.Commands.RegisterCommand
{
    public sealed record RegisterCommand(
        string UserName,
        string Name,
        string Email,
        string Password) : IRequest<Result<string>>;

}
