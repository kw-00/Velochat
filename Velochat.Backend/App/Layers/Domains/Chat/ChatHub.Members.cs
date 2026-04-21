using Microsoft.AspNetCore.Authorization;
using Velochat.Backend.App.Layers.Infrastructure;
using Velochat.Backend.App.Layers.Models;
using Velochat.Backend.App.Shared.Exceptions;
using Velochat.Backend.App.Shared.Metrics;

namespace Velochat.Backend.App.Layers.Domains.Chat;

public partial class ChatHub
{
    [Authorize]
    public async Task Invite(int roomId, int userId)
    {

        var clientUserId = GetClientUserId();
        var inviteeUserId = userId;

        var room = await roomRepository.GetByIdAsync(roomId) 
            ?? throw new NotFoundException($"Room with ID of {roomId} not found.");
            
        if (room.OwnerId != clientUserId) 
            throw new ForbiddenException("Client does not own the room.");

        try {
            var invitation = await invitationRepository.CreateAsync(new Invitation
            {
                RoomId = roomId,
                InviteeId = inviteeUserId
            });
            var invitationDTO = await invitationRepository
                .GetFullInvitationDataAsync(invitation.ToModel())
                
                ?? throw new RaceConditionException(
                    "Invitation or related records disappeared mid-operation."
                );
            await SendInvitedAsync(invitationDTO);
        }
        catch (DuplicatePrimaryKeyException<Invitation>)
        {
            // User was already invited
            VelochatMetrics.Increment(VelochatMetrics.DuplicateInvitation);
        }
        catch (IdentifierNotFoundException<Models.User> ex)
        {
            throw new RaceConditionException(
                $"Client or invitee ID disappeared mid-operation. {ex.Message}"
            );
        }
        catch (IdentifierNotFoundException<Room> ex)
        {
            throw new RaceConditionException(
                $"Room disappeared mid-operation. {ex.Message}"
            );
        }
    }

    [Authorize]
    public async Task RevokeInvitation(int roomId, int userId)
    {
        var clientUserId = GetClientUserId();
        var inviteeUserId = userId;

        var room = await roomRepository.GetByIdAsync(roomId) 
            ?? throw new NotFoundException($"Room with ID of {roomId} not found.");

        if (room.OwnerId != clientUserId) 
            throw new ForbiddenException("Client does not own the room.");

        await invitationRepository.DeleteAsync(new Invitation
        {
            RoomId = roomId,
            InviteeId = inviteeUserId
        });
    }

    [Authorize]
    public async Task KickMember(int roomId, int userId)
    {
        var clientUserId = GetClientUserId();
        var memberUserId = userId;

        var room = await roomRepository.GetByIdAsync(roomId) 
            ?? throw new NotFoundException($"Room with ID of {roomId} not found.");
        
        if (room.OwnerId != clientUserId) 
            throw new ForbiddenException("Client does not own the room.");

        await roomPresenceRepository.DeleteAsync(new RoomPresence
        {
            RoomId = roomId,
            MemberId = memberUserId
        });
        await SendKickedAsync(roomId);
    }
}

   