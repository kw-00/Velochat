namespace Velochat.Backend.App.Layers.Infrastructure.RepositoryExceptions;

public class DatabaseCheckException(string message) : RepositoryException(message);