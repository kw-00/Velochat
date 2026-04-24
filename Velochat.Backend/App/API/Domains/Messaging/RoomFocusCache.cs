using System.Collections.Concurrent;
using Velochat.Backend.App.API.Realtime.Session;
using Velochat.Backend.App.Shared.Exceptions;

namespace Velochat.Backend.App.API.Realtime.RPCManagement;

/// <summary>
/// Cache for which group each connection is assigned to.
/// 
/// In terms of the idea, the group should be associated
/// with the room the client is currently observing. Think
/// of a user selecting a chat room. The ID of that chatroom should
/// correspond to the assigned group for the user's connection.
/// </summary>
public class RoomFocusCache : IDisposable
{
    private ConcurrentDictionary<string, int> _cache = new();

    private Action<IRealtimeSession> _disconnectedHandler;

    public RoomFocusCache()
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
        GC.SuppressFinalize(this);
        GlobalHub.OnDisconnectedAsyncEvent -= _disconnectedHandler;
    }
}