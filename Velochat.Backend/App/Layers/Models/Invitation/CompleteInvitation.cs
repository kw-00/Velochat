namespace Velochat.Backend.App.Layers.Models;

public class CompleteInvitation : ICompleteModel
{
    public required int RoomId { get; init; }
    public required int InviteeId { get; init; }
}
