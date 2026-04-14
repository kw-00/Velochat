using Velochat.Backend.App.Layers.Models;

namespace Velochat.Backend.App.Layers.Infrastructure;

public interface IRoomRepository
{
    Task<CompleteRoom> CreateAsync(Room room);

    Task DeleteAsync(int roomId);

    Task<IReadOnlyList<CompleteRoom>> GetAllByMemberIdAsync(int identityId);

    Task<IReadOnlyList<CompleteRoom>> GetAllByInviteeIdAsync(int identityId);
}