using Application.DTOs.ApplicationRequestDTOs;
using Domain.Entities;
using Infrastructure.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Persistence;

namespace Infrastructure.Services
{
    public class ApplicationStatusUpdater(IServiceProvider serviceProvider, IHubContext<ApplicationRequestsHub> hubContext) : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider = serviceProvider;
        private readonly IHubContext<ApplicationRequestsHub> _hubContext = hubContext;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using IServiceScope scope = _serviceProvider.CreateScope();
                AppDbContext dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                DateTime now = DateTime.UtcNow;
                DateTime expirationThreshold = now.AddMinutes(-1);

                int updatedCount = await dbContext.ApplicationRequests
                    .Where(x => x.Status == "submitted" && x.Date <= expirationThreshold)
                    .ExecuteUpdateAsync(setters => setters
                        .SetProperty(r => r.Status, r => "completed"),
                        cancellationToken: stoppingToken);

                if (updatedCount > 0)
                {
                    List<ApplicationRequest> updatedRequests = await dbContext.ApplicationRequests
                        .Where(x => x.Status == "completed" && x.Date <= now)
                        .Include(x => x.Type)
                        .ToListAsync(cancellationToken: stoppingToken);

                    List<ApplicationRequestDto> response = updatedRequests.Select(x => new ApplicationRequestDto
                    {
                        Id = x.Id,
                        Date = x.Date,
                        Type = x.Type?.Name ?? string.Empty,
                        Status = x.Status
                    }).ToList();

                    await _hubContext.Clients.All.SendAsync("ApplicationRequestsUpdated", response, stoppingToken);

                }

                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
            }
        }

    }

}
