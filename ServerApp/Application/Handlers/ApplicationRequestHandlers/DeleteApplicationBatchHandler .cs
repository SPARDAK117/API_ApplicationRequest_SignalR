using Application.Commands.ApplicationRequestCommands;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Handlers.ApplicationRequestHandlers
{
    public class DeleteApplicationBatchHandler : IRequestHandler<DeleteApplicationBatchCommand, bool>
    {
        private readonly AppDbContext _dbContext;

        public DeleteApplicationBatchHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> Handle(DeleteApplicationBatchCommand request, CancellationToken cancellationToken)
        {
            var entities = await _dbContext.ApplicationRequests
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
