using System.Diagnostics.CodeAnalysis;

namespace Velochat.Backend.App.Layers.Models;

public class RoomPresence : IMalleableModel
{
    [PrimaryKey]
    public int? RoomId { get; set; }
    [PrimaryKey]
    public int? IdentityId { get; set; }

    [MemberNotNull(nameof(RoomId), nameof(IdentityId))]
    public void EnsureInsertable()
    {
        if (RoomId is null || IdentityId is null) throw new ModelNotInsertableException();
    }

    [MemberNotNull(nameof(RoomId), nameof(IdentityId))]
    public void EnsureIdentifiable()
    {
        if (RoomId is null || IdentityId is null) throw new ModelNotIdentifiableException();
    }
}
