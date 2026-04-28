using Velochat.Backend.App.Infrastructure.DTOs;

namespace Velochat.Backend.App.API.Auth;

public static class TokenCookies
{
    public static void SetTokens(HttpResponse response, EncodedTokenPair tokenPair)
    {
        response.Cookies.Append("accessToken", tokenPair.AccessToken, CreateJwtCookieOptions());
        response.Cookies.Append("refreshToken", tokenPair.RefreshToken, CreateJwtCookieOptions());
    }

    private static CookieOptions CreateJwtCookieOptions() => new()
    {
        HttpOnly = true,
        Secure = true,
        SameSite = SameSiteMode.Strict,
        Path = "/"
    };
}