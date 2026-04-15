using Velochat.Backend.App.Layers.Infrastructure;
using Velochat.Backend.App.Layers.Models;

namespace Velochat.Backend.App.Layers.Infrastructure;

public interface IRefreshTokenStateRepository
{
    /// <summary>
    /// Retrieves refresh token state by token.
    /// </summary>
    /// <param name="token">The encoded refresh token.</param>
    /// <returns>A complete refresh token state or null if not found.</returns>
    Task<CompleteRefreshTokenState?> GetByTokenAsync(string token);

    /// <summary>
    /// Inserts a new refresh token state.
    /// </summary>
    /// <param name="refreshTokenState">
    /// A malleable model of refresh token state.
    /// </param>
    /// <returns></returns>
    /// <exception cref="ModelNotInsertableException"></exception>
    Task CreateAsync(RefreshTokenState refreshTokenState);

    /// <summary>
    /// Updates a refresh token state.
    /// </summary>
    /// <param name="refreshTokenState">
    /// A complete model of refresh token state.
    /// </param>
    /// <returns></returns>
    Task UpdateAsync(CompleteRefreshTokenState refreshTokenState);

    /// <summary>
    /// Sets a refresh token status state to revoked.
    /// </summary>
    /// <param name="token">
    /// The encoded refresh token.
    /// </param>
    /// <returns></returns>
    Task RevokeAsync(string token);

    /// <summary>
    /// Sets all refresh token states for an identity to revoked.
    /// </summary>
    /// <param name="identityId"></param>
    /// <returns></returns>
    /// <exception cref="RecordNotFoundException{Identity}"></exception>
    Task RevokeByIdentityIdAsync(int identityId);
}
