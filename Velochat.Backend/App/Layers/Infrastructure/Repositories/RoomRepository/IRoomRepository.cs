using Velochat.Backend.App.Layers.Models;

namespace Velochat.Backend.App.Layers.Infrastructure;

public interface IRoomRepository
{
    Task<CompleteRoom> CreateAsync(Room room);

    Task DeleteAsync(int roomId);

    Task<IReadOnlyList<CompleteRoom>> GetAllForMemberIdAsync(int identityId);

    Task<IReadOnlyList<CompleteRoom>> GetAllForInviteeIdAsync(int identityId);
}