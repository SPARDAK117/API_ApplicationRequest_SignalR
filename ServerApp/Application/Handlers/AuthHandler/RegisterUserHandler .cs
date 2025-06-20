using Application.Commands.AuthCommands;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Handlers.AuthHandler
{
    public class RegisterUserHandler(AppDbContext context) : IRequestHandler<RegisterUserCommand, int>
    {
        private readonly AppDbContext _context = context;

        public async Task<int> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            if (await _context.LoginCredentials.AnyAsync(u => u.Email == request.Email || u.Username == request.Username, cancellationToken))
            {
                throw new ArgumentException("Email o username ya están en uso.");
            }

            Role role = (await _context.Roles
                .FirstOrDefaultAsync(r => r.Id == request.RoleId, cancellationToken) ?? new()) ?? throw new ArgumentException("The rol doesn´t exists.");

            LoginCredential newUser = new()
            {
                Username = request.Username,
                Email = request.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                RoleId = role.Id
            };

            _context.LoginCredentials.Add(newUser);
            await _context.SaveChangesAsync(cancellationToken);

            return newUser.Id;
        }
    }

}
