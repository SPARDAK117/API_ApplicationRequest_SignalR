using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories
{
    public class LoginCredentialRepository : ILoginCredentialRepository
    {
        private readonly AppDbContext _context;

        public LoginCredentialRepository(AppDbContext context)
        {
            _context = context;
        }

        public Task<LoginCredential?> GetByUsernameOrEmailAsync(string input)
        {
            return _context.LoginCredentials
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Username == input || u.Email == input);
        }
    }
}
