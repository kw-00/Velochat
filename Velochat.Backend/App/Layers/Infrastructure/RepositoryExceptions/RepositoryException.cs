using Velochat.Backend.App.Shared.Exceptions;

namespace Velochat.Backend.App.Layers.Infrastructure;

public abstract class RepositoryException(string message) : VelochatException(message);