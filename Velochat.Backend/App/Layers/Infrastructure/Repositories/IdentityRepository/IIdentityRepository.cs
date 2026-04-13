using Velochat.Backend.App.Layers.Models;

namespace Velochat.Backend.App.Layers.Infrastructure;

public interface IIdentityRepository
{
    Task<PersistedIdentity?> GetByIdAsync(int id);
    Task<PersistedIdentity?> GetByCredentialsAsync(string login, string passwordHash);
    Task<PersistedIdentity> CreateAsync(Identity identity);
}
