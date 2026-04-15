using Velochat.Backend.App.Shared.Exceptions;

namespace Velochat.Backend.App.Layers.Domains;

public abstract class HttpStatusException(string message, int statusCode) : VelochatException(message)
{
    public int StatusCode { get; } = statusCode;
}
