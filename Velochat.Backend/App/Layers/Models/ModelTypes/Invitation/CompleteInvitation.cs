namespace Velochat.Backend.App.Layers.Models;

public class CompleteInvitation : ICompleteModel
{
    [PrimaryKey]
    public required int RoomId { get; init; }
    [PrimaryKey]
    public required int InviteeId { get; init; }
}
