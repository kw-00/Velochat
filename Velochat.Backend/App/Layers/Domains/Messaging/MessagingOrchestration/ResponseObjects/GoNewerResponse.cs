using Velochat.Backend.App.Layers.Models;

namespace Velochat.Backend.App.Layers.Domains.Messaging;

public class GoNewerResponse
{
    public required IReadOnlyList<CompleteChatMessage> Messages { get; set; }
    public required bool BottomReached { get; set; }
}