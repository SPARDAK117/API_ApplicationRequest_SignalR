using Application.Commands;
using Application.DTOs;
using Application.Queries;
using Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationRequestsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ApplicationRequestsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
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
