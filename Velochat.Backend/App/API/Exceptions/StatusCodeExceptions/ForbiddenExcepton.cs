namespace Velochat.Backend.App.Shared.Exceptions;

public class ForbiddenException(string message) 
    : StatusCodeException(message, 403);