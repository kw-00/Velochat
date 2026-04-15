using Velochat.Backend.App.Layers.Models;

namespace Velochat.Backend.App.Layers.Infrastructure;

public class DuplicateRoomPathException(Room room) 
    : RepositoryException(
        $"Room with OwnerId/Name of {room.OwnerId}/\"{room.Name}\""
        + " already exists in database."
    );