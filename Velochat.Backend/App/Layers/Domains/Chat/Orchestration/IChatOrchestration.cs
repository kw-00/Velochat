using Velochat.Backend.App.Layers.Models;

namespace Velochat.Backend.App.Layers.Domains.Chat;

public interface IChatOrchestration
{
    Task<PersistedRoom> CreateRoomAsync(int userId, string name);
}

   