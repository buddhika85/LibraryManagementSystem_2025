
namespace API.SignalR
{
    public interface ISignalRHelper
    {
        Task BroadcastMessageToAllConnectedClientsAsync(string message, object? arg1, object? arg2 = null);
        Task BroadcastMessageToSpecificClientAsync(string email, string message, object? arg1, object? arg2 = null);
    }
}