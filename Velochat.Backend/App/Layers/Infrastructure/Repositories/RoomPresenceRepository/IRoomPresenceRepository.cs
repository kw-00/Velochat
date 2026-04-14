using Velochat.Backend.App.Layers.Models;

namespace Velochat.Backend.App.Layers.Infrastructure;

public interface IRoomPresenceRepository
{
    Task<CompleteRoomPresence> CreateAsync(RoomPresence roomPresence);

    Task DeleteAsync(RoomPresence roomPresence);
}