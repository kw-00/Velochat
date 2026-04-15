using Velochat.Backend.App.Layers.DTOs;
using Velochat.Backend.App.Layers.Infrastructure;
using Velochat.Backend.App.Layers.Models;

namespace Velochat.Backend.App.Layers.Domains.Identity;

public class IdentityOrchestration(
    IIdentityRepository identityRepository,
    IRefreshTokenStateRepository refreshTokenStateRepository,
    IPasswordService passwordService,
    IAuthTokenService authTokenService
) : IIdentityOrchestration
{

    public async Task<(CompleteIdentity Identity, EncodedTokenPair EncodedTokenPair)> RegisterAsync(Credentials credentials)
    {
        var passwordHash = passwordService.HashPassword(credentials.Password);
        var identity = new Models.Identity
        {
            Login = credentials.Login,
            PasswordHash = passwordHash
        };

        var CompleteIdentity = await identityRepository.CreateAsync(identity);

        var tokenPair = await GetTokenPairAsync(CompleteIdentity.Id);
        return (CompleteIdentity, tokenPair);
    }

    public async Task<EncodedTokenPair> LogInAsync(Credentials credentials)
    {
        var hashedPassword = passwordService.HashPassword(credentials.Password);
        var matchedIdentity = await identityRepository.GetByCredentialsAsync(credentials.Login, hashedPassword) 
            ?? throw new UnauthorizedException("Login and password do not match any identity.");

        var tokenPair = await GetTokenPairAsync(matchedIdentity.Id);
        return tokenPair;
    }


    public async Task<EncodedTokenPair> RefreshTokenAsync(string refreshTokenString)
    {
        var identityId = await CheckAndHandleRefreshTokenStatus(refreshTokenString);
        return await GetTokenPairAsync(identityId);
    }

    public async Task LogOutAsync(string refreshTokenString)
    {
        await CheckAndHandleRefreshTokenStatus(refreshTokenString);
        await refreshTokenStateRepository.RevokeAsync(refreshTokenString);
    }

    private async Task<EncodedTokenPair> GetTokenPairAsync(int identityId)
    {
        var tokens = authTokenService.GenerateTokenPair(identityId);
        return authTokenService.EncodeTokenPair(tokens);
    }

    private async Task<int> CheckAndHandleRefreshTokenStatus(string refreshTokenString)
    {
        try {
            var refreshToken = await authTokenService.ParseRefreshTokenAsync(refreshTokenString);
            var identityId = int.Parse(refreshToken.Subject);
            var refreshTokenState = await refreshTokenStateRepository.GetByTokenAsync(refreshTokenString)
                ?? throw new UnauthorizedException("Invalid refresh token.");

            if (refreshTokenState.Status == RefreshTokenState.Revoked) throw new UnauthorizedException("Refresh token has been revoked.");
            if (refreshTokenState.Status == RefreshTokenState.Used)
            {
            await refreshTokenStateRepository.RevokeByIdentityIdAsync(identityId);
            throw new UnauthorizedException("Refresh token has been used. Revoking all tokens for identity.");
            }
            if (refreshTokenState.Status == RefreshTokenState.Active) return identityId;
            throw new UnauthorizedException("Refresh token status is not valid.");
        }
        catch (Exception ex)
        {
            throw new UnauthorizedException($"Invalid refresh token. {ex.Message}");
        }
    }
}