using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Velochat.Backend.App.Layers.Domains.Messaging;
using Velochat.Backend.App.Shared.RealtimeCommunication;

namespace Velochat.Backend.App.Layers.GlobalHub;

public class GlobalHub(
    MessagingCommandDispatcher messagingCommandDispatcher
) : Hub
{
    private IRealtimeSession Session => new SignalRRealtimeSession(this);

    [Authorize]
    public async Task<CommandResult> Messaging(string command, params object[] args)
    {
        return await messagingCommandDispatcher.ExecuteAsync(Session, command, args);
    }
}