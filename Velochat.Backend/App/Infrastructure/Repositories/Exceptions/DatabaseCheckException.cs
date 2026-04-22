namespace Velochat.Backend.App.Infrastructure.Repositories;

public class DatabaseCheckException(string message) : RepositoryException(message);