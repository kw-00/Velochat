using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;
using Velochat.Backend.App.Layers.Models;

namespace Velochat.Backend.App.Layers.Domains.Chat;
public partial class ChatHub
{
    private int GetClientIdentityId()
    {
        var httpContext = Context.GetHttpContext()
            ?? throw new UnauthorizedException("HttpContext is missing.");

        var identityIdString = httpContext.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value
            ?? throw new UnauthorizedException("User identifier (sub) is missing.");

        var identityIdIsInteger = int.TryParse(identityIdString, out var identityId);
        if (!identityIdIsInteger) throw new UnauthorizedException("User identifier (sub) is not an integer.");
        return identityId;
    }

    private async Task EnsureRoomPresence(int roomId, int identityId)
    {
        _ = await roomPresenceRepository.GetAsync(new RoomPresence
        {
            RoomId = roomId,
            IdentityId = identityId
        })
        ?? throw new ForbiddenException("Client is not in the room.");
    }
}