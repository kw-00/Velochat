using Velochat.Backend.App.Infrastructure.Models;

namespace Velochat.Backend.App.API.Domains.Messaging;

public class SubscribeFeedResponse
{
    public required IReadOnlyList<CompleteChatMessage> Messages { get; set; }
    public required bool IsContinuity { get; set; }
}