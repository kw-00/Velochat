using System.Diagnostics.CodeAnalysis;

namespace Velochat.Backend.App.Layers.Models;

public class RoomPresence : IModel
{
    public int? RoomId { get; set; }
    public int? IdentityId { get; set; }

    [MemberNotNull(nameof(RoomId), nameof(IdentityId))]
    public void EnsureInsertable()
    {
        if (RoomId is null || IdentityId is null) throw new ModelNotInsertableException();
    }
}
