using Velochat.Backend.App.Layers.Models;

namespace Velochat.Backend.App.Layers.Infrastructure;

public interface IFriendshipRepository
{
    /// <summary>
    /// Retrieves a friend request between two users.
    /// </summary>
    /// <param name="firstUserId"></param>
    /// <param name="secondUserId"></param>
    /// <returns>
    /// The matching friend request
    /// or null if not found.
    /// </returns>
    Task<CompleteFriendship?> GetFriendshipAsync(int firstUserId, int secondUserId);

    /// <summary>
    /// Retrieves a list of users that have initiated 
    /// friendship with the given user but are yet
    /// to be accepted as friends.
    /// </summary>
    /// <param name="userId"></param>
    /// <returns>
    /// A list of matching users.
    /// </returns>
    Task<IReadOnlyList<CompleteUser>> GetPendingInitiatorsAsync(int userId);

    /// <summary>
    /// Retrieves a list of users that are friends with the given user.
    /// </summary>
    /// <param name="userId"></param>
    /// <returns>
    /// A list of matching users.
    /// </returns>
    Task<IReadOnlyList<CompleteUser>> GetFriendsAsync(int userId);

    /// <summary>
    /// Creates a pending (not accepted) friendship between
    /// initiator and receiver.
    /// </summary>
    /// <param name="initiatorId"></param>
    /// <param name="receiverId"></param>
    /// <returns>
    /// A complete model of the receiver user.
    /// </returns>
    /// <exception cref="IdentifierNotFoundException{User}">
    /// Thrown when the sender or receiver does not exist.
    /// </exception>
    /// <exception cref="EquivalentAlreadyExistsException{Friendship}"> 
    /// Thrown when a friendship between the initiator 
    /// and receiver already exists, whether accepted, or not.
    /// </exception>
    Task<CompleteUser> CreatePendingAsync(int initiatorId, int receiverId);


    /// <summary>
    /// Makes a friendship between two users accepted.
    /// </summary>
    /// <param name="friendship"></param>
    /// <returns>
    /// A complete model of the receiver user.
    /// </returns>
    /// <exception cref="ModelNotIdentifiableException">
    /// Thrown when the friend request is not identifiable.
    /// </exception>
    /// <exception cref="IdentifierNotFoundException{Friendship}">
    /// Thrown when the friend request does not exist.
    /// </exception>
    Task<CompleteUser> AcceptAsync(Friendship friendship);

    /// <summary>
    /// Deletes a friendship between two users.
    /// </summary>
    /// <param name="firstUserId"></param>
    /// <param name="secondUserId"></param>
    /// <returns></returns>
    Task DeleteAsync(int firstUserId, int secondUserId);

    /// <summary>
    /// Deletes a friendship.
    /// </summary>
    /// <param name="friendship"></param>
    /// <returns></returns>
    Task DeleteAsync(Friendship friendship);
}