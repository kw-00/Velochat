using Microsoft.AspNetCore.Mvc;

using Velochat.Backend.App.Infrastructure.DTOs;
using Velochat.Backend.App.Shared.Exceptions;


namespace Velochat.Backend.App.API.Domains.Identity;

[ApiController]
[Route("[controller]")]
public class IdentityController(IUserOrchestration userOrchestration) : ControllerBase
{
    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> RegisterAsync([FromBody] Credentials credentials)
    {
        var result = await userOrchestration.RegisterAsync(credentials);

        SetJwtCookies(result.EncodedTokenPair);
        return Ok(result.User);
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> LogInAsync([FromBody] Credentials credentials)
    {
        var result = await userOrchestration.LogInAsync(credentials);
        
        SetJwtCookies(result.EncodedTokenPair);
        return Ok(result.User);
    }

    [HttpGet]
    [Route("refresh-token")]
    public async Task<IActionResult> RefreshTokenAsync()
    {
        var refreshToken = Request.Cookies["refreshToken"] 
            ?? throw new UnauthorizedException("Refresh token not found.");

        var tokenPair = await userOrchestration.RefreshTokenAsync(refreshToken);
        SetJwtCookies(tokenPair);
        return Ok();
    }

    [HttpGet]
    [Route("log-out")]
    public async Task<IActionResult> LogOutAsync()
    {
        var refreshToken = Request.Cookies["refreshToken"] 
            ?? throw new UnauthorizedException("Refresh token not found.");
        await userOrchestration.LogOutAsync(refreshToken);
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