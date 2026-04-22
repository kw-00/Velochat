using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Velochat.Backend.App.Layers.Domains.Messaging;
using Velochat.Backend.App.Shared.RealtimeCommunication;

namespace Velochat.Backend.App.Layers.Hubs;

public sealed class GlobalHub(
    MessagingCommandDispatcher messagingCommandDispatcher
) : Hub
{
    public static event Action<SignalRRealtimeSession>? OnDisconnectedAsyncEvent;
    private IRealtimeSession Session => new SignalRRealtimeSession(this);

    [Authorize]
    public async Task<CommandResult> Messaging(string command, params object[] args)
    {
        return await messagingCommandDispatcher.ExecuteAsync(Session, command, args);
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await base.OnDisconnectedAsync(exception);
        OnDisconnectedAsyncEvent?.Invoke(new SignalRRealtimeSession(this));
    }
}