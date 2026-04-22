using Velochat.Backend.App.Infrastructure.Repositories;
using Velochat.Backend.App.Infrastructure.Models;

namespace Velochat.Backend.App.Infrastructure.Repositories;

public interface IUserRepository
{
    /// <summary>
    /// Retrieves user by ID.
    /// </summary>
    /// <param name="id"></param>
    /// <returns>
    /// A complete model of the retrieved user
    /// or null if not found.
    /// </returns>
    Task<CompleteUser?> GetByIdAsync(int id);

    /// <summary>
    /// Retrieves user by login and password hash.
    /// </summary>
    /// <param name="login"></param>
    /// <param name="passwordHash"></param>
    /// <returns>
    /// A complete model of the retrieved user
    /// or null if not found.
    /// </returns>
    Task<CompleteUser?> GetByCredentialsAsync(string login, string passwordHash);

    /// <summary>
    /// Inserts a new user.
    /// </summary>
    /// <param name="user">
    /// A malleable model of the user to be inserted.
    /// </param>
    /// <returns>A complete model of the created user.</returns>
    /// <exception cref="DuplicateLoginException">
    /// Thrown when an user with the same login already exists.
    /// </exception>
    Task<CompleteUser> CreateAsync(User user);
}
