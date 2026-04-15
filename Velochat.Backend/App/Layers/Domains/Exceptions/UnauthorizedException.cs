namespace Velochat.Backend.App.Layers.Domains;

public class UnauthorizedException(string message) 
    : HttpStatusException(message, 401);
