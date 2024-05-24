using MediatR;
using Template.Domain.Common;

namespace Template.Application.UseCases.Authentications.Commands.LoginCommand
{
    public record LoginCommand(
        string Email,
        string Password) : IRequest<Result<string>>;

}
