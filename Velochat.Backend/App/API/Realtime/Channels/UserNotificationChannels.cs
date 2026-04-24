using Velochat.Backend.App.API.Realtime.Session;
using Velochat.Backend.App.Infrastructure.Models;

namespace Velochat.Backend.App.API.Realtime.Channels;


public class UserNotificationChannels : ChannelGroup
{
    public Task SendFriendshipRequested(
        IRealtimeSession session, int userId, CompleteUser requester
    ) => Broadcast(session, userId, ["FriendshipRequested", requester]);

    public Task SendFriendshipAccepted(
        IRealtimeSession session, int userId, CompleteUser acceptor
    ) => Broadcast(session, userId, ["FriendshipAccepted", acceptor]);

    public Task SendAddedToRoom(
        IRealtimeSession session, int roomId, CompleteRoom room
    ) => Broadcast(session, roomId, ["AddedToRoom", room]);
}