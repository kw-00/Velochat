

namespace Velochat.Backend.App.Layers.Infrastructure;

public class DuplicateLoginException(string login) 
    : RepositoryException($"Login of {login} already exists in database.");