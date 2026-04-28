using Velochat.Backend.App.Infrastructure.DTOs;
using Velochat.Backend.App.Infrastructure.Persistence;
using Velochat.Backend.App.Infrastructure.Services;
using Velochat.Backend.App.Infrastructure.Models;
using Velochat.Backend.App.Shared.Exceptions;

namespace Velochat.Backend.App.API.Auth;

public class AuthOrchestration(
    IUserRepository userRepository,
    IPasswordService passwordService,
    IAuthTokenService authTokenService
) : IAuthOrchestration
{

    public async Task<SessionInitData> RegisterAsync(Credentials credentials)
    {
        var passwordHash = passwordService.HashPassword(credentials.Password);
        var user = new User
        {
            Login = credentials.Login,
            PasswordHash = passwordHash
        };

        var completeUser = await userRepository.CreateAsync(user);

        var tokenPair = await authTokenService.GenerateTokenPairAsync(completeUser.Id);
        return new SessionInitData
        {
            User = completeUser,
            TokenPair = tokenPair
        };
    }

    public async Task<SessionInitData> LogInAsync(Credentials credentials)
    {
        var userWithPasswordhash = await userRepository
            .GetWithPasswordHashAsync(credentials.Login) 
            ?? throw new UnauthorizedException("Wrong login or password.");

        if (!passwordService.Verify(credentials.Password, userWithPasswordhash.PasswordHash))
            throw new UnauthorizedException("Wrong login or password.");
        

        var tokenPair = await authTokenService.GenerateTokenPairAsync(userWithPasswordhash.Id);
        var user = new CompleteUser
        {
            Id = userWithPasswordhash.Id,
            Login = userWithPasswordhash.Login,
        };
        return new SessionInitData
        {
            User = user,
            TokenPair = tokenPair
        };
    }

    public async Task<SessionInitData> RefreshSessionAsync(string refreshTokenString)
    {
        var refreshToken = await authTokenService.VerifyRefreshTokenAsync(refreshTokenString);
        var userId = refreshToken.GetUserId();
        var tokenPair = await authTokenService.GenerateTokenPairAsync(userId);
        var user = await userRepository.GetByIdAsync(userId)
            ?? throw new RaceConditionException(
                "User disappeared from database as token was being generated."
            );
        return new SessionInitData
        {
            User = user,
            TokenPair = tokenPair
        };
    }


    public async Task LogOutAsync(string refreshTokenString)
    {
        await authTokenService.VerifyRefreshTokenAsync(refreshTokenString);
        await authTokenService.RevokeRefreshTokenAsync(refreshTokenString);
    }
}