namespace Velochat.Backend.App.Exceptions;

public class UnauthorizedException(string message) : HttpStatusException(message);
