using Velochat.Backend.App.Layers.Infrastructure;
using Velochat.Backend.App.Layers.Models;

namespace Velochat.Backend.App.Layers.Infrastructure;

public interface IInvitationRepository
{
    /// <summary>
    /// Gets an invitation.
    /// </summary>
    /// <param name="invitation">
    /// A melleable model of the invitation to be retrieved.
    /// </param>
    /// <returns>
    /// A complete model of the invitation
    /// or null if the invitation does not exist.
    /// </returns>
    /// <exception cref="ModelNotIdentifiableException">
    /// Thrown when the model is not identifiable.
    /// </exception>
    Task<CompleteInvitation?> GetAsync(Invitation invitation);


    /// <summary>
    /// Gets all invitations by identity ID.
    /// </summary>
    /// <param name="identityId"></param>
    /// <returns>
    /// A list of complete models of the invitations.
    /// </returns>
    /// <exception cref="RecordNotFoundException{Identity}">
    /// Thrown when identity with the given ID does not exist.
    /// </exception>
    Task<List<CompleteInvitation>> GetAsync(int identityId);

    /// <summary>
    /// Inserts a new invitation.
    /// </summary>
    /// <param name="invitation"></param>
    /// <returns>A complete model of the invitation.</returns>
    /// <exception cref="DuplicatePrimaryKeyException{Invitation}">
    /// Thrown when an invitation with the same primary key already exists.
    /// The primary key in this case is RoomId and InviteeId.
    /// </exception>
    /// <exception cref="RecordNotFoundException{Identity}">
    /// Thrown when identity (invitee) with the given ID does not exist.
    /// </exception>
    /// <exception cref="RecordNotFoundException{Room}">
    /// Thrown when room with the given ID does not exist.
    /// </exception>
    Task<CompleteInvitation> CreateAsync(Invitation invitation);

    /// <summary>
    /// Deletes an invitation.
    /// </summary>
    /// <param name="invitation">
    /// A melleable model of the initation to be deleted.
    /// </param>
    /// <returns></returns>
    /// <exception cref="ModelNotIdentifiableException">
    /// Thrown when the model is not identifiable.
    /// </exception>
    Task DeleteAsync(Invitation invitation);
}