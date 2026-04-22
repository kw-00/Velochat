using System.IdentityModel.Tokens.Jwt;
using Velochat.Backend.App.Infrastructure.DTOs;

namespace Velochat.Backend.App.Infrastructure.Services;

public interface IAuthTokenService
{
    TokenPair GenerateTokenPair(int userId);

    EncodedTokenPair EncodeTokenPair(TokenPair tokenPair);

    Task<JwtSecurityToken> ParseAccessTokenAsync(string tokenString);
    Task<JwtSecurityToken> ParseRefreshTokenAsync(string tokenString);
}
