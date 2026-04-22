using Velochat.Backend.App.Shared.RealtimeCommunication;

namespace Velochat.Backend.App.Layers.Hubs;

public static class ChannelCategories
{
    public static readonly ChannelCategory MessageFeed = new("general");

    public static readonly ChannelCategory FriendshipNotifications 
        = new("friendship-notifications");
}