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

        /// <summary>
        /// Creates a new application request.
        /// </summary>
        /// <param name="commandDto">Data for the new application request</param>
        /// <returns>ID of the created request</returns>
        /// <response code="200">Request created successfully</response>
        /// <response code="400">Invalid data or creation error</response>
        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create(CreateApplicationRequestDto commandDto)
        {
            CreateApplicationRequestCommand command = new(commandDto.TypeId, commandDto.Message);
            int id = await _mediator.Send(command);
            return Ok(new { id });
        }

        /// <summary>
        /// Retrieves all application requests for the dashboard.
        /// </summary>
        /// <returns>List of application request DTOs</returns>
        /// <response code="200">List retrieved successfully</response>
        [HttpGet("getApplicationDashboard")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<ApplicationRequestDto>>> GetAll()
        {
            List<ApplicationRequestDto> result = await _mediator.Send(new GetAllApplicationRequestsQuery());
            return Ok(result);
        }

        /// <summary>
        /// Deletes a batch of application requests (admin only).
        /// </summary>
        /// <param name="ids">IDs of the application requests to delete</param>
        /// <returns>No content if successful, or Not Found if none matched</returns>
        /// <response code="204">Requests deleted successfully</response>
        /// <response code="404">One or more requests were not found</response>
        [Authorize(Roles = "Admin")]
        [HttpDelete("deleteApplicationBatch")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete([FromBody] DeleteApplicationBatchDto commandDto)
        {
            bool result = await _mediator.Send(new DeleteApplicationBatchCommand { Ids = commandDto.Ids });

            if (result)
            {
                return NoContent();
            }

            return NotFound("Some or all application requests not found.");
        }
    }
}
