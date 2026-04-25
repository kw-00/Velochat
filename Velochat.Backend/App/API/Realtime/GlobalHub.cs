using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Velochat.Backend.App.API.Domains.Friendship;
using Velochat.Backend.App.API.Domains.Messaging;
using Velochat.Backend.App.API.Domains.Rooms;
using Velochat.Backend.App.API.Realtime.Channels;
using Velochat.Backend.App.API.Realtime.RPCManagement;
using Velochat.Backend.App.API.Realtime.Session;

namespace Velochat.Backend.App.API.Realtime;

public sealed class GlobalHub(
    MessagingCommandDispatcher messagingCommandDispatcher,
    FriendshipCommandDispatcher friendshipCommandDispatcher,
    RoomsCommandDispatcher roomsCommandDispatcher,
    UserNotificationChannels userNotificationChannels
) : Hub
{
    public static event Action<SignalRRealtimeSession>? OnConnectedAsyncEvent;
    public static event Action<SignalRRealtimeSession>? OnDisconnectedAsyncEvent;

    private IRealtimeSession Session => new SignalRRealtimeSession(this);

    [Authorize]
    public async Task<CommandResult> Messaging(string command, params object[] args)
        => await messagingCommandDispatcher.ExecuteAsync(Session, command, args);


    [Authorize]
    public async Task<CommandResult> Friendship(string command, params object[] args)
        => await friendshipCommandDispatcher.ExecuteAsync(Session, command, args);
    

    [Authorize]
    public async Task<CommandResult> Rooms(string command, params object[] args)
        => await roomsCommandDispatcher.ExecuteAsync(Session, command, args);

    public override async Task OnConnectedAsync()
    {
        await base.OnConnectedAsync();
        var userId = Session.UserId;
        await userNotificationChannels.Subscribe(Session, userId);
        OnConnectedAsyncEvent?.Invoke(new SignalRRealtimeSession(this));
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await base.OnDisconnectedAsync(exception);
        OnDisconnectedAsyncEvent?.Invoke(new SignalRRealtimeSession(this));
    }
}