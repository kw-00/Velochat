using System.Diagnostics.CodeAnalysis;

namespace Velochat.Backend.App.Layers.Models;

public class Invitation : IMalleableModel
{
    [PrimaryKey]
    public int? RoomId { get; set; }
    [PrimaryKey]
    public int? InviteeId { get; set; }

    [MemberNotNull(nameof(RoomId), nameof(InviteeId))]
    public void EnsureInsertable()
    {
        if (RoomId is null || InviteeId is null) throw new ModelNotInsertableException();
    }

    [MemberNotNull(nameof(RoomId), nameof(InviteeId))]
    public void EnsureIdentifiable()
    {
        if (RoomId is null || InviteeId is null) throw new ModelNotIdentifiableException();
    }
}
