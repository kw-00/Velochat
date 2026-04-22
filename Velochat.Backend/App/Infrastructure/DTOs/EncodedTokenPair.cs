namespace Velochat.Backend.App.Infrastructure.DTOs;

public class EncodedTokenPair
{
    public required string AccessToken { get; set; }
    public required string RefreshToken { get; set; }
}