namespace Velochat.Backend.App.Exceptions.StatusExceptions;

public class UnauthorizedException(string message) 
    : HttpStatusException(message, 401);
