using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Velochat.Backend.App.Infrastructure.DTOs;
using Velochat.Backend.App.Shared.Options;

namespace Velochat.Backend.App.Infrastructure.Services;

public class AuthTokenService : IAuthTokenService
{
    private readonly JwtOptions _jwtOptions;
    private readonly JwtSecurityTokenHandler _accessTokenHandler = new();
    private readonly JwtSecurityTokenHandler _refreshTokenHandler = new();

    public AuthTokenService(IOptions<JwtOptions> jwtOptions)
    {
        _jwtOptions = jwtOptions.Value;
        _accessTokenHandler.TokenLifetimeInMinutes = (int)_jwtOptions.AccessTokenLifetimeMinutes;
        _refreshTokenHandler.TokenLifetimeInMinutes = (int)(_jwtOptions.RefreshTokenLifetimeHours * 60);
    }

    public TokenPair GenerateTokenPair(int userId)
    {
        var securityTokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity([new Claim("sub", userId.ToString())]),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Secret)), 
                SecurityAlgorithms.HmacSha256Signature
            )
        };

        return new TokenPair
        {
            AccessToken = _accessTokenHandler.CreateJwtSecurityToken(securityTokenDescriptor),
            RefreshToken = _refreshTokenHandler.CreateJwtSecurityToken(securityTokenDescriptor)
        };
    }

    public EncodedTokenPair EncodeTokenPair(TokenPair tokenPair) => new() 
    {
        AccessToken = EncodeAccessToken(tokenPair.AccessToken),
        RefreshToken = EncodeRefreshToken(tokenPair.RefreshToken)
    };

    public async Task<JwtSecurityToken> ParseAccessTokenAsync(string tokenString)
    {
        return await ParseToken(_accessTokenHandler, tokenString);
    }

    public async Task<JwtSecurityToken> ParseRefreshTokenAsync(string tokenString) 
    {
        return await ParseToken(_refreshTokenHandler,tokenString);
    }

    private string EncodeAccessToken(JwtSecurityToken token)
    {
        return _accessTokenHandler.WriteToken(token);
    }

    private string EncodeRefreshToken(JwtSecurityToken token)
    {
        return _accessTokenHandler.WriteToken(token);
    }


    private static async Task<JwtSecurityToken> ParseToken(JwtSecurityTokenHandler handler, string tokenString)
    {
        var validation = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(new string('x', 128))),
        };
        
        var validationResult = await handler.ValidateTokenAsync(tokenString, validation);
        if (!validationResult.IsValid) throw validationResult.Exception;
        Console.WriteLine("Claims:");
        foreach (var key in validationResult.Claims.Keys) Console.WriteLine($"{key}: {validationResult.Claims[key]}");
        _ = validationResult.ClaimsIdentity.FindFirst(c => c.Type == ClaimTypes.NameIdentifier);
        return (JwtSecurityToken) validationResult.SecurityToken;
    }
}
