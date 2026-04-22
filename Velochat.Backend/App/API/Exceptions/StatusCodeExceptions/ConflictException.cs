namespace Velochat.Backend.App.Shared.Exceptions;

public class ConflictException(string message) 
    : StatusCodeException(message, 409);
