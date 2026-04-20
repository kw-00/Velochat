namespace Velochat.Backend.App.Shared;

public interface IHTTPContextWrapper
{
    HttpContext HttpContext { get; }

    int ClientIdentity { get; }
}