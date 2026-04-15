namespace Velochat.Backend.App.Layers.Domains;

public class BadRequestException(string message) 
    : HttpStatusException(message, 400);
