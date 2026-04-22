namespace Velochat.Backend.App.Infrastructure.Repositories;

public interface IPasswordService
{
    string HashPassword(string password);
}
