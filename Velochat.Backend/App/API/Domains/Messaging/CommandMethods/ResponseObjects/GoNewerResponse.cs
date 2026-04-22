using Velochat.Backend.App.Infrastructure.Models;

namespace Velochat.Backend.App.API.Domains.Messaging;

public class GoNewerResponse
{
    public required IReadOnlyList<CompleteChatMessage> Messages { get; set; }
    public required bool BottomReached { get; set; }
}