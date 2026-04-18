
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
        IOptions<ChatOptions> chatOptions
    ) : Hub, IChatHub
{

    
    [Authorize]
    public async Task<InitialChatInformation> GetInitialChatInformation()
    {
        try
        {
            var identityIdString = Context
                .GetHttpContext()
                ?.User
                .Claims
                .First(c => c.Type == ClaimTypes.NameIdentifier)
                .Value
                ?? throw new UnauthorizedException("User identifier (sub) is missing.");
            
            var identityId = int.Parse(identityIdString);
            return new InitialChatInformation
            {
                Rooms = await roomRepository.GetByMemberIdAsync(identityId),
                Invitations = await invitationRepository.GetFullInvitationDataAsync(identityId)
            };
        }
        catch (IdentifierNotFoundException<Models.Identity> ex)
        {
            throw new NotFoundException(ex.Message);
        }
        catch (FormatException ex)
        {
            throw new UnauthorizedException(ex.Message);
        }
    }
}

   