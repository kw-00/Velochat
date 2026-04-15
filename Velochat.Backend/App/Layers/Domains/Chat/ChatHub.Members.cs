using Velochat.Backend.App.Layers.Infrastructure;
using Velochat.Backend.App.Layers.Models;
using Velochat.Backend.App.Shared.Exceptions;

namespace Velochat.Backend.App.Layers.Domains.Chat;

public partial class ChatHub
{
    public async Task Invite(int roomId, int identityId)
    {

        var clientIdentityId = GetClientIdentityId();
        var inviteeIdentityId = identityId;

        var room = await roomRepository.GetByIdAsync(roomId) 
            ?? throw new NotFoundException($"Room with ID of {roomId} not found.");
            
        if (room.OwnerId != clientIdentityId) 
            throw new ForbiddenException("Client does not own the room.");

        try {
            await invitationRepository.CreateAsync(new Invitation
            {
                RoomId = roomId,
                InviteeId = inviteeIdentityId
            });
        }
        catch (DuplicatePrimaryKeyException<Invitation> ex)
        {
            throw new ConflictException(ex.Message);
        }
        catch (RecordNotFoundException<Models.Identity> ex)
        {
            throw new RaceConditionException(
                $"Client or invitee ID disappeared mid-operation. {ex.Message}"
            );
        }
        catch (RecordNotFoundException<Room> ex)
        {
            throw new RaceConditionException(
                $"Room disappeared mid-operation. {ex.Message}"
            );
        }
    }

    public async Task RevokeInvitation(int roomId, int identityId)
    {
        var clientIdentityId = GetClientIdentityId();
        var inviteeIdentityId = identityId;

        var room = await roomRepository.GetByIdAsync(roomId) 
            ?? throw new NotFoundException($"Room with ID of {roomId} not found.");

        if (room.OwnerId != clientIdentityId) 
            throw new ForbiddenException("Client does not own the room.");

        await invitationRepository.DeleteAsync(new Invitation
        {
            RoomId = roomId,
            InviteeId = inviteeIdentityId
        });
    }

    public async Task KickMember(int roomId, int identityId)
    {
        var clientIdentityId = GetClientIdentityId();
        var memberIdentityId = identityId;

        var room = await roomRepository.GetByIdAsync(roomId) 
            ?? throw new NotFoundException($"Room with ID of {roomId} not found.");
        
        if (room.OwnerId != clientIdentityId) 
            throw new ForbiddenException("Client does not own the room.");

        await roomPresenceRepository.DeleteAsync(new RoomPresence
        {
            RoomId = roomId,
            IdentityId = memberIdentityId
        });
    }
}

   