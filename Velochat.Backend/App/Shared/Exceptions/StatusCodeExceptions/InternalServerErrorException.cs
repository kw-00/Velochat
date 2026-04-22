namespace Velochat.Backend.App.Shared.Exceptions;

public class InternalServerErrorException(string message) 
    : StatusCodeException(message, 500);