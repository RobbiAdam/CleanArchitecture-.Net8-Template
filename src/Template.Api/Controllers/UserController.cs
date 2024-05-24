using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Template.Application.UseCases.Users.Queries.GetCurrentUserQuery;

namespace Template.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;
        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("me")]
        [Authorize]
        public async Task<IActionResult> GetCurrentUserAsync(CancellationToken ct)
        {
            var response = await _mediator.Send(new GetCurrentUserQuery(), ct);
            if (!response.IsSuccess)
            {
                return BadRequest(response);
            }
            return Ok(response);    
        }
    }
}
