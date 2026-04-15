namespace Velochat.Backend.App.Layers.Models;

public class CompleteRoomPresence : ICompleteModel
{
    [PrimaryKey]
    public required int RoomId { get; init; }
    [PrimaryKey]
    public required int MemberId { get; init; }
}
