namespace Velochat.Backend.App.Exceptions.RepositoryExceptions;

public class MissingRoomPresenceException() 
    : RepositoryException(
        "Identity and room with given IDs are not connected."
        + " Room presence not found."
    );