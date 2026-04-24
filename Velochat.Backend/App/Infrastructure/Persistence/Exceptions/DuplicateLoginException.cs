

namespace Velochat.Backend.App.Infrastructure.Persistence;

public class DuplicateLoginException(string login) 
    : RepositoryException($"Login of {login} already exists in database.");