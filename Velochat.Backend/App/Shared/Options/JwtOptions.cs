using System.ComponentModel.DataAnnotations;

namespace Velochat.Backend.App.Shared.Options;

public class JwtOptions
{
    [Required]
    public required string Secret { get; set; }

    [Required]
    [Range(1, double.MaxValue)]
    public required double AccessTokenLifetimeMinutes { get; set; }

    [Required]
    [Range(1, double.MaxValue)]
    public required double RefreshTokenLifetimeHours { get; set; }
}