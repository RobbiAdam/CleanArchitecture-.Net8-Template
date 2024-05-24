using MediatR;
using Template.Domain.Common;

namespace Template.Application.UseCases.Authentications.Commands.ChangePasswordCommand
{
    public record ChangePasswordCommand(
        string OldPassword,
        string NewPassword) : IRequest<Result<bool>>;

}
