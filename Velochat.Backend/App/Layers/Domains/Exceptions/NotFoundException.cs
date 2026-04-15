namespace Velochat.Backend.App.Layers.Domains;

public class NotFoundException(string message) 
    : HttpStatusException(message, 404);
