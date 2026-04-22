namespace Velochat.Backend.App.Shared.Exceptions;

public class VelochatException : Exception
{
    public VelochatException() : base() { }
    public VelochatException(string message) : base(message) { }

    public VelochatException(string message, Exception innerException) 
        : base(message, innerException) { }
}