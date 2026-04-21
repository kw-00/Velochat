using Velochat.Backend.App.Layers.DTOs;
using Velochat.Backend.App.Layers.Infrastructure;
using Velochat.Backend.App.Layers.Models;

namespace Velochat.Backend.App.Layers.Domains.User;

public class UserOrchestration(
    IUserRepository userRepository,
    IRefreshTokenStateRepository refreshTokenStateRepository,
    IPasswordService passwordService,
    IAuthTokenService authTokenService
) : IUserOrchestration
{

    public async Task<(
        CompleteUser User, 
        EncodedTokenPair EncodedTokenPair
    )> RegisterAsync(Credentials credentials)
    {
        var passwordHash = passwordService.HashPassword(credentials.Password);
        var user = new Models.User
        {
            Login = credentials.Login,
            PasswordHash = passwordHash
        };

        var CompleteUser = await userRepository.CreateAsync(user);

        var tokenPair = await GetTokenPairAsync(CompleteUser.Id);
        return (CompleteUser, tokenPair);
    }

    public async Task<(
        CompleteUser User, 
        EncodedTokenPair EncodedTokenPair
    )> LogInAsync(Credentials credentials)
    {
        var hashedPassword = passwordService.HashPassword(credentials.Password);
        var matchedUser = await userRepository
            .GetByCredentialsAsync(credentials.Login, hashedPassword) 
            ?? throw new UnauthorizedException(
                "Login and password do not match any user."
            );

        var tokenPair = await GetTokenPairAsync(matchedUser.Id);
        return (matchedUser, tokenPair);
    }


    public async Task<EncodedTokenPair> RefreshTokenAsync(string refreshTokenString)
    {
        var userId = await CheckAndHandleRefreshTokenStatus(refreshTokenString);
        return await GetTokenPairAsync(userId);
    }

    public async Task LogOutAsync(string refreshTokenString)
    {
        await CheckAndHandleRefreshTokenStatus(refreshTokenString);
        await refreshTokenStateRepository.RevokeAsync(refreshTokenString);
    }

    private async Task<EncodedTokenPair> GetTokenPairAsync(int userId)
    {
        var tokens = authTokenService.GenerateTokenPair(userId);
        return authTokenService.EncodeTokenPair(tokens);
    }

    private async Task<int> CheckAndHandleRefreshTokenStatus(string refreshTokenString)
    {
        try {
            var refreshToken = await authTokenService.ParseRefreshTokenAsync(
                refreshTokenString
            );
            var userId = int.Parse(refreshToken.Subject);
            var refreshTokenState = await refreshTokenStateRepository
                .GetByTokenAsync(refreshTokenString)
                ?? throw new UnauthorizedException("Invalid refresh token.");

            if (refreshTokenState.Status == RefreshTokenState.Revoked) 
                throw new UnauthorizedException("Refresh token has been revoked.");
            if (refreshTokenState.Status == RefreshTokenState.Used)
            {
                await refreshTokenStateRepository.RevokeByUserIdAsync(userId);
                throw new UnauthorizedException(
                    "Refresh token has been used. Revoking all tokens for user."
                );
            }
            if (refreshTokenState.Status == RefreshTokenState.Active) return userId;
            throw new UnauthorizedException("Refresh token status is not valid.");
        }
        catch (Exception ex)
        {
            throw new UnauthorizedException($"Invalid refresh token. {ex.Message}");
        }
    }
}