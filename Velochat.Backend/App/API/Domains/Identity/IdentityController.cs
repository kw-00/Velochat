using Microsoft.AspNetCore.Mvc;

using Velochat.Backend.App.Infrastructure.DTOs;
using Velochat.Backend.App.Shared.Exceptions;


namespace Velochat.Backend.App.API.Domains.Identity;

[ApiController]
[Route("[controller]")]
public class IdentityController(IIdentityOrchestration identityOrchestration) : ControllerBase
{
    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> RegisterAsync([FromBody] Credentials credentials)
    {
        var result = await identityOrchestration.RegisterAsync(credentials);

        SetJwtCookies(result.TokenPair);
        return Ok(result.User);
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> LogInAsync([FromBody] Credentials credentials)
    {
        var result = await identityOrchestration.LogInAsync(credentials);
        
        SetJwtCookies(result.TokenPair);
        return Ok(result.User);
    }

    [HttpGet]
    [Route("refresh-session")]
    public async Task<IActionResult> RefreshSessionAsync()
    {
        var refreshToken = Request.Cookies["refreshToken"] 
            ?? throw new UnauthorizedException("Refresh token not found.");
        var result = await identityOrchestration.RefreshSessionAsync(refreshToken);
        SetJwtCookies(result.TokenPair);
        return Ok(result.User);
    }

    [HttpGet]
    [Route("log-out")]
    public async Task<IActionResult> LogOutAsync()
    {
        var refreshToken = Request.Cookies["refreshToken"] 
            ?? throw new UnauthorizedException("Refresh token not found.");
        await identityOrchestration.LogOutAsync(refreshToken);
        return Ok();
    }

    private void SetJwtCookies(EncodedTokenPair tokenPair)
    {
        var options = CreateJwtCookieOptions();
        Response.Cookies.Append("accessToken", tokenPair.AccessToken, options);
        Response.Cookies.Append("refreshToken", tokenPair.RefreshToken, options);
    }

    private static CookieOptions CreateJwtCookieOptions() => new()
    {
        HttpOnly = true,
        Secure = true,
        SameSite = SameSiteMode.None
    };
}