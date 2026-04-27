namespace Velochat.Backend.App.Infrastructure.Persistence;

public interface IPasswordService
{
    string HashPassword(string password);

    bool Verify(string password, string hash);
}
