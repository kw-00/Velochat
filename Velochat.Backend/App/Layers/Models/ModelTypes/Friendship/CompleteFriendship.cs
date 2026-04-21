namespace Velochat.Backend.App.Layers.Models;

public class CompleteFriendship : ICompleteModel
{
    [PrimaryKey]
    public int Subject1Id { get; init; }

    [PrimaryKey]
    public int Subject2Id { get; init; }
}