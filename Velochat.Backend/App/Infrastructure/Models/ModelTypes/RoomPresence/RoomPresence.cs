using System.Diagnostics.CodeAnalysis;

namespace Velochat.Backend.App.Infrastructure.Models;

public class RoomPresence : IMalleableModel
{
    [PrimaryKey]
    public int? RoomId { get; set; }
    [PrimaryKey]
    public int? MemberId { get; set; }

    [MemberNotNull(nameof(RoomId), nameof(MemberId))]
    public void EnsureInsertable()
    {
        if (RoomId is null || MemberId is null) throw new ModelNotInsertableException();
    }

    [MemberNotNull(nameof(RoomId), nameof(MemberId))]
    public void EnsureIdentifiable()
    {
        if (RoomId is null || MemberId is null) throw new ModelNotIdentifiableException();
    }
}
