namespace Velochat.Backend.App.Exceptions.RepositoryExceptions;

public class NoInvitationException() 
    : RepositoryException("No invitation found for given user and room IDs.");