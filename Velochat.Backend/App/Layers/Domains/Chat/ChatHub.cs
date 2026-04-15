
using Microsoft.AspNetCore.SignalR;
using Velochat.Backend.App.Layers.Infrastructure;


using Velochat.Backend.App.Layers.DTOs;
using System.Security.Claims;
using Velochat.Backend.App.Shared.Options;
using Microsoft.Extensions.Options;

namespace Velochat.Backend.App.Layers.Domains.Chat;

public partial class ChatHub(
        IIdentityRepository identityRepository,
        IRoomRepository roomRepository,
        IInvitationRepository invitationRepository,
        IRoomPresenceRepository roomPresenceRepository,
        IChatMessageRepository chatMessageRepository,
        IOptions<ChatOptions> chatOptions
    ) : Hub, IChatHub
{
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
                MemberOfRoomIds = await roomRepository.GetByMemberIdAsync(identityId),
                InvitedToRoomIds = await roomRepository.GetByInviteeIdAsync(identityId)
            };
        }
        catch (RecordNotFoundException<Models.Identity> ex)
        {
            throw new NotFoundException(ex.Message);
        }
        catch (FormatException ex)
        {
            throw new UnauthorizedException(ex.Message);
        }
    }
}

   