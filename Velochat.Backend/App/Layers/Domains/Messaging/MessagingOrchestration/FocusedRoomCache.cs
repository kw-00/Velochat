using System.Collections.Concurrent;
using Velochat.Backend.App.Shared.Exceptions;
using Velochat.Backend.App.Layers.Hubs;
using Velochat.Backend.App.Shared.RealtimeCommunication;

namespace Velochat.Backend.App.Layers.Domains.Messaging;

/// <summary>
/// Cache for which group each connection is assigned to.
/// 
/// In terms of the idea, the group should be associated
/// with the room the client is currently observing. Think
/// of a user selecting a chat room. The ID of that chatroom should
/// correspond to the assigned group for the user's connection.
/// </summary>
public class FocusedRoomCache : IDisposable
{
    private ConcurrentDictionary<string, int> _cache = new();

    private Action<IRealtimeSession> _disconnectedHandler;

    public FocusedRoomCache()
    {
        _disconnectedHandler = (session) =>
        {
            ClearFocus(session.ConnectionId);
        };

        GlobalHub.OnDisconnectedAsyncEvent += _disconnectedHandler;
    }

    public void SetFocus(string connectionId, int roomId)
    {
        _cache[connectionId] = roomId;
    }

    public int? TryGetFocusedRoom(string connectionId)
    {
        return _cache.TryGetValue(connectionId, out var roomId) ? roomId : null;
    }

    public int GetFocusedRoom(string connectionId)
    {
        var roomSelected = _cache.TryGetValue(connectionId, out var roomId);
        if (!roomSelected) 
            throw new NotFoundException("Client has not selected a chatroom.");
        return roomId;
    }

    public void ClearFocus(string connectionId)
    {
        _cache.TryRemove(connectionId, out _);
    }

    public void Dispose()
    {
        GlobalHub.OnDisconnectedAsyncEvent -= _disconnectedHandler;
    }
}