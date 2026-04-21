
using Microsoft.AspNetCore.Authorization;
using Velochat.Backend.App.Layers.Infrastructure;
using Velochat.Backend.App.Layers.Models;
using Velochat.Backend.App.Shared.Exceptions;

namespace Velochat.Backend.App.Layers.Domains.Chat;

public partial class ChatHub
{
    [Authorize]
    public async Task<CompleteRoom> CreateRoom(string name)
    {

        var userId = GetClientUserId();
        try
        {
            var room = await roomRepository.CreateAsync(new Room
            {
                Name = name,
                OwnerId = userId
            });
            await SubscribeToMessageFeed(room.Id);
            return room;
        }
        catch (DuplicateRoomPathException ex)
        {
            throw new ConflictException(ex.Message);
        }
    }

    [Authorize]
    public async Task DestroyRoom(int roomId)
    {
        var userId = GetClientUserId();
        var roomToRemove = await roomRepository.GetByIdAsync(roomId) 
            ?? throw new NotFoundException("Room not found.");

        if (roomToRemove.OwnerId != userId) 
            throw new ForbiddenException("Client does not own the room.");
        await roomRepository.DeleteAsync(roomToRemove.Id);
        await SendRoomClosedAsync(roomId);
    }

    [Authorize]
    public async Task<CompleteRoom> JoinRoom(int roomId)
    {
        var userId = GetClientUserId();
        var invitation = await invitationRepository.GetAsync(new Invitation
        {
            RoomId = roomId,
            InviteeId = userId
        }) 
        ?? throw new ForbiddenException("Client is not invited to the room.");

        try
        {
            await roomPresenceRepository.CreateAsync(new RoomPresence
            {
                RoomId = invitation.RoomId,
                MemberId = invitation.InviteeId
            });
            var room = await roomRepository.GetByIdAsync(roomId)
                ?? throw new RaceConditionException(
                    $"Room disappeared mid-operation."
                );
            return room;
        }
        catch (DuplicatePrimaryKeyException<RoomPresence> ex)
        {
            throw new ConflictException(ex.Message);
        }
        catch (IdentifierNotFoundException<Room> ex)
        {
            throw new RaceConditionException(
                $"Room disappeared mid-operation. {ex.Message}"
            );
        }
        catch (IdentifierNotFoundException<Models.User> ex)
        {
            throw new RaceConditionException(
                $"User disappeared mid-operation. {ex.Message}"
            );
        }
    }

    public async Task LeaveRoom(int roomId)
    {
        var userId = GetClientUserId();
        await roomPresenceRepository.DeleteAsync(new RoomPresence
        {
            RoomId = roomId,
            MemberId = userId
        });
        await UnsubscribeFromMessageFeed(roomId);
    }
}

   