using System.Security.Claims;
using Velochat.Backend.App.Layers.Domains;

namespace Velochat.Backend.App.Shared;

public class HttpContextWrapper(HttpContext httpContext) : IHTTPContextWrapper
{
    public HttpContext HttpContext { get; set; } = httpContext;

    public int ClientIdentity { get
        {
            var identityIdString 
                = HttpContext
                    .User
                    .Claims
                    .First(c => c.Type == ClaimTypes.NameIdentifier).Value
                    ?? throw new UnauthorizedException(
                        "User identifier (sub) is missing."
                    );

            var identityIdIsInteger = int.TryParse(identityIdString, out var identityId);
            if (!identityIdIsInteger) throw new UnauthorizedException(
                "User identifier (sub) is not an integer."
            );
            return identityId;
        }
    }
}