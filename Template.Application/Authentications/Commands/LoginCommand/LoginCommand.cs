using MediatR;
using Template.Contract.Common.Bases;

namespace Template.Application.Authentications.Commands.LoginCommand
{
    public record LoginCommand(
        string Email,
        string Password) : IRequest<BaseResponse<string>>;  

}
