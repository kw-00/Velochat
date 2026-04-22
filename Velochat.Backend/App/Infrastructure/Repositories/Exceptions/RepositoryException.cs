using Velochat.Backend.App.Shared.Exceptions;

namespace Velochat.Backend.App.Infrastructure.Repositories;

public abstract class RepositoryException(string message) : VelochatException(message);