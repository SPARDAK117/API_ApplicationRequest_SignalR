using Microsoft.AspNetCore.SignalR;

namespace Infrastructure.Hubs
{

    public class ApplicationRequestsHub : Hub
    {
        public async Task SendMessage(string message)
        {
            await Clients.All.SendAsync("StatusUpdated", message);
        }
    }
}
