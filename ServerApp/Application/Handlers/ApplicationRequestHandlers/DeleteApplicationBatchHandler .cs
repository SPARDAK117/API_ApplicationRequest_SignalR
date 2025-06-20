using Application.Commands.ApplicationRequestCommands;
using Application.DTOs.ApplicationRequestDTOs;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Handlers.ApplicationRequestHandlers
{
    public class DeleteApplicationBatchHandler(AppDbContext dbContext) : IRequestHandler<DeleteApplicationBatchCommand, bool>
    {
        private readonly AppDbContext _dbContext = dbContext;

        public async Task<bool> Handle(DeleteApplicationBatchCommand request, CancellationToken cancellationToken)
        {
            List<ApplicationRequest> entities = await _dbContext.ApplicationRequests
                .Where(r => request.Ids.Contains(r.Id))
                .ToListAsync(cancellationToken);

            if (entities.Count == 0)
                return false;

            _dbContext.ApplicationRequests.RemoveRange(entities);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
