using System.Diagnostics.CodeAnalysis;

namespace Velochat.Backend.App.Layers.Models;

public class Invitation : IModel
{
    public int? RoomId { get; set; }
    public int? InviteeId { get; set; }

    [MemberNotNull(nameof(RoomId), nameof(InviteeId))]
    public void EnsureInsertable()
    {
        if (RoomId is null || InviteeId is null) throw new ModelNotInsertableException();
    }
}
