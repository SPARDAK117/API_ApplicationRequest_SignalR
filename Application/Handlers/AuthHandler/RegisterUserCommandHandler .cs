using Application.Commands.AuthCommands;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Handlers.AuthHandler
{
    public class RegisterUserCommandHandler(AppDbContext context) : IRequestHandler<RegisterUserCommand, int>
    {
        private readonly AppDbContext _context = context;

        public async Task<int> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            if (await _context.LoginCredentials.AnyAsync(u => u.Email == request.Email || u.Username == request.Username, cancellationToken))
            {
                throw new ArgumentException("Email o username ya están en uso.");
            }

            var role = await _context.Roles
                .FirstOrDefaultAsync(r => r.Id == request.RoleId, cancellationToken);

            if (role == null)
            {
                throw new ArgumentException("El rol especificado no existe.");
            }

            var newUser = new LoginCredential
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
