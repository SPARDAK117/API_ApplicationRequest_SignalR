using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

public class ApplicationRequestRepository : IApplicationRequestRepository
{
    private readonly AppDbContext _context;

    public ApplicationRequestRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(ApplicationRequest entity)
    {
        await _context.ApplicationRequests.AddAsync(entity);
    }

    public async Task<IEnumerable<ApplicationRequest>> GetAllAsync()
    {
        return await _context.ApplicationRequests.ToListAsync();
    }

    public async Task<ApplicationRequest?> GetByIdAsync(int id)
    {
        return await _context.ApplicationRequests.FindAsync(id);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
    public async Task<List<ApplicationRequest>> GetAllWithTypeAsync()
    {
        return await _context.ApplicationRequests
            .Include(ar => ar.Type)
            .ToListAsync();
    }
}
