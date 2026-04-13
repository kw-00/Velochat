using Velochat.Backend.App.Layers.Models;

namespace Velochat.Backend.App.Layers.Infrastructure;

public interface IRefreshTokenStateRepository
{
    public Task<CompleteRefreshTokenState?> GetByTokenAsync(string token);

    public Task CreateAsync(CompleteRefreshTokenState refreshTokenState);

    public Task<CompleteRefreshTokenState> UpdateAsync(CompleteRefreshTokenState refreshTokenState);

    public Task RevokeAsync(string token);
    public Task RevokeAllByIdentityIdAsync(int identityId);
}
