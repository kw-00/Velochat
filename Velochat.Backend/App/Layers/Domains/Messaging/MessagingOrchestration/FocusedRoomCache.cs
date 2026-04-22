using System.Collections.Concurrent;
using Velochat.Backend.App.Shared.Exceptions;

namespace Velochat.Backend.App.Layers.Domains.Messaging;

/// <summary>
/// Cache for which group each connection is assigned to.
/// 
/// In terms of the idea, the group should be associated
/// with the room the client is currently observing. Think
/// of a user selecting a chat room. The ID of that chatroom should
/// correspond to the assigned group for the user's connection.
/// </summary>
public class FocusedRoomCache
{
    private ConcurrentDictionary<string, int> _cache = new();

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
        if (!roomSelected) throw new NotFoundException("Client has not selected a chatroom.");
        return roomId;
    }

    public void ClearFocus(string connectionId)
    {
        _cache.TryRemove(connectionId, out _);
    }
}