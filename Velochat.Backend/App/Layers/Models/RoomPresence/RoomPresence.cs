using System.Diagnostics.CodeAnalysis;

namespace Velochat.Backend.App.Layers.Models;

public class RoomPresence : IModel
{
    public int? RoomId { get; set; }
    public int? IdentityId { get; set; }

    [MemberNotNullWhen(true, nameof(RoomId), nameof(IdentityId))]
    public bool CanBePeristed() => RoomId is not null && IdentityId is not null;
}
