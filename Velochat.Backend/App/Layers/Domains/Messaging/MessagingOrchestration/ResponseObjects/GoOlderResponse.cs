using Velochat.Backend.App.Layers.Models;

namespace Velochat.Backend.App.Layers.Domains.Messaging;

public class GoOlderResponse
{
    public required IReadOnlyList<CompleteChatMessage> Messages { get; set; }
    public required bool TopReached { get; set; }
}