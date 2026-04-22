namespace Velochat.Backend.App.Infrastructure.DTOs;

public class FullInvitationDTO
{
    public required int RoomId { get; init; }
    public required string RoomName { get; init; }

    public required int InviteeId { get; init; }

    public required string InviteeLogin { get; init; }

    public required int RoomOwnerId { get; init; }

    public required string RoomOwnerLogin { get; init; }
}
