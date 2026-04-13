using Velochat.Backend.App.Layers.Models;

namespace Velochat.Backend.App.Layers.Infrastructure;

public interface IIdentityRepository
{
    Task<CompleteIdentity?> GetByIdAsync(int id);
    Task<CompleteIdentity?> GetByCredentialsAsync(string login, string passwordHash);
    Task<CompleteIdentity> CreateAsync(Identity identity);
}
