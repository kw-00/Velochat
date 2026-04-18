
using Microsoft.AspNetCore.Authorization;
using Velochat.Backend.App.Layers.Infrastructure;
using Velochat.Backend.App.Layers.Models;

namespace Velochat.Backend.App.Layers.Domains.Chat;

public partial class ChatHub
{
    [Authorize]
    public async Task<CompleteChatMessage> SendMessage(int roomId, string content)
    {
        var identityId = GetClientIdentityId();
        await EnsureRoomPresenceAsync(roomId, identityId);

        CompleteChatMessage message;
        try
        {

            message = await chatMessageRepository.CreateAsync(new ChatMessage
            {
                RoomId = roomId,
                AuthorId = identityId,
                Content = content
            });

            await SendMessageReceivedAsync(message);
        }
        catch (IdentifierNotFoundException<Room> ex)
        {
            throw new NotFoundException(ex.Message);
        }
        catch (IdentifierNotFoundException<Models.Identity> ex)
        {
            throw new NotFoundException(ex.Message);
        }

        return message;
    }

    [Authorize]
    public async Task<IReadOnlyList<CompleteChatMessage>> GetMessagesBefore(int roomId, int before)
    {
        var identityId = GetClientIdentityId();
        _ = await roomPresenceRepository.GetAsync(new RoomPresence
        {
            RoomId = roomId,
            MemberId = identityId
        })
        ?? throw new ForbiddenException("Client is not in the room.");

        var messages = await chatMessageRepository.GetByRoomIdBeforeAsync(
            roomId, 
            before, 
            chatOptions.Value.MessageBatchSize
        );
        return messages;
    }

    [Authorize]
    public async Task<IReadOnlyList<CompleteChatMessage>> GetMessagesAfter(int roomId, int after) 
    {
        var identityId = GetClientIdentityId();
        await EnsureRoomPresenceAsync(roomId, identityId);

        var messages = await chatMessageRepository.GetByRoomIdAfterAsync(
            roomId, 
            after, 
            chatOptions.Value.MessageBatchSize
        );
        return messages;
    }

    [Authorize]
    public async Task<IReadOnlyList<CompleteChatMessage>> GetRecentMessages(int roomId) 
    {
        var identityId = GetClientIdentityId();
        await EnsureRoomPresenceAsync(roomId, identityId);

        var messages = await chatMessageRepository.GetByRoomIdAsync(
            roomId, 
            chatOptions.Value.MessageBatchSize
        );
        return messages;
    }
}

   