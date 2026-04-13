using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Shared.Options;
using Velochat.Backend.App.Layers.DTOs;

namespace Velochat.Backend.App.Layers.Infrastructure;

public class AuthTokenService : IAuthTokenService
{
    private readonly JwtOptions _jwtOptions;
    private readonly JwtSecurityTokenHandler _accessTokenHandler = new();
    private readonly JwtSecurityTokenHandler _refreshTokenHandler = new();

    public AuthTokenService(JwtOptions jwtOptions)
    {
        _jwtOptions = jwtOptions;
        _accessTokenHandler.TokenLifetimeInMinutes = (int)_jwtOptions.AccessTokenLifetimeMinutes;
        _refreshTokenHandler.TokenLifetimeInMinutes = (int)(_jwtOptions.RefreshTokenLifetimeHours * 60);

        _accessTokenHandler.MapInboundClaims = false;
        _refreshTokenHandler.MapInboundClaims = false;
    }

    public TokenPair GenerateTokenPair(int identityId)
    {
        var securityTokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity([new Claim("sub", identityId.ToString())]),
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

    public async Task<JwtSecurityToken> ParseAccessTokenAsync(string tokenString)
    {
        return await ParseToken(_accessTokenHandler, tokenString);
    }
    public async Task<JwtSecurityToken> ParseRefreshTokenAsync(string tokenString) {
        return await ParseToken(_refreshTokenHandler,tokenString);
    }

    public string EncodeAccessToken(JwtSecurityToken token)
    {
        return _accessTokenHandler.WriteToken(token);
    }

    public string EncodeRefreshToken(JwtSecurityToken token)
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
        _ = validationResult.Claims["sub"];
        return (JwtSecurityToken) validationResult.SecurityToken;
    }
}
