
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

        var identityId = GetClientIdentityId();
        try
        {
            var room = await roomRepository.CreateAsync(new Room
            {
                Name = name,
                OwnerId = identityId
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
        var identityId = GetClientIdentityId();
        var roomToRemove = await roomRepository.GetByIdAsync(roomId) 
            ?? throw new NotFoundException("Room not found.");

        if (roomToRemove.OwnerId != identityId) 
            throw new ForbiddenException("Client does not own the room.");
        await roomRepository.DeleteAsync(roomToRemove.Id);
        await SendRoomClosedAsync(roomId);
    }

    [Authorize]
    public async Task<CompleteRoom> JoinRoom(int roomId)
    {
        var identityId = GetClientIdentityId();
        var invitation = await invitationRepository.GetAsync(new Invitation
        {
            RoomId = roomId,
            InviteeId = identityId
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
        catch (IdentifierNotFoundException<Models.Identity> ex)
        {
            throw new RaceConditionException(
                $"Identity disappeared mid-operation. {ex.Message}"
            );
        }
    }

    public async Task LeaveRoom(int roomId)
    {
        var identityId = GetClientIdentityId();
        await roomPresenceRepository.DeleteAsync(new RoomPresence
        {
            RoomId = roomId,
            MemberId = identityId
        });
        await UnsubscribeFromMessageFeed(roomId);
    }
}

   