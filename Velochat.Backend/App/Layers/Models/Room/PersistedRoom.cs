namespace Velochat.Backend.App.Layers.Models;

public class PersistedRoom : IPersistedModel<Room>
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public required int OwnerId { get; init; }

    public Room ToModel() => new()
    {
        Id = Id,
        Name = Name,
        OwnerId = OwnerId
    };
}
