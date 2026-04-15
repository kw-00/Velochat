using Velochat.Backend.App.Layers.Models;

namespace Velochat.Backend.App.Layers.DTOs
{
    public class InitialChatInformation
    {
        public required IReadOnlyList<CompleteRoom> MemberOfRoomIds { get; set; }
        public required IReadOnlyList<CompleteRoom> InvitedToRoomIds{ get; set; }
    }
}