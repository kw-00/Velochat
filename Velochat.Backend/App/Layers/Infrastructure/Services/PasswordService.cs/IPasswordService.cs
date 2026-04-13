namespace Velochat.Backend.App.Layers.Infrastructure;

public interface IPasswordService
{
    string HashPassword(string password);
}
