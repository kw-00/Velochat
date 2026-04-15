
using Velochat.Backend.App.Layers.Infrastructure;
using Velochat.Backend.App.Layers.Models;
using Velochat.Backend.App.Shared.Exceptions;

namespace Velochat.Backend.App.Layers.Domains.Chat;

public partial class ChatHub
{
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
            return room;
        }
        catch (DuplicateRoomPathException ex)
        {
            throw new ConflictException(ex.Message);
        }
    }

    public async Task DestroyRoom(int roomId)
    {
        var identityId = GetClientIdentityId();
        var roomToRemove = await roomRepository.GetByIdAsync(roomId) 
            ?? throw new NotFoundException("Room not found.");

        if (roomToRemove.OwnerId != identityId) 
            throw new ForbiddenException("Client does not own the room.");
        await roomRepository.DeleteAsync(roomToRemove.Id);
    }

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
            var room = await roomRepository.GetByIdAsync(roomId);
            return room
                ?? throw new RaceConditionException(
                    $"Room disappeared mid-operation."
                );
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
}

   