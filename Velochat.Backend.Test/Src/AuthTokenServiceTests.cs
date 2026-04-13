using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Shared.Options;
using Velochat.Backend.App.Layers.Infrastructure;

namespace Velochat.Backend.Test.Src;

[TestClass]
public class AuthTokenServiceTests
{
    [TestMethod]
    public async Task TokenParsing_MissingSubject_Throws()
    {
        var options = CreatePlaceholderOptions();
        var service = new AuthTokenService(options);

        var accessTokenNoSub = service.EncodeAccessToken(CreateAccessToken(null, options));
        var refreshTokenNoSub = service.EncodeRefreshToken(CreateRefreshToken(null, options));

        await Assert.ThrowsAsync<Exception>(async () => await service.ParseAccessTokenAsync(accessTokenNoSub));
        await Assert.ThrowsAsync<Exception>(async () => await service.ParseRefreshTokenAsync(refreshTokenNoSub));
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

        var service = new AuthTokenService(options);
        
        var tokenPair = service.GenerateTokenPair(1);

        await service.ParseAccessTokenAsync(service.EncodeAccessToken(tokenPair.AccessToken));
        await service.ParseRefreshTokenAsync(service.EncodeRefreshToken(tokenPair.RefreshToken));
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