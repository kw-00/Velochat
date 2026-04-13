using System.IdentityModel.Tokens.Jwt;

namespace Velochat.Backend.App.Layers.DTOs;

public class TokenPair
{
    public required JwtSecurityToken AccessToken { get; set; }
    public required JwtSecurityToken RefreshToken { get; set; }
}

