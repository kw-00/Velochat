using Microsoft.AspNetCore.SignalR;
using Velochat.Backend.App.Shared.Auth;
using Velochat.Backend.App.Shared.Exceptions;

namespace Velochat.Backend.App.API.Realtime.RPCManagement;

public class SignalRRealtimeSession(
    Hub hub
) : IRealtimeSession
{
    public string ConnectionId => hub.Context.ConnectionId;

    public int UserId
    {
        get;
        init
        {
            var claimsPrincipal = hub.Context.User
                ?? throw new UnauthorizedException("User is not authenticated.");
            field = IdentityInfoExtraction.GetUserId(claimsPrincipal);
        }
    }


    public async Task SubscribeAsync(string channel) 
    {
        await hub.Groups.AddToGroupAsync(ConnectionId, channel);
    }

    public async Task UnsubscribeAsync(string channel)
    {
        await hub.Groups.RemoveFromGroupAsync(ConnectionId, channel);
    }

    public async Task BroadcastAsync(string channel, object?[] data)
    {
        await hub.Clients.All.SendAsync(channel, data);
    }

    public async Task BroadcastAsync(
        string channel, object?[] data, CancellationToken cancellationToken
    )
    {
        await hub.Clients.All.SendAsync(channel, data, cancellationToken);
    }
}