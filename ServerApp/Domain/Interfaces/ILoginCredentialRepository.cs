using Domain.Entities;

namespace Domain.Interfaces
{
    public interface ILoginCredentialRepository
    {
        Task<LoginCredential?> GetByUsernameOrEmailAsync(string input);
    }
}
