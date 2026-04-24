using System.Diagnostics.CodeAnalysis;

namespace Velochat.Backend.App.Infrastructure.Models;

public class RoomPresence : IMalleableModel
{
    [PrimaryKey]
    public int? RoomId { get; set; }
    [PrimaryKey]
    public int? UserId { get; set; }

    [MemberNotNull(nameof(RoomId), nameof(UserId))]
    public void EnsureInsertable()
    {
        if (RoomId is null || UserId is null) throw new ModelNotInsertableException();
    }

    [MemberNotNull(nameof(RoomId), nameof(UserId))]
    public void EnsureIdentifiable()
    {
        if (RoomId is null || UserId is null) throw new ModelNotIdentifiableException();
    }
}
