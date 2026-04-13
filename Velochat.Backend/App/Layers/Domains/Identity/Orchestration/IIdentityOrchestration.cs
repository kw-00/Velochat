using System.IdentityModel.Tokens.Jwt;
using Velochat.Backend.App.Layers.DTOs;
using Velochat.Backend.App.Layers.Models;

namespace Velochat.Backend.App.Layers.Domains.Chat;

public interface IIdentityOrchestration
{
    Task<(CompleteIdentity Identity, TokenPair TokenPair)> RegisterAsync(string login, string password);

    Task<TokenPair> LogInAsync(string login, string password);

    Task<TokenPair> RefreshTokenAsync(string refreshTokenString);

    Task LogOutAsync(string refreshTokenString);

}
