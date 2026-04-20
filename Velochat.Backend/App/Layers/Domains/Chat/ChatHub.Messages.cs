
using System.Collections.Concurrent;
using System.Diagnostics;
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
    public async Task<GoOlderResponse> GoOlder(
        int oldestMessageOnClient
    )
    {
        var identityId = GetClientIdentityId();
        var roomId = currentChatroomCache.GetCurrentChatroom(Context.ConnectionId);
        await EnsureRoomPresenceAsync(roomId, identityId);

        var messages = await chatMessageRepository.GetByRoomIdBeforeAsync(
            roomId, 
            oldestMessageOnClient, 
            chatOptions.Value.MessageBatchSize
        );

        var topReached = messages.Count < chatOptions.Value.MessageBatchSize;
        await UnsubscribeFromMessageFeed(roomId);
        return new GoOlderResponse
        {
            Messages = messages,
            TopReached = topReached
        };
    }

    [Authorize]
    public async Task<GoNewerResponse> GoNewer(
        int newestMessageOnClient
    ) 
    {
        var identityId = GetClientIdentityId();
        var roomId = currentChatroomCache.GetCurrentChatroom(Context.ConnectionId);
        await EnsureRoomPresenceAsync(roomId, identityId);

        var messages = await chatMessageRepository.GetByRoomIdAfterAsync(
            roomId, 
            newestMessageOnClient, 
            chatOptions.Value.MessageBatchSize
        );

        var bottomReached = messages.Count < chatOptions.Value.MessageBatchSize;
        if (bottomReached) await SubscribeToMessageFeed(roomId);
        return new GoNewerResponse
        {
            Messages = messages,
            BottomReached = bottomReached
        };
    }

    [Authorize]
    public async Task<SwitchRoomsResponse> SwitchRooms(
        int roomId, int? newestMessageOnClient
    )
    {
        var identityId = GetClientIdentityId();
        await EnsureRoomPresenceAsync(roomId, identityId);
        await UnsubscribeFromMessageFeed(
            currentChatroomCache.GetCurrentChatroom(Context.ConnectionId)
        );
        currentChatroomCache.SetCurrentChatroom(Context.ConnectionId, roomId);
        await SubscribeToMessageFeed(roomId);
        var missedMessages = await chatMessageRepository.GetNewestByRoomIdAsync(
            roomId,
            newestMessageOnClient ?? -1,
            chatOptions.Value.MessageBatchSize
        );

        var IsContinuity = missedMessages.Count < chatOptions.Value.MessageBatchSize;
        
        return new SwitchRoomsResponse
        {
            Messages = missedMessages,
            IsContinuity = IsContinuity
        };
    }

    [Authorize]
    public async Task UnsubscribeFromMessageFeed()
    {
        var currentRoom = currentChatroomCache.GetCurrentChatroom(Context.ConnectionId);
        await UnsubscribeFromMessageFeed(currentRoom);
    }
}

   