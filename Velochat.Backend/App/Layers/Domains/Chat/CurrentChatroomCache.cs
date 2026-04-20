using System.Collections.Concurrent;

namespace Velochat.Backend.App.Layers.Domains.Chat;

/// <summary>
/// Cache for which group each connection is assigned to.
/// 
/// In terms of the idea, the group should be associated
/// with the room the client is currently observing. Think
/// of a user selecting a chat room. The ID of that chatroom should
/// correspond to the assigned group for the user's connection.
/// </summary>
public class CurrentChatroomCache
{
    public ConcurrentDictionary<string, int> Cache { get; } = new();

    public void SetCurrentChatroom(string connectionId, int roomId)
    {
        Cache[connectionId] = roomId;
    }

    public int GetCurrentChatroom(string connectionId)
    {
        var roomSelected = Cache.TryGetValue(connectionId, out var roomId);
        if (!roomSelected) throw new NotFoundException("Client has not selected a chatroom.");
        return roomId;
    }

}