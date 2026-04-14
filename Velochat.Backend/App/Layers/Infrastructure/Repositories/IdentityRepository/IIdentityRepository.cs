using Velochat.Backend.App.Exceptions.RepositoryExceptions;
using Velochat.Backend.App.Layers.Models;

namespace Velochat.Backend.App.Layers.Infrastructure;

public interface IIdentityRepository
{
    /// <summary>
    /// Retrieves identity by ID.
    /// </summary>
    /// <param name="id"></param>
    /// <returns>
    /// A complete model of the retrieved identity
    /// or null if not found.
    /// </returns>
    Task<CompleteIdentity?> GetByIdAsync(int id);

    /// <summary>
    /// Retrieves identity by login and password hash.
    /// </summary>
    /// <param name="login"></param>
    /// <param name="passwordHash"></param>
    /// <returns>
    /// A complete model of the retrieved identity
    /// or null if not found.
    /// </returns>
    Task<CompleteIdentity?> GetByCredentialsAsync(string login, string passwordHash);

    /// <summary>
    /// Inserts a new identity.
    /// </summary>
    /// <param name="identity">
    /// A malleable model of the identity to be inserted.
    /// </param>
    /// <returns>A complete model of the created identity.</returns>
    /// <exception cref="DuplicateLoginException">
    /// Thrown when an identity with the same login already exists.
    /// </exception>
    Task<CompleteIdentity> CreateAsync(Identity identity);
}
