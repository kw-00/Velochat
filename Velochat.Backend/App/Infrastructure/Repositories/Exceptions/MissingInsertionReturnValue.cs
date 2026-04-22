namespace Velochat.Backend.App.Infrastructure.Repositories;

public class MissingInsertionReturnValue() 
    : RepositoryException(
        "Insertion was successful, but nothing was returned"
        + " despite RETURNING clause."
    );