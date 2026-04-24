

namespace Velochat.Backend.App.API.Realtime.Session;

public interface IRealtimeSession
{
    string ConnectionId { get; }
    int UserId { get; }

    Task SubscribeAsync(string channel);

    Task UnsubscribeAsync(string channel);

    Task BroadcastAsync(
        string channel, 
        object?[] data
    );

    Task BroadcastAsync(
        string channel, 
        object?[] data, 
        CancellationToken cancellationToken
    );
}
