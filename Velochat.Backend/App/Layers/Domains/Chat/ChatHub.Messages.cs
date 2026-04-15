
using Velochat.Backend.App.Layers.Infrastructure;
using Velochat.Backend.App.Layers.Models;

namespace Velochat.Backend.App.Layers.Domains.Chat;

public partial class ChatHub
{
    public async Task<CompleteChatMessage> SendMessage(int roomId, string content)
    {
        var identityId = GetClientIdentityId();
        await EnsureRoomPresence(roomId, identityId);

        CompleteChatMessage message;
        try
        {

            message = await chatMessageRepository.CreateAsync(new ChatMessage
            {
                RoomId = roomId,
                AuthorId = identityId,
                Content = content
            });
        }
        catch (RecordNotFoundException<Room> ex)
        {
            throw new NotFoundException(ex.Message);
        }
        catch (RecordNotFoundException<Models.Identity> ex)
        {
            throw new NotFoundException(ex.Message);
        }

        return message;
    }

    public async Task<IReadOnlyList<CompleteChatMessage>> GetMessagesBefore(int roomId, int before)
    {
        var identityId = GetClientIdentityId();
        _ = await roomPresenceRepository.GetAsync(new RoomPresence
        {
            RoomId = roomId,
            IdentityId = identityId
        })
        ?? throw new ForbiddenException("Client is not in the room.");

        var messages = await chatMessageRepository.GetByRoomIdBeforeAsync(
            roomId, 
            before, 
            chatOptions.Value.MessageBatchSize
        );
        return messages;
    }

    public async Task<IReadOnlyList<CompleteChatMessage>> GetMessagesAfter(int roomId, int after) 
    {
        var identityId = GetClientIdentityId();
        await EnsureRoomPresence(roomId, identityId);

        var messages = await chatMessageRepository.GetByRoomIdAfterAsync(
            roomId, 
            after, 
            chatOptions.Value.MessageBatchSize
        );
        return messages;
    }

    public async Task<IReadOnlyList<CompleteChatMessage>> GetRecentMessages(int roomId) 
    {
        var identityId = GetClientIdentityId();
        await EnsureRoomPresence(roomId, identityId);

        var messages = await chatMessageRepository.GetByRoomIdAsync(
            roomId, 
            chatOptions.Value.MessageBatchSize
        );
        return messages;
    }
}

   