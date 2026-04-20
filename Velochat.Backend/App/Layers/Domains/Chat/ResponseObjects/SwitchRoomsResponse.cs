using Velochat.Backend.App.Layers.Models;

namespace Velochat.Backend.App.Layers.Domains.Chat;

public class SwitchRoomsResponse
{
    public required IReadOnlyList<CompleteChatMessage> Messages { get; set; }
    public required bool IsContinuity { get; set; }
}