using System.IdentityModel.Tokens.Jwt;
using Velochat.Backend.App.Layers.DTOs;
using Velochat.Backend.App.Layers.Models;

namespace Velochat.Backend.App.Layers.Domains.Identity;

public interface IIdentityOrchestration
{
    Task<(CompleteIdentity Identity, EncodedTokenPair EncodedTokenPair)> RegisterAsync(Credentials credentials);

    Task<EncodedTokenPair> LogInAsync(Credentials credentials);

    Task<EncodedTokenPair> RefreshTokenAsync(string refreshTokenString);

    Task LogOutAsync(string refreshTokenString);

}
