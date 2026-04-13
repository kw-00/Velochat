namespace Velochat.Backend.App.Exceptions;

public class NotFoundException(string message) : HttpStatusException(message);
