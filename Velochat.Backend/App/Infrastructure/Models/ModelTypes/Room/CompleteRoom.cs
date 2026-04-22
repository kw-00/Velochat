namespace Velochat.Backend.App.Infrastructure.Models;

public class CompleteRoom : ICompleteModel
{
    [PrimaryKey]
    public required int Id { get; init; }
    public required string Name { get; init; }
    public required int OwnerId { get; init; }
}
