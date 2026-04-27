using Velochat.Backend.App.Infrastructure.DTOs;
using Velochat.Backend.App.Infrastructure.Persistence;
using Velochat.Backend.App.Infrastructure.Services;
using Velochat.Backend.App.Infrastructure.Models;
using Velochat.Backend.App.Shared.Exceptions;

namespace Velochat.Backend.App.API.Domains.Identity;

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
        var user = new User
        {
            Login = credentials.Login,
            PasswordHash = passwordHash
        };

        var completeUser = await userRepository.CreateAsync(user);

        var tokenPair = await GetTokenPairAsync(completeUser.Id);
        return (completeUser, tokenPair);
    }

    public async Task<(
        CompleteUser User, 
        EncodedTokenPair EncodedTokenPair
    )> LogInAsync(Credentials credentials)
    {
        var userWithPasswordhash = await userRepository
            .GetWithPasswordHashAsync(credentials.Login) 
            ?? throw new UnauthorizedException("Wrong login or password.");

        if (!passwordService.Verify(credentials.Password, userWithPasswordhash.PasswordHash))
            throw new UnauthorizedException("Wrong login or password.");
        

        var tokenPair = await GetTokenPairAsync(userWithPasswordhash.Id);
        var user = new CompleteUser
        {
            Id = userWithPasswordhash.Id,
            Login = userWithPasswordhash.Login,
        };
        return (user, tokenPair);
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