using Velochat.Backend.App.Infrastructure.Models;

namespace Velochat.Backend.App.Infrastructure.Repositories;

public class DuplicateRoomPathException(Room room) 
    : RepositoryException(
        $"Room with OwnerId/Name of {room.OwnerId}/\"{room.Name}\""
        + " already exists in database."
    );