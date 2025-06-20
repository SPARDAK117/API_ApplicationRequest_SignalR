using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IApplicationRequestRepository
    {
        Task<IEnumerable<ApplicationRequest>> GetAllAsync();
        Task<ApplicationRequest?> GetByIdAsync(int id);
        Task SaveChangesAsync();
        Task AddAsync(ApplicationRequest entity);
        Task<List<ApplicationRequest>> GetAllWithTypeAsync();
    }
}
