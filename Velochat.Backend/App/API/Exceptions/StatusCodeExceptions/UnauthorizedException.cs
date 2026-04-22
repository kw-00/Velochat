namespace Velochat.Backend.App.Shared.Exceptions;

public class UnauthorizedException(string message) 
    : StatusCodeException(message, 401);
