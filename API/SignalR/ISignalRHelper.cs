
namespace API.SignalR
{
    public interface ISignalRHelper
    {
        Task BroadcastToAllConnectedClientsAsync(string message, object? arg1, object? arg2 = null);
        Task BroadcastToSpecificClientAsync(string email, string message, object? arg1, object? arg2 = null);
    }
}