namespace Velochat.Backend.App.Exceptions.StatusExceptions;

public class NotFoundException(string message) 
    : HttpStatusException(message, 404);
