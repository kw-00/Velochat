namespace Velochat.Backend.App.Shared.Exceptions;

public abstract class StatusCodeException(string message, int statusCode) : VelochatException(message)
{
    public int StatusCode { get; } = statusCode;
}
