namespace Velochat.Backend.App.Infrastructure.Persistence;

public class MissingInsertionReturnValue() 
    : RepositoryException(
        "Insertion was successful, but nothing was returned"
        + " despite RETURNING clause."
    );