namespace Velochat.Backend.App.Layers.Domains;

public class ForbiddenException(string message) 
    : HttpStatusException(message, 403);