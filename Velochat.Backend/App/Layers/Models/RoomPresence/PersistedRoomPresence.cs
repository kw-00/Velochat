namespace Velochat.Backend.App.Layers.Models;

public class PersistedRoomPresence : IPersistedModel<RoomPresence>
{
    public required int RoomId { get; init; }
    public required int IdentityId { get; init; }

    public RoomPresence ToModel() => new()
    {
        RoomId = RoomId,
        IdentityId = IdentityId
    };
}
