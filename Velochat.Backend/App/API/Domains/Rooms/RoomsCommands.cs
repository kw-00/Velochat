using Velochat.Backend.App.API.Realtime.Channels;
using Velochat.Backend.App.API.Realtime.Session;
using Velochat.Backend.App.Infrastructure.Models;
using Velochat.Backend.App.Infrastructure.Persistence;
using Velochat.Backend.App.Shared.Exceptions;

namespace Velochat.Backend.App.API.Domains.Rooms;

public class RoomsCommands(
    RoomRepository roomRepository,
    RoomPresenceRepository roomPresenceRepository,
    FriendshipRepository friendshipRepository,
    UserRepository userRepository,
    RoomFeedChannels fullRoomUpdateChannels,
    UserNotificationChannels userNotificationChannels
) : IRoomsCommands
{
    public Task<IReadOnlyList<CompleteRoom>> GetRoomsAsync(IRealtimeSession session)
    {
        var rooms = roomRepository.GetByMemberIdAsync(session.UserId);
        return rooms;
    }

    public async Task<CompleteRoom> CreateRoomAsync(
        IRealtimeSession session, string name
    )
    {
        var room = await roomRepository.CreateAsync(
            new Room { Name = name }, 
            session.UserId
        );
        return room;
    }

    public async Task LeaveRoomAsync(IRealtimeSession session, int roomId)
    {
        await roomPresenceRepository.DeleteAsync(new RoomPresence
        {
            RoomId = roomId,
            UserId = session.UserId
        });
        await fullRoomUpdateChannels.BroadcastUserLeft(session, roomId, session.UserId);
    }

    public async Task AddUserAsync(
        IRealtimeSession session, int roomId, int userId
    )
    {
        await EnsureRoomPresenceAsync(roomId, session.UserId);
        var friendship = await friendshipRepository
            .GetFriendshipAsync(userId, session.UserId);

        
        if (friendship is null || friendship.Accepted) 
            throw new ForbiddenException(
                $"Users with IDs of {session.UserId} and {userId} are not friends."
            );

        await roomPresenceRepository.CreateAsync(new RoomPresence
        {
            RoomId = roomId,
            UserId = userId
        });

        var addedUser = await userRepository.GetByIdAsync(userId) 
            ?? throw new RaceConditionException(
                "User not found in database despite just having been added to a room."
                + " Likely race condition."
            );
        await fullRoomUpdateChannels.BroadcastUserJoined(session, roomId, addedUser);

        var room = await roomRepository.GetByIdAsync(roomId) 
            ?? throw new RaceConditionException(
                "Room not found in database, even though a user was just added to it."
                + " Likely race condition."
            );
        await userNotificationChannels.SendAddedToRoom(session, roomId, room);
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