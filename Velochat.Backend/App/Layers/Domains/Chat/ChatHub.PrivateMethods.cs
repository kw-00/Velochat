using System.Security.Claims;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.SignalR;
using Velochat.Backend.App.Layers.DTOs;
using Velochat.Backend.App.Layers.Models;

namespace Velochat.Backend.App.Layers.Domains.Chat;
public partial class ChatHub
{
    private int GetClientUserId()
    {
        var httpContext = Context.GetHttpContext()
            ?? throw new UnauthorizedException("HttpContext is missing.");

        var userIdString 
            = httpContext
                .User
                .Claims
                .First(c => c.Type == ClaimTypes.NameIdentifier).Value
                ?? throw new UnauthorizedException(
                    "User identifier (sub) is missing."
                );

        var userIdIsInteger = int.TryParse(userIdString, out var userId);
        if (!userIdIsInteger) throw new UnauthorizedException(
            "User identifier (sub) is not an integer."
        );
        return userId;
    }

    private async Task EnsureRoomPresenceAsync(int roomId, int userId)
    {
        _ = await roomPresenceRepository.GetAsync(new RoomPresence
        {
            RoomId = roomId,
            MemberId = userId
        })
        ?? throw new ForbiddenException("Client is not in the room.");
    }

    private static string RoomIdMessageFeedGroup(int roomId)
    {
        return "msgfeed:" + roomId.ToString();
    }
    private static int MessageFeedGroupToRoomId(string messageFeedGroupName) 
    {
        return int.Parse(messageFeedGroupName.Split(":")[0]);
    }

    private async Task SubscribeToMessageFeed(int roomId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, RoomIdMessageFeedGroup(roomId));
    }

    private async Task UnsubscribeFromMessageFeed(int roomId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, RoomIdMessageFeedGroup(roomId));
    }
}