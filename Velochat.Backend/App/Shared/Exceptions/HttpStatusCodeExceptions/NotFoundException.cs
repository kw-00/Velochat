namespace Velochat.Backend.App.Shared.Exceptions;

public class NotFoundException(string message) : HttpStatusException(message);
