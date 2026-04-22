using System.Collections.Concurrent;
using Velochat.Backend.App.API.Realtime.RPCManagement;

namespace Velochat.Backend.App.API.Realtime;

public class UserConnectionCache : IDisposable
{
    private Action<IRealtimeSession> _connectedHandler;
    private Action<IRealtimeSession> _disconnectedHandler;
    private ConcurrentDictionary<int, ConcurrentDictionary<string, byte>> _cache = new();

    public UserConnectionCache()
    {
        GlobalHub.OnConnectedAsyncEvent += _connectedHandler = (session) => 
        {
            AddConnection(session.UserId, session.ConnectionId);
        };

        GlobalHub.OnDisconnectedAsyncEvent += _disconnectedHandler = (session) => 
        {
            RemoveConnection(session.UserId, session.ConnectionId);
        };
    }


    public ISet<string> GetConnections(int userId)
    {
        _cache.TryGetValue(userId, out var connections);
        return connections?.Keys.ToHashSet() ?? [];
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        GlobalHub.OnConnectedAsyncEvent -= _connectedHandler;
        GlobalHub.OnDisconnectedAsyncEvent -= _disconnectedHandler;
    }

    private void AddConnection(int userId, string connectionId)
    {
        _cache.AddOrUpdate(
            userId,
            (_) => new ConcurrentDictionary<string, byte> { [connectionId] = default },
            (_, connections) => 
            { 
                connections[connectionId] = default; return connections; 
            }
        );    
    }

    private void RemoveConnection(int userId, string connectionId)
    {
        _cache.AddOrUpdate(
            userId,
            (_) => new ConcurrentDictionary<string, byte>(),
            (_, connections) => 
            { 
                connections.TryRemove(connectionId, out byte _); return connections; 
            }
        );

        _cache.TryRemove(
            new KeyValuePair<int, ConcurrentDictionary<string, byte>>(
                userId, new ConcurrentDictionary<string, byte>()
            )
        );
    }
}