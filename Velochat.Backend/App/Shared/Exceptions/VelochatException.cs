namespace Velochat.Backend.App.Shared.Exceptions;

public class VelochatException : Exception
{
    public VelochatException(string message) : base(message) { }
    public VelochatException() : base() { }
}