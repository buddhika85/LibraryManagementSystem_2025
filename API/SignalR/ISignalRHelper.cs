
namespace API.SignalR
{
    public interface ISignalRHelper
    {
        Task BroadcastMessageToAllConnectedClients(string message, object? arg1, object? arg2);
        Task BroadcastMessageToSpecificClient(string email, string message, object? arg1, object? arg2);
    }
}