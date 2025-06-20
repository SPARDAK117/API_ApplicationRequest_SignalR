using Application.Commands.AuthCommands;
using Application.DTOs.AuthDTOs;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace Application.Handlers.AuthHandler
{
    public class LoginHandler : IRequestHandler<LoginCommand, AuthResultDto>
    {
        private readonly ILoginCredentialRepository _repository;
        private readonly IJwtService _jwtService;

        public LoginHandler(ILoginCredentialRepository repository, IJwtService jwtService)
        {
            _repository = repository;
            _jwtService = jwtService;
        }

        public async Task<AuthResultDto> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            LoginCredential roleName = new();
            LoginCredential? user = await _repository.GetByUsernameOrEmailAsync(request.Input);

            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                throw new UnauthorizedAccessException("Invalid credentials");

            roleName.Username = user.Role?.Name ?? throw new InvalidOperationException("User role not assigned");

            string token = _jwtService.GenerateToken(user.Id, user.Username, roleName.Username);

            return new AuthResultDto
            {
                Token = token,
                Username = user.Username,
                Role = roleName.Role?.Name ?? "Unknown"
            };
        }
    }
}
