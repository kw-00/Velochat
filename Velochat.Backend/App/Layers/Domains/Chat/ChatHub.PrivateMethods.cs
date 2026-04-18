using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;
using Velochat.Backend.App.Layers.DTOs;
using Velochat.Backend.App.Layers.Models;

namespace Velochat.Backend.App.Layers.Domains.Chat;
public partial class ChatHub
{
    private int GetClientIdentityId()
    {
        var httpContext = Context.GetHttpContext()
            ?? throw new UnauthorizedException("HttpContext is missing.");

        var identityIdString 
            = httpContext
                .User
                .Claims
                .First(c => c.Type == ClaimTypes.NameIdentifier).Value
                ?? throw new UnauthorizedException(
                    "User identifier (sub) is missing."
                );

        var identityIdIsInteger = int.TryParse(identityIdString, out var identityId);
        if (!identityIdIsInteger) throw new UnauthorizedException(
            "User identifier (sub) is not an integer."
        );
        return identityId;
    }

    private async Task EnsureRoomPresenceAsync(int roomId, int identityId)
    {
        _ = await roomPresenceRepository.GetAsync(new RoomPresence
        {
            RoomId = roomId,
            MemberId = identityId
        })
        ?? throw new ForbiddenException("Client is not in the room.");
    }


    private async Task AddToGroupAsync(int roomId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, roomId.ToString());
    }

    private async Task RemoveFromGroupAsync(int roomId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId.ToString());
    }
}