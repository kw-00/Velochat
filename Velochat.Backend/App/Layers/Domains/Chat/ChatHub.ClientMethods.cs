using Microsoft.AspNetCore.SignalR;
using Velochat.Backend.App.Layers.DTOs;
using Velochat.Backend.App.Layers.Models;

namespace Velochat.Backend.App.Layers.Domains.Chat;

public partial class ChatHub
{
    private async Task SendRoomClosedAsync(int roomId)
    {
        await Clients.Group(roomId.ToString()).SendAsync("RoomClosed", roomId);
    }

    private async Task SendInvitedAsync(FullInvitationDTO invitationDTO)
    {
        await Clients
            .Group(invitationDTO.RoomId.ToString())
            .SendAsync("Invited", invitationDTO);
    }

    private async Task SendKickedAsync(int roomId)
    {
        await Clients.Group(roomId.ToString()).SendAsync("Kicked", roomId);
    }

    private async Task SendMessageReceivedAsync(CompleteChatMessage message)
    {
        await Clients
            .Group(message.RoomId.ToString())
            .SendAsync("MessageReceived", message);
    }
}