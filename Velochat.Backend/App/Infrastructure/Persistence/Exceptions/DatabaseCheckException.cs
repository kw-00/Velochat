namespace Velochat.Backend.App.Infrastructure.Persistence;

public class DatabaseCheckException(string message) : RepositoryException(message);