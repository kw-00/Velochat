

namespace Velochat.Backend.App.Infrastructure.Repositories;

public class DuplicateLoginException(string login) 
    : RepositoryException($"Login of {login} already exists in database.");