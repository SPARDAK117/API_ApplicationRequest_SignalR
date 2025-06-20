using Application.Commands.AuthCommands;
using Application.DTOs.AuthDTOs;
using Domain.Interfaces;
using MediatR;

namespace Application.Handlers.AuthHandler
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthResultDto>
    {
        private readonly ILoginCredentialRepository _repository;
        private readonly IJwtService _jwtService;

        public LoginCommandHandler(ILoginCredentialRepository repository, IJwtService jwtService)
        {
            _repository = repository;
            _jwtService = jwtService;
        }

        public async Task<AuthResultDto> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _repository.GetByUsernameOrEmailAsync(request.Input);

            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                throw new UnauthorizedAccessException("Invalid credentials");

            var roleName = user.Role?.Name ?? throw new InvalidOperationException("User role not assigned");

            var token = _jwtService.GenerateToken(user.Id, user.Username, roleName);

            return new AuthResultDto
            {
                Token = token,
                Username = user.Username,
                Role = roleName
            };
        }
    }
}
