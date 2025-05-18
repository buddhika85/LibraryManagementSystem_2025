using Microsoft.AspNetCore.SignalR;

namespace API.SignalR
{
    // A wrapper for HubContext
    public class SignalRHelper : ISignalRHelper
    {
        private readonly IHubContext<NotificationHub> hubContext;

        public SignalRHelper(IHubContext<NotificationHub> hubContext)
        {
            this.hubContext = hubContext;
        }

        public async Task BroadcastMessageToAllConnectedClientsAsync(string message, object? arg1, object? arg2 = null)
        {
            await hubContext.Clients.All.SendAsync(message, arg1, arg2);
        }

        public async Task BroadcastMessageToSpecificClientAsync(string email, string message, object? arg1, object? arg2 = null)
        {
            var connectionId = NotificationHub.GetConnectionIdByEmail(email);
            if (connectionId != null)
            {
                await hubContext.Clients.Client(connectionId).SendAsync(message, arg1, arg2);
            }
        }
    }
}
