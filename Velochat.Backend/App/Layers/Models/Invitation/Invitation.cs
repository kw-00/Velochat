using System.Diagnostics.CodeAnalysis;

namespace Velochat.Backend.App.Layers.Models;

public class Invitation : IModel
{
    public int? ChatroomId { get; set; }
    public int? InviteeId { get; set; }

    [MemberNotNullWhen(true, nameof(ChatroomId), nameof(InviteeId))]
    public bool CanBePeristed() => ChatroomId is not null && InviteeId is not null;
}
