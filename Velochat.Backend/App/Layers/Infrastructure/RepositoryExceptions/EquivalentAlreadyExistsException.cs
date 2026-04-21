using Velochat.Backend.App.Layers.Models;

namespace Velochat.Backend.App.Layers.Infrastructure;

public class EquivalentAlreadyExistsException<T>() : RepositoryException(
    $"An equivalent of the given {typeof(T).Name} already exists in database."
) where T : IModel;