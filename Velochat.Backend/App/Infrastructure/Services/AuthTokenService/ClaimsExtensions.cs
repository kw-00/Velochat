using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Velochat.Backend.App.Shared.Exceptions;

namespace Velochat.Backend.App.Infrastructure.Services;

public static class ClaimsExtensions
{
    public static int GetUserId(this ClaimsPrincipal claimsPrincipal)
    {
        return claimsPrincipal.Claims.GetUserId();
    }

    public static int GetUserId(this JwtSecurityToken token)
    {
        return token.Claims.GetUserId();
    }

    public static int GetUserId(this IEnumerable<Claim> claims)
    {
        
        var userIdString 
            = claims
                .First(c => c.Type == ClaimTypes.NameIdentifier).Value
                ?? throw new UnauthorizedException(
                    "User identifier (sub) is missing."
                );

        var userIdIsInteger = int.TryParse(userIdString, out var userId);
        if (!userIdIsInteger) throw new UnauthorizedException(
            "User identifier (sub) is not an integer."
        );
        return userId;
        
    }
}