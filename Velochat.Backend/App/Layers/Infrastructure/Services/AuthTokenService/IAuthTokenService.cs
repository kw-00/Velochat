using System.IdentityModel.Tokens.Jwt;
using Velochat.Backend.App.Layers.DTOs;

namespace Velochat.Backend.App.Layers.Infrastructure;

public interface IAuthTokenService
{
    TokenPair GenerateTokenPair(int identityId);

    EncodedTokenPair EncodeTokenPair(TokenPair tokenPair);

    Task<JwtSecurityToken> ParseAccessTokenAsync(string tokenString);
    Task<JwtSecurityToken> ParseRefreshTokenAsync(string tokenString);
}
