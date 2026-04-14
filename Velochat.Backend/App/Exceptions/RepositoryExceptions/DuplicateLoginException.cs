namespace Velochat.Backend.App.Exceptions.RepositoryExceptions;

public class DuplicateLoginException() 
    : RepositoryException("Login already exists in database.");