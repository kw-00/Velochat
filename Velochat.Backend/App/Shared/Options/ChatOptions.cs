using System.ComponentModel.DataAnnotations;

namespace Velochat.Backend.App.Shared.Options;

public class ChatOptions
{
    [Required]
    public required int MessageBatchSize { get; set; }
}