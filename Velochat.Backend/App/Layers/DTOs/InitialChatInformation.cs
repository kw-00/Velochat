using Velochat.Backend.App.Layers.Models;

namespace Velochat.Backend.App.Layers.DTOs
{
    public class InitialChatInformation
    {
        public required IReadOnlyList<CompleteRoom> Rooms { get; set; }
        public required IReadOnlyList<FullInvitationDTO> Invitations{ get; set; }
    }
}