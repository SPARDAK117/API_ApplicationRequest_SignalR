using Application.Commands.ApplicationRequestCommands;
using Application.DTOs.ApplicationRequestDTOs;
using Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{

    [Route("api/[controller]")]
    [ApiController]

    public class ApplicationController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost("create")]
        public async Task<IActionResult> Create(CreateApplicationRequestCommand command)
        {
            var id = await _mediator.Send(command);
            return Ok(new { id });
        }

        [HttpGet("getApplicationDashboard")]
        public async Task<ActionResult<List<ApplicationRequestDto>>> GetAll()
        {
            var result = await _mediator.Send(new GetAllApplicationRequestsQuery());
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("deleteApplicationBatch")]
        public async Task<IActionResult> Delete([FromBody] int[] ids)
        {
            var result = await _mediator.Send(new DeleteApplicationBatchCommand { Ids = ids });

            if (result)
            {
                return NoContent();
            }

            return NotFound("Some or all application requests not found.");
        }

    }
}
