using System.Security.Claims;
using Velochat.Backend.App.Layers.Domains;
using Velochat.Backend.App.Shared.Exceptions;

namespace Velochat.Backend.App.Shared;

public class HttpContextWrapper(HttpContext httpContext) : IHTTPContextWrapper
{
    public HttpContext HttpContext { get; set; } = httpContext;

    public int ClientUser { get
        {
            var userIdString 
                = HttpContext
                    .User
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
}