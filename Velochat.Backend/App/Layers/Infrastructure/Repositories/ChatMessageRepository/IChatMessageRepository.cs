using Velochat.Backend.App.Layers.Models;

namespace Velochat.Backend.App.Layers.Infrastructure;

public interface IChatMessageRepository
{
    Task<CompleteChatMessage> CreateAsync(CompleteChatMessage message);

    Task<IReadOnlyList<CompleteChatMessage>> GetAllByRoomIdAsync(int roomId);

    Task DeleteAllByRoomIdAsync(int roomId);
}