using Application.Commands.AuthCommands;
using Application.DTOs.AuthDTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        /// <summary>
        /// Authenticates a user and generates a JWT token.
        /// </summary>
        /// <param name="request">User credentials (username/email and password)</param>
        /// <returns>JWT token and authenticated user information</returns>
        /// <response code="200">Returns the token if credentials are valid</response>
        /// <response code="401">Invalid credentials</response>
        [HttpPost("login")]
        [ProducesResponseType(typeof(AuthResultDto), 200)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            try
            {
                var result = await _mediator.Send(new LoginCommand
                {
                    Input = request.Username,
                    Password = request.Password
                });

                return Ok(result);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized("Invalid credentials");
            }
        }

        /// <summary>
        /// Registers a new user and assigns a role.
        /// </summary>
        /// <param name="command">Registration data including username, email, password and role ID</param>
        /// <returns>ID of the newly registered user</returns>
        /// <response code="200">User created successfully</response>
        /// <response code="400">Invalid input or registration error</response>
        [HttpPost("register")]
        [ProducesResponseType(typeof(object), 200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserCommand command)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(command.Username) ||
                    string.IsNullOrWhiteSpace(command.Email) ||
                    string.IsNullOrWhiteSpace(command.Password) ||
                    command.RoleId <= 0)
                {
                    return BadRequest("Invalid input data");
                }

                var result = await _mediator.Send(command);
                return Ok(new { UserId = result });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
