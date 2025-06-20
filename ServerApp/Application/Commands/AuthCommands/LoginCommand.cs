using Application.DTOs.AuthDTOs;
using MediatR;

namespace Application.Commands.AuthCommands
{
    public record LoginCommand : IRequest<AuthResultDto>
    {
        public string Input { get; init; } = string.Empty;
        public string Password { get; init; } = string.Empty;
    }
}
