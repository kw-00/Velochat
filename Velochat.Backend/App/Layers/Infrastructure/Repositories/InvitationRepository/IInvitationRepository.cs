using Velochat.Backend.App.Layers.Models;

namespace Velochat.Backend.App.Layers.Infrastructure;

public interface IInvitationRepository
{
    Task<Invitation> CreateAsync(Invitation invitation);
    Task DeleteAsync(int invitationId);
}