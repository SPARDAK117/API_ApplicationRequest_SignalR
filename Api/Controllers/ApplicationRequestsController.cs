using Application.Commands.ApplicationRequestCommands;
using Application.DTOs.ApplicationRequestDTOs;
using Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]

    public class ApplicationRequestsController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost("create")]
        public async Task<IActionResult> Create(CreateApplicationRequestCommand command)
        {
            var id = await _mediator.Send(command);
            return Ok(new { id });
        }

        [HttpGet]
        public async Task<ActionResult<List<ApplicationRequestDto>>> GetAll()
        {
            var result = await _mediator.Send(new GetAllApplicationRequestsQuery());
            return Ok(result);
        }
    }
}
