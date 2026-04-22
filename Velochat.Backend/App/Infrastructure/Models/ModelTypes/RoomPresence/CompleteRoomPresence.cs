namespace Velochat.Backend.App.Infrastructure.Models;

public class CompleteRoomPresence : ICompleteModel
{
    [PrimaryKey]
    public required int RoomId { get; init; }
    [PrimaryKey]
    public required int MemberId { get; init; }
}
