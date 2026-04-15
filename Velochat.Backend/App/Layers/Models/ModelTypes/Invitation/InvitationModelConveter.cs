namespace Velochat.Backend.App.Layers.Models;

public static class InvitationModelConveter
{
    
    /// <summary>
    /// Converts <see cref="Invitation"/> to <see cref="CompleteInvitation"/>.
    /// Does not mutate the original object.
    /// </summary>
    /// <param name="invitation">The invitation to convert.</param>
    /// <returns>The conversion result.</returns>
    /// <exception cref="ModelNotCompleteException">
    /// Thrown when the RoomId or InviteeId of the invitation is null.
    /// </exception>
    public static CompleteInvitation ToCompleteModel(this Invitation invitation) => new()
    {
        RoomId = invitation.RoomId ?? throw new ModelNotCompleteException(),
        InviteeId = invitation.InviteeId ?? throw new ModelNotCompleteException()
    };

    /// <summary>
    /// Converts <see cref="CompleteInvitation"/> to <see cref="Invitation"/>.
    /// Does not mutate the original object.
    /// </summary>
    /// <param name="invitation">The invitation to convert.</param>
    /// <returns>The conversion result.</returns>
    public static Invitation ToModel(this CompleteInvitation invitation) => new()
    {
        RoomId = invitation.RoomId,
        InviteeId = invitation.InviteeId
    };
}