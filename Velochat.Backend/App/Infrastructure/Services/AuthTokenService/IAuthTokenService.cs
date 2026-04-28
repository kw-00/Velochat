using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Velochat.Backend.App.Infrastructure.DTOs;
using Velochat.Backend.App.Shared.Exceptions;

namespace Velochat.Backend.App.Infrastructure.Services;

public interface IAuthTokenService
{
    Task<EncodedTokenPair> GenerateTokenPairAsync(int userId);

    Task<JwtSecurityToken> VerifyAccessTokenAsync(string tokenString);
    Task<JwtSecurityToken> VerifyRefreshTokenAsync(string tokenString);

    Task RevokeRefreshTokenAsync(string tokenString);
}
