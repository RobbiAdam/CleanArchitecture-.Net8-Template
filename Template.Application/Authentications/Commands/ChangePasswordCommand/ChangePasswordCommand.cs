using MediatR;
using Template.Contract.Common.Bases;

namespace Template.Application.Authentications.Commands.ChangePasswordCommand
{
    public record ChangePasswordCommand(
        string OldPassword,
        string NewPassword) : IRequest<BaseResponse<bool>>;
    
}
