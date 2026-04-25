using System.Security.Claims;
using Velochat.Backend.App.Shared.Exceptions;

namespace Velochat.Backend.App.API.Auth;

public static class IdentityInfoExtraction
{
    public static int GetUserId(this ClaimsPrincipal claimsPrincipal)
    {
        
        var userIdString 
            = claimsPrincipal
                .Claims
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