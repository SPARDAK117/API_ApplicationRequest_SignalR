using MediatR;

namespace Application.Commands.AuthCommands
{
    public class RegisterUserCommand : IRequest<int>
    {
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public int RoleId { get; set; } = 2;
    }
}
