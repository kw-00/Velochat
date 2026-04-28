using Velochat.Backend.App.Infrastructure.DTOs;
using Velochat.Backend.App.Infrastructure.Models;

namespace Velochat.Backend.App.API.Domains.Identity;

public interface IIdentityOrchestration
{
    Task<SessionInitData> RegisterAsync(Credentials credentials);

    Task<SessionInitData> LogInAsync(Credentials credentials);

    Task<SessionInitData> RefreshSessionAsync(string refreshTokenString);

    Task LogOutAsync(string refreshTokenString);

}
