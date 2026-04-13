using System.IdentityModel.Tokens.Jwt;
using Velochat.Backend.App.Layers.DTOs;

namespace Velochat.Backend.App.Layers.Infrastructure;

public interface IAuthTokenService
{
    TokenPair GenerateTokenPair(int identityId);
    Task<JwtSecurityToken> ParseAccessTokenAsync(string tokenString);
    Task<JwtSecurityToken> ParseRefreshTokenAsync(string tokenString);

    string EncodeAccessToken(JwtSecurityToken token);
    string EncodeRefreshToken(JwtSecurityToken token);
}
