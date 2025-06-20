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
        /// Autentica a un usuario y genera un token JWT.
        /// </summary>
        /// <param name="request">Credenciales del usuario (username/email y contraseña)</param>
        /// <returns>Token JWT y datos del usuario autenticado</returns>
        /// <response code="200">Retorna el token si las credenciales son válidas</response>
        /// <response code="401">Credenciales inválidas</response>
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
        /// Registra un nuevo usuario con rol asociado.
        /// </summary>
        /// <param name="command">Datos del nuevo usuario</param>
        /// <returns>ID del usuario registrado</returns>
        /// <response code="200">Usuario creado correctamente</response>
        /// <response code="400">Datos inválidos o error en el registro</response>
        [HttpPost("register")]
        [ProducesResponseType(typeof(object), 200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserCommand command)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(command.Username) || string.IsNullOrWhiteSpace(command.Email) || string.IsNullOrWhiteSpace(command.Password) || command.RoleId <= 0)
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
