
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
    FullRoomUpdateChannels fullRoomUpdateChannels
)
{
    public async Task<CompleteChatMessage> SendMessage(
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

            await fullRoomUpdateChannels.BroadcastMessage(session, message);
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

    public async Task<GoOlderResponse> GoOlder(
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
        await UnsubscribeFromMessageFeed(session);
        return new GoOlderResponse
        {
            Messages = messages,
            TopReached = topReached
        };
    }

    public async Task<GoNewerResponse> GoNewer(
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
        if (bottomReached) await SubscribeToMessageFeed(session);
        return new GoNewerResponse
        {
            Messages = messages,
            BottomReached = bottomReached
        };
    }

    public async Task<SwitchRoomsResponse> SwitchRooms(
        IRealtimeSession session, int toRoomId, int? newestMessageOnClient
    )
    {
        var userId = session.UserId;
        var currentChatroomId = focusedRoomCache
            .TryGetFocusedRoom(session.ConnectionId);

        if (currentChatroomId is not null)
        {
            await UnsubscribeFromMessageFeed(session);
        }

        await EnsureRoomPresenceAsync(toRoomId, userId);
        focusedRoomCache.SetFocus(session.ConnectionId, toRoomId);
        await SubscribeToMessageFeed(session);
        var missedMessages = await chatMessageRepository.GetNewestByRoomIdAsync(
            toRoomId,
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

    public async Task ZoneOut(IRealtimeSession session)
    {
        await UnsubscribeFromMessageFeed(session);
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

    private async Task SubscribeToMessageFeed(IRealtimeSession session)
    {
        var focusedRoomId = focusedRoomCache.GetFocusedRoom(session.ConnectionId);
        await fullRoomUpdateChannels.Subscribe(session, focusedRoomId);
    }

    private async Task UnsubscribeFromMessageFeed(IRealtimeSession session)
    {
        var focusedRoomId = focusedRoomCache.TryGetFocusedRoom(session.ConnectionId);
        if (focusedRoomId is null) return;
        await fullRoomUpdateChannels.Unsubscribe(session, focusedRoomId.Value);
    }
}