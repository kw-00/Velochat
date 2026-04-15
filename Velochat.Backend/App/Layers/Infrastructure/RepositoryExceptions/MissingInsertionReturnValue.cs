namespace Velochat.Backend.App.Layers.Infrastructure.RepositoryExceptions;

public class MissingInsertionReturnValue() 
    : RepositoryException(
        "Insertion was successful, but nothing was returned"
        + " despite RETURNING clause."
    );