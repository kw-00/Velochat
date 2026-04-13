
using Velochat.Backend.App.Exceptions;
using Velochat.Backend.App.Layers.DTOs;
using Velochat.Backend.App.Layers.Infrastructure;
using Velochat.Backend.App.Layers.Models;

namespace Velochat.Backend.App.Layers.Domains.Chat;

public class IdentityOrchestration(
    IIdentityRepository identityRepository,
    IRefreshTokenStateRepository refreshTokenStateRepository,
    IPasswordService passwordService,
    IAuthTokenService authTokenService
) : IIdentityOrchestration
{

    public async Task<(CompleteIdentity Identity, TokenPair TokenPair)> RegisterAsync(string login, string password)
    {
        var passwordHash = passwordService.HashPassword(password);
        var identity = new Identity
        {
            Login = login,
            PasswordHash = passwordHash
        };

        var CompleteIdentity = await identityRepository.CreateAsync(identity);

        var tokenPair = await GetTokenPairAsync(CompleteIdentity.Id);
        return (CompleteIdentity, tokenPair);
    }

    public async Task<TokenPair> LogInAsync(string login, string password)
    {
        var hashedPassword = passwordService.HashPassword(password);
        var matchedIdentity = await identityRepository.GetByCredentialsAsync(login, hashedPassword) 
            ?? throw new UnauthorizedException("Login and password do not match any identity.");

        var tokenPair = await GetTokenPairAsync(matchedIdentity.Id);
        return tokenPair;
    }


    public async Task<TokenPair> RefreshTokenAsync(string refreshTokenString)
    {
        var identityId = await CheckAndHandleRefreshTokenStatus(refreshTokenString);
        return await GetTokenPairAsync(identityId);
    }

    public async Task LogOutAsync(string refreshTokenString)
    {
        await CheckAndHandleRefreshTokenStatus(refreshTokenString);
        await refreshTokenStateRepository.RevokeAsync(refreshTokenString);
    }

    private async Task<TokenPair> GetTokenPairAsync(int identityId)
    {
        throw new NotImplementedException();
    }

    private async Task<int> CheckAndHandleRefreshTokenStatus(string refreshTokenString)
    {
        var refreshToken = await authTokenService.ParseRefreshTokenAsync(refreshTokenString);
        var identityId = int.Parse(refreshToken.Subject);
        var refreshTokenState = await refreshTokenStateRepository.GetByTokenAsync(refreshTokenString)
            ?? throw new UnauthorizedException("Invalid refresh token.");

        if (refreshTokenState.Status == RefreshTokenState.Revoked) throw new UnauthorizedException("Refresh token has been revoked.");
        if (refreshTokenState.Status == RefreshTokenState.Used)
        {
           await refreshTokenStateRepository.RevokeAllByIdentityIdAsync(identityId);
           throw new UnauthorizedException("Refresh token has been used. Revoking all tokens for identity.");
        }
        if (refreshTokenState.Status == RefreshTokenState.Active) return identityId;
        throw new UnauthorizedException("Refresh token status is not valid.");
    }
}