

using Velochat.Backend.App.API.Realtime.Session;
using Velochat.Backend.App.Infrastructure.Models;

namespace Velochat.Backend.App.API.Realtime.Channels;

public class RoomFeedChannels : ChannelGroup
{
    public Task BroadcastMessage(
        IRealtimeSession session, CompleteChatMessage message
    ) => Broadcast(session, message.RoomId, ["MessageReceived", message]);

    public Task BroadcastUserJoined(
        IRealtimeSession session, int roomId, CompleteUser user
    ) => Broadcast(session, roomId, ["UserJoined", user]);

    public Task BroadcastUserLeft(
        IRealtimeSession session, int roomId, int userId
    ) => Broadcast(session, roomId, ["UserLeft", userId]);
}