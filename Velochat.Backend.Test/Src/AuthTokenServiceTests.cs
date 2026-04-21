using System.UserModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.UserModel.Tokens;
using Velochat.Backend.App.Layers.DTOs;
using Velochat.Backend.App.Layers.Infrastructure;
using Velochat.Backend.App.Shared.Options;

namespace Velochat.Backend.Test.Src;

[TestClass]
public class AuthTokenServiceTests
{
    [TestMethod]
    public async Task TokenParsing_MissingSubject_Throws()
    {
        var options = CreatePlaceholderOptions();
        var service = new AuthTokenService(Options.Create(options));

        var tokenPairNoSub = new TokenPair 
        { 
            AccessToken = CreateAccessToken(null, options), 
            RefreshToken = CreateRefreshToken(null, options) 
        };

        var encodedTokenPair = service.EncodeTokenPair(tokenPairNoSub);

        await Assert.ThrowsAsync<Exception>(
            async () => await service.ParseAccessTokenAsync(encodedTokenPair.AccessToken)
        );
        await Assert.ThrowsAsync<Exception>(
            async () => await service.ParseRefreshTokenAsync(encodedTokenPair.RefreshToken)
        );
    }

    [TestMethod]
    public async Task TokenParsing_OwnTokensNotExpired_Accepts()
    {
        var options = new JwtOptions
        {
            Secret = new string('x', 128),
            AccessTokenLifetimeMinutes = double.MaxValue,
            RefreshTokenLifetimeHours = double.MaxValue
        };

        var service = new AuthTokenService(Options.Create(options));
        
        var tokenPair = service.GenerateTokenPair(1);
        var encodedTokenPair = service.EncodeTokenPair(tokenPair);
        await service.ParseAccessTokenAsync(encodedTokenPair.AccessToken);
        await service.ParseRefreshTokenAsync(encodedTokenPair.RefreshToken);
    }
        
    private static JwtOptions  CreatePlaceholderOptions() => new()
    {
        Secret = new string('x', 128),
        AccessTokenLifetimeMinutes = 1,
        RefreshTokenLifetimeHours = 1
    };

    private static JwtSecurityToken CreateAccessToken(int? sub, JwtOptions options)
        => CreateToken(sub, (int)options.AccessTokenLifetimeMinutes, options);

    private static JwtSecurityToken CreateRefreshToken(int? sub, JwtOptions options)
        => CreateToken(sub, (int)options.RefreshTokenLifetimeHours * 60, options);

    private static JwtSecurityToken CreateToken(int? sub, int lifetimeMinutes, JwtOptions options)
    {
        Claim[] claims = sub is not null ? [new Claim("sub", ((int)sub).ToString())] : [];
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.Secret));
        var accessToken = new JwtSecurityToken(
            claims:claims, 
            expires: DateTime.Now.AddMinutes(lifetimeMinutes),
            signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature)
        );
        return accessToken;
    }


}