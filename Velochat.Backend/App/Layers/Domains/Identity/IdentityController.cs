using Microsoft.AspNetCore.Mvc;

using Velochat.Backend.App.Layers.DTOs;


namespace Velochat.Backend.App.Layers.Domains.Identity;

[ApiController]
[Route("[controller]")]
public class IdentityController(IIdentityOrchestration identityOrchestration) : ControllerBase
{
    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> RegisterAsync([FromBody] Credentials credentials)
    {
        var result = await identityOrchestration.RegisterAsync(credentials);
        return Ok(result);
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> LogInAsync([FromBody] Credentials credentials)
    {
        var result = await identityOrchestration.LogInAsync(credentials);
        return Ok(result);
    }

    [HttpGet]
    [Route("refresh-token")]
    public async Task<IActionResult> RefreshTokenAsync()
    {
        var refreshToken = Request.Cookies["refreshToken"] 
            ?? throw new UnauthorizedException("Refresh token not found.");

        var tokenPair = await identityOrchestration.RefreshTokenAsync(refreshToken);
        var options = CreateJwtCookieOptions();
        Response.Cookies.Append("accessToken", tokenPair.AccessToken, options);
        Response.Cookies.Append("refreshToken", tokenPair.RefreshToken, options);
        return Ok();
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

    private static CookieOptions CreateJwtCookieOptions() => new()
    {
        HttpOnly = true,
        Secure = true,
        SameSite = SameSiteMode.None
    };
}