namespace Velochat.Backend.App.Exceptions.StatusExceptions;

public abstract class HttpStatusException(string message, int statusCode) : Exception(message)
{
    public int StatusCode { get; } = statusCode;
}
