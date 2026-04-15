namespace Velochat.Backend.App.Layers.Domains;

public class ConflictException(string message) 
    : HttpStatusException(message, 409);
