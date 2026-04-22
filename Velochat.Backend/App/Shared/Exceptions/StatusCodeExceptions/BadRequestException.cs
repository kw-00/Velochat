namespace Velochat.Backend.App.Shared.Exceptions;

public class BadRequestException(string message) 
    : StatusCodeException(message, 400);
