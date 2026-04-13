using Velochat.Backend.App.Layers.Models;

namespace Velochat.Backend.App.Layers.Infrastructure;

public interface IRefreshTokenStateRepository
{
    public Task<PersistedRefreshTokenState?> GetByTokenAsync(string token);

    public Task CreateAsync(PersistedRefreshTokenState refreshTokenState);

    public Task<PersistedRefreshTokenState> UpdateAsync(PersistedRefreshTokenState refreshTokenState);

    public Task RevokeAsync(string token);
    public Task RevokeAllByIdentityIdAsync(int identityId);
}
