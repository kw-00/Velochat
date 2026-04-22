namespace Velochat.Backend.App.Infrastructure.Models;

public class CompleteFriendship : ICompleteModel
{
    [PrimaryKey]
    public required int InitiatorId { get; init; }

    [PrimaryKey]
    public required int ReceiverId { get; init; }

    public required bool Accepted { get; init; }
}