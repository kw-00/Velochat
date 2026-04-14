namespace Velochat.Backend.App.Layers.Models;

public static class InvitationModelConveter
{
    public static CompleteInvitation ToCompleteModel(this Invitation invitation) => new()
    {
        RoomId = invitation.RoomId ?? throw new ModelNotCompleteException(),
        InviteeId = invitation.InviteeId ?? throw new ModelNotCompleteException()
    };

    public static Invitation ToModel(this CompleteInvitation invitation) => new()
    {
        RoomId = invitation.RoomId,
        InviteeId = invitation.InviteeId
    };
}