namespace Velochat.Backend.App.Shared.Exceptions;

public class NotFoundException(string message) 
    : StatusCodeException(message, 404);
