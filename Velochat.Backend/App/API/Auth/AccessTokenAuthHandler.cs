
using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Velochat.Backend.App.Infrastructure.Repositories;

namespace Velochat.Backend.App.Shared.Auth;

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
        var accessTokenString = Context.Request.Cookies["accessToken"];
        if (accessTokenString is null) return AuthenticateResult.NoResult();

        try
        { 
            var accessToken = await _authTokenService.ParseAccessTokenAsync(accessTokenString);
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

