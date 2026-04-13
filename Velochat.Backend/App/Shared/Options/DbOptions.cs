using System.ComponentModel.DataAnnotations;

namespace Velochat.Backend.App.Shared.Options;

public class DbOptions
{
    [Required]
    public required string ConnectionString { get; set; }
}