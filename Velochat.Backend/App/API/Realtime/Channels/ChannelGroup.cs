using Velochat.Backend.App.API.Realtime.Session;

namespace Velochat.Backend.App.API.Realtime.Channels;

public abstract class ChannelGroup
{
    protected string Name => GetType().Name;

    public async Task Subscribe(IRealtimeSession session, int channelId)
    {
        await session.SubscribeAsync(GetChannel(channelId));
    }

    public async Task Unsubscribe(IRealtimeSession session, int channelId)
    {
        await session.UnsubscribeAsync(GetChannel(channelId));
    }

    public async Task Broadcast(
        IRealtimeSession session, int channelId, object?[] data
    )
    {
        await session.BroadcastAsync(GetChannel(channelId), data);
    }

    public string GetChannel(object localChannelId) => $"{Name}:{localChannelId}";
}