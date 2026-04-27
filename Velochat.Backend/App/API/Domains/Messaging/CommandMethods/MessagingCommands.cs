
using Microsoft.Extensions.Options;
using Velochat.Backend.App.Infrastructure.Persistence;
using Velochat.Backend.App.Infrastructure.Models;
using Velochat.Backend.App.Shared.Exceptions;
using Velochat.Backend.App.Shared.Options;
using Velochat.Backend.App.API.Realtime.RPCManagement;
using Velochat.Backend.App.API.Realtime.Session;
using Velochat.Backend.App.API.Realtime.Channels;


namespace Velochat.Backend.App.API.Domains.Messaging;

public class MessagingCommands(
    IOptions<ChatOptions> chatOptions,
    ChatMessageRepository chatMessageRepository,
    RoomPresenceRepository roomPresenceRepository,
    RoomFocusCache focusedRoomCache,
    RoomFeedChannels roomFeedChannels
) : IMessagingCommands
{
    public async Task<CompleteChatMessage> SendMessageAsync(
        IRealtimeSession session, string content
    )
    {
        var userId = session.UserId;
        var roomId = focusedRoomCache.GetFocusedRoom(session.ConnectionId);
        await EnsureRoomPresenceAsync(roomId, userId);

        CompleteChatMessage message;
        try
        {

            message = await chatMessageRepository.CreateAsync(new ChatMessage
            {
                RoomId = roomId,
                AuthorId = userId,
                Content = content
            });

            await roomFeedChannels.BroadcastMessage(session, message);
        }
        catch (IdentifierNotFoundException<Room> ex)
        {
            throw new NotFoundException(ex.Message);
        }
        catch (IdentifierNotFoundException<User> ex)
        {
            throw new NotFoundException(ex.Message);
        }

        return message;
    }

    public async Task<GoOlderResponse> GoOlderAsync(
        IRealtimeSession session,
        int oldestMessageOnClient
    )
    {    
        var userId = session.UserId;
        var roomId = focusedRoomCache.GetFocusedRoom(session.ConnectionId);
        await EnsureRoomPresenceAsync(roomId, userId);

        var messages = await chatMessageRepository.GetByRoomIdBeforeAsync(
            roomId, 
            oldestMessageOnClient, 
            chatOptions.Value.MessageBatchSize
        );

        var topReached = messages.Count < chatOptions.Value.MessageBatchSize;
        return new GoOlderResponse
        {
            Messages = messages,
            TopReached = topReached
        };
    }

    public async Task<GoNewerResponse> GoNewerAsync(
        IRealtimeSession session,
        int newestMessageOnClient
    ) 
    {
        var userId = session.UserId;
        var roomId = focusedRoomCache.GetFocusedRoom(session.ConnectionId);
        await EnsureRoomPresenceAsync(roomId, userId);

        var messages = await chatMessageRepository.GetByRoomIdAfterAsync(
            roomId, 
            newestMessageOnClient, 
            chatOptions.Value.MessageBatchSize
        );

        var bottomReached = messages.Count < chatOptions.Value.MessageBatchSize;
        if (bottomReached) await roomFeedChannels.Subscribe(session, roomId);
        return new GoNewerResponse
        {
            Messages = messages,
            BottomReached = bottomReached
        };
    }

    public async Task<SubscribeFeedResponse> SubscribeFeedAsync(
        IRealtimeSession session, int? newestMessageOnClient
    )
    {
        var userId = session.UserId;
        var roomId = focusedRoomCache
            .TryGetFocusedRoom(session.ConnectionId)
            ?? throw new NotFoundException(
                "Client must focus on a room before subscribing to messages."
            );

        await EnsureRoomPresenceAsync(roomId, userId);
        focusedRoomCache.SetFocus(session.ConnectionId, roomId);
        await roomFeedChannels.Subscribe(session, roomId);

        var missedMessages = await chatMessageRepository.GetNewestByRoomIdAsync(
            roomId,
            newestMessageOnClient ?? -1,
            chatOptions.Value.MessageBatchSize
        );

        var IsContinuity = missedMessages.Count < chatOptions.Value.MessageBatchSize;
        
        return new SubscribeFeedResponse
        {
            Messages = missedMessages,
            IsContinuity = IsContinuity
        };
    }

    public async Task UnsubscribeFeedAsync(IRealtimeSession session)
    {
        var focusedRoomId = focusedRoomCache.TryGetFocusedRoom(session.ConnectionId);
        if (focusedRoomId is null) return;
        await roomFeedChannels.Unsubscribe(session, focusedRoomId.Value);
    }

    public async Task<SubscribeFeedResponse> SwitchFocusAsync(
        IRealtimeSession session, int toRoomId, int? newestMessageOnClient
    )
    {
        var userId = session.UserId;
        var currentRoomId = focusedRoomCache
            .TryGetFocusedRoom(session.ConnectionId);

        if (currentRoomId is not null)
        {
            await UnsubscribeFeedAsync(session);
        }
        focusedRoomCache.SetFocus(session.ConnectionId, toRoomId);
        return await SubscribeFeedAsync(session, newestMessageOnClient);
    }

    public async Task ZoneOutAsync(IRealtimeSession session)
    {
        await UnsubscribeFeedAsync(session);
        focusedRoomCache.ClearFocus(session.ConnectionId);
    }

    private async Task EnsureRoomPresenceAsync(int roomId, int userId)
    {
        _ = await roomPresenceRepository.GetAsync(new RoomPresence
        {
            RoomId = roomId,
            UserId = userId
        })
        ?? throw new ForbiddenException("Client is not in the room.");
    }
}