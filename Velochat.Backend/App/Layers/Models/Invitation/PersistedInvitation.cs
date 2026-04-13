namespace Velochat.Backend.App.Layers.Models;

public class PersistedInvitation : IPersistedModel<Invitation>
{
    public required int ChatroomId { get; init; }
    public required int InviteeId { get; init; }

    public Invitation ToModel() => new()
    {
        ChatroomId = ChatroomId,
        InviteeId = InviteeId
    };
}
