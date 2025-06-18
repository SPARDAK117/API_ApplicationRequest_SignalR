using Infrastructure.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Persistence;

namespace Infrastructure.Services
{
    public class ApplicationStatusUpdater : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IHubContext<ApplicationRequestsHub> _hubContext;

        public ApplicationStatusUpdater(IServiceProvider serviceProvider, IHubContext<ApplicationRequestsHub> hubContext)
        {
            _serviceProvider = serviceProvider;
            _hubContext = hubContext;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using IServiceScope scope = _serviceProvider.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                DateTime now = DateTime.UtcNow;
                DateTime expirationThreshold = now.AddMinutes(-1);

                int updatedCount = await dbContext.ApplicationRequests
                    .Where(x => x.Status == "submitted" && x.Date <= expirationThreshold)
                    .ExecuteUpdateAsync(setters => setters
                        .SetProperty(r => r.Status, r => "completed"),
                        cancellationToken: stoppingToken);

                if (updatedCount > 0)
                {
                    await _hubContext.Clients.All.SendAsync("ApplicationRequestsUpdated", cancellationToken: stoppingToken);
                }

                await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
            }
        }

    }

}
