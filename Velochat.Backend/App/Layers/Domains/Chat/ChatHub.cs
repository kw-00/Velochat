
using Microsoft.AspNetCore.SignalR;
using Velochat.Backend.App.Layers.Infrastructure;


using Velochat.Backend.App.Layers.DTOs;
using System.Security.Claims;
using Velochat.Backend.App.Shared.Options;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authorization;

namespace Velochat.Backend.App.Layers.Domains.Chat;

public partial class ChatHub(
        IRoomRepository roomRepository,
        IInvitationRepository invitationRepository,
        IRoomPresenceRepository roomPresenceRepository,
        IChatMessageRepository chatMessageRepository,
        IOptions<ChatOptions> chatOptions,
        CurrentChatroomCache currentChatroomCache
    ) : Hub, IChatHub
{

    
    [Authorize]
    public async Task<InitialChatInformation> GetInitialChatInformation()
    {
        try
        {
            var userIdString = Context
                .GetHttpContext()
                ?.User
                .Claims
                .First(c => c.Type == ClaimTypes.NameIdentifier)
                .Value
                ?? throw new UnauthorizedException("User identifier (sub) is missing.");
            
            var userId = int.Parse(userIdString);
            return new InitialChatInformation
            {
                Rooms = await roomRepository.GetByMemberIdAsync(userId),
                Invitations = await invitationRepository.GetFullInvitationDataAsync(userId)
            };
        }
        catch (IdentifierNotFoundException<Models.User> ex)
        {
            throw new NotFoundException(ex.Message);
        }
        catch (FormatException ex)
        {
            throw new UnauthorizedException(ex.Message);
        }
    }
}

   