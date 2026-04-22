using Velochat.Backend.App.Infrastructure.Models;

namespace Velochat.Backend.App.Infrastructure.DTOs
{
    public class InitialChatInformation
    {
        public required IReadOnlyList<CompleteRoom> Rooms { get; set; }
        public required IReadOnlyList<FullInvitationDTO> Invitations{ get; set; }
    }
}