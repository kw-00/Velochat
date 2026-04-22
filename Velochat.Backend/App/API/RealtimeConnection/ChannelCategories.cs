using Velochat.Backend.App.API.Realtime.RPCManagement;

namespace Velochat.Backend.App.API.Realtime;

public static class ChannelCategories
{
    public static readonly ChannelCategory MessageFeed = new("general");

    public static readonly ChannelCategory FriendshipNotifications 
        = new("friendship-notifications");
}