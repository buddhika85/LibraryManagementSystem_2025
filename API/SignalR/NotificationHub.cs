using API.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace API.SignalR
{
    [Authorize]
    public class NotificationHub : Hub
    {
        // to store a list of client connections which this API has been consumed by
        private static readonly ConcurrentDictionary<string, string> userConnections = new();          // key: email, value: users connection Id

        public override Task OnConnectedAsync()
        {
            // add connected user
            var email = Context.User?.GetEmail();
            if (!string.IsNullOrEmpty(email))
                userConnections[email] = Context.ConnectionId;

            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            // remove user
            var email = Context.User?.GetEmail();
            if (!string.IsNullOrEmpty(email))
                userConnections.TryRemove(email, out _);

            return base.OnDisconnectedAsync(exception);
        }

        public static string? GetConnectionIdByEmail(string email)
        {
            userConnections.TryGetValue(email, out var connectionId);
            return connectionId;
        }
    }
}
