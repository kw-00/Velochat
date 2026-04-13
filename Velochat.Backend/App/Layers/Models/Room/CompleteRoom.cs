namespace Velochat.Backend.App.Layers.Models;

public class CompleteRoom : ICompleteModel
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public required int OwnerId { get; init; }
}
