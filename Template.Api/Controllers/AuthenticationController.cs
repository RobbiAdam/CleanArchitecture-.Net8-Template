using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Template.Application.UseCases.Authentications.Commands.ChangePasswordCommand;
using Template.Application.UseCases.Authentications.Commands.LoginCommand;
using Template.Application.UseCases.Authentications.Commands.RegisterCommand;

namespace Template.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthenticationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> RegisterAsync(
            [FromBody] RegisterCommand command, CancellationToken ct)
        {
            var response = await _mediator.Send(command, ct);
            if (!response.IsSuccess)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> LoginAsync(
            [FromBody] LoginCommand command, CancellationToken ct)
        {
            var response = await _mediator.Send(command, ct);
            if (!response.IsSuccess)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPut]
        [Route("change-password")]
        [Authorize]
        public async Task<IActionResult> ChangePasswordAsync(
            [FromBody] ChangePasswordCommand command, CancellationToken ct)
        {
            var response = await _mediator.Send(command, ct);
            if (!response.IsSuccess)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}
