
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Velochat.Backend.App.Infrastructure.Services;

namespace Velochat.Backend.App.API.Auth;

public class AccessTokenAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private readonly IAuthTokenService _authTokenService;

    public AccessTokenAuthHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options, 
        ILoggerFactory logger, 
        UrlEncoder encoder,
        IAuthTokenService authTokenService

    ) : base(options, logger, encoder)
    {
        _authTokenService = authTokenService;
    }
    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        Context.Request.Cookies.TryGetValue("accessToken", out var accessTokenString);
        if (accessTokenString is null) return AuthenticateResult.NoResult();

        try
        { 
            JwtSecurityToken accessToken;
            try
            {
                accessToken = await _authTokenService.VerifyAccessTokenAsync(accessTokenString);
            } 
            catch (Exception e)
            {
                Context.Request.Cookies.TryGetValue("refreshToken", out var refreshTokenString);
                if (refreshTokenString is null) return AuthenticateResult.Fail(e);
                var refreshToken = await _authTokenService.VerifyRefreshTokenAsync(refreshTokenString);
                var userId = refreshToken.GetUserId();
                var tokenPair = await _authTokenService.GenerateTokenPairAsync(userId);
                accessToken = await _authTokenService.VerifyAccessTokenAsync(tokenPair.AccessToken);
            }
            var claimsPrincipal = new ClaimsPrincipal();
            claimsPrincipal.AddIdentity(
                new ClaimsIdentity(
                    accessToken.Claims, Scheme.Name
                )
            );
            var authenticationTicket = new AuthenticationTicket(claimsPrincipal, Scheme.Name);
            return AuthenticateResult.Success(authenticationTicket);
        }
        catch (Exception e)
        {
            return AuthenticateResult.Fail(e);
        }
    }
}

