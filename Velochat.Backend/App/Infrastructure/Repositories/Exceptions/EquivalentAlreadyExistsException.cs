using Velochat.Backend.App.Infrastructure.Models;

namespace Velochat.Backend.App.Infrastructure.Repositories;

public class EquivalentAlreadyExistsException<T>() : RepositoryException(
    $"An equivalent of the given {typeof(T).Name} already exists in database."
) where T : IModel;