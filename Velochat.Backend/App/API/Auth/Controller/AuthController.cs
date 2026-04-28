using Microsoft.AspNetCore.Mvc;

using Velochat.Backend.App.Infrastructure.DTOs;
using Velochat.Backend.App.Shared.Exceptions;


namespace Velochat.Backend.App.API.Auth;

[ApiController]
[Route("[controller]")]
public class AuthController(IAuthOrchestration identityOrchestration) : ControllerBase
{
    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> RegisterAsync([FromBody] Credentials credentials)
    {
        var result = await identityOrchestration.RegisterAsync(credentials);

        TokenCookies.SetTokens(Response, result.TokenPair);
        return Ok(result.User);
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> LogInAsync([FromBody] Credentials credentials)
    {
        var result = await identityOrchestration.LogInAsync(credentials);
        
        TokenCookies.SetTokens(Response, result.TokenPair);
        return Ok(result.User);
    }

    [HttpGet]
    [Route("refresh-session")]
    public async Task<IActionResult> RefreshSessionAsync()
    {
        var refreshToken = Request.Cookies["refreshToken"] 
            ?? throw new UnauthorizedException("Refresh token not found.");
        var result = await identityOrchestration.RefreshSessionAsync(refreshToken);
        TokenCookies.SetTokens(Response, result.TokenPair);
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
}