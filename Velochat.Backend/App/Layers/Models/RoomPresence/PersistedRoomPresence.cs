namespace Velochat.Backend.App.Layers.Models;

public class CompleteRoomPresence : ICompleteModel
{
    public required int RoomId { get; init; }
    public required int IdentityId { get; init; }
}
