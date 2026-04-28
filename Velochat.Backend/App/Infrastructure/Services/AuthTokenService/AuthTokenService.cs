using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Velochat.Backend.App.Infrastructure.DTOs;
using Velochat.Backend.App.Infrastructure.Models;
using Velochat.Backend.App.Infrastructure.Persistence;
using Velochat.Backend.App.Shared.Exceptions;
using Velochat.Backend.App.Shared.Options;

namespace Velochat.Backend.App.Infrastructure.Services;

public class AuthTokenService : IAuthTokenService
{
    private readonly JwtOptions _jwtOptions;
    private readonly JwtSecurityTokenHandler _accessTokenHandler = new();
    private readonly JwtSecurityTokenHandler _refreshTokenHandler = new();

    private readonly IRefreshTokenStateRepository _refreshTokenStateRepository;

    public AuthTokenService(IOptions<JwtOptions> jwtOptions, IRefreshTokenStateRepository refreshTokenStateRepository)
    {
        _jwtOptions = jwtOptions.Value;
        _accessTokenHandler.TokenLifetimeInMinutes = (int)_jwtOptions.AccessTokenLifetimeMinutes;
        _refreshTokenHandler.TokenLifetimeInMinutes = (int)(_jwtOptions.RefreshTokenLifetimeHours * 60);
        _refreshTokenStateRepository = refreshTokenStateRepository;
    }

    public async Task<EncodedTokenPair> GenerateTokenPairAsync(int userId)
    {
        var securityTokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity([new Claim("sub", userId.ToString())]),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Secret)), 
                SecurityAlgorithms.HmacSha256Signature
            )
        };

        var tokenPair = new TokenPair
        {
            AccessToken = _accessTokenHandler.CreateJwtSecurityToken(securityTokenDescriptor),
            RefreshToken = _refreshTokenHandler.CreateJwtSecurityToken(securityTokenDescriptor)
        };
        var encodedPair = EncodeTokenPair(tokenPair);

        try
        {
            await _refreshTokenStateRepository.CreateAsync(new Models.RefreshTokenState
            {
                Token = encodedPair.RefreshToken,
                UserId = userId
            });
            return encodedPair;
        }
        catch (IdentifierNotFoundException<User> ex)
        {
            throw new NotFoundException(ex.Message);
        }
    }

    public async Task<JwtSecurityToken> VerifyAccessTokenAsync(string tokenString)
    {
        return await ParseTokenAsync(_accessTokenHandler, tokenString);
    }

    public async Task<JwtSecurityToken> VerifyRefreshTokenAsync(string tokenString) 
    {
        var decodedToken = await ParseTokenAsync(_refreshTokenHandler, tokenString);
        var userId = decodedToken.GetUserId();

        var refreshTokenState = await _refreshTokenStateRepository.GetByTokenAsync(tokenString)
            ?? throw new UnauthorizedException("Invalid refresh token.");
        if (refreshTokenState.Status == RefreshTokenState.Revoked) 
            throw new UnauthorizedException("Refresh token has been revoked.");
        if (refreshTokenState.Status == RefreshTokenState.Used)
        {
            await _refreshTokenStateRepository.RevokeByUserIdAsync(userId);
            throw new UnauthorizedException(
                "Refresh token has been used. Revoking all tokens for user."
            );
        }
        if (refreshTokenState.Status == RefreshTokenState.Active) return decodedToken;
        throw new UnauthorizedException("Refresh token status is not valid.");
    }

    public async Task RevokeRefreshTokenAsync(string tokenString)
    {
        await _refreshTokenStateRepository.RevokeAsync(tokenString);
    }

    private EncodedTokenPair EncodeTokenPair(TokenPair tokenPair) => new() 
    {
        AccessToken = EncodeAccessToken(tokenPair.AccessToken),
        RefreshToken = EncodeRefreshToken(tokenPair.RefreshToken)
    };

    private string EncodeAccessToken(JwtSecurityToken token)
    {
        return _accessTokenHandler.WriteToken(token);
    }

    private string EncodeRefreshToken(JwtSecurityToken token)
    {
        return _accessTokenHandler.WriteToken(token);
    }


    private async Task<JwtSecurityToken> ParseTokenAsync(JwtSecurityTokenHandler handler, string tokenString)
    {
        var validation = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Secret)),
        };
        
        var validationResult = await handler.ValidateTokenAsync(tokenString, validation);
        if (!validationResult.IsValid) throw validationResult.Exception;
        return (JwtSecurityToken) validationResult.SecurityToken;
    }
}
