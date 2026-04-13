using Velochat.Backend.App.Layers.Models;

namespace Velochat.Backend.App.Layers.Infrastructure;

public interface IRefreshTokenStateRepository
{
    Task<CompleteRefreshTokenState?> GetByTokenAsync(string token);

    Task CreateAsync(CompleteRefreshTokenState refreshTokenState);

    Task UpdateAsync(CompleteRefreshTokenState refreshTokenState);

    Task RevokeAsync(string token);
    Task RevokeAllByIdentityIdAsync(int identityId);
}
