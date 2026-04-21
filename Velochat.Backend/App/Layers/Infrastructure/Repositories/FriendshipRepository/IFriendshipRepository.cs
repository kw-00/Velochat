using Velochat.Backend.App.Layers.Models;

namespace Velochat.Backend.App.Layers.Infrastructure;

public interface IFriendshipRepository
{
    /// <summary>
    /// Retrieves a friend request between two identities.
    /// </summary>
    /// <param name="senderId"></param>
    /// <param name="recipientId"></param>
    /// <returns>
    /// The matching friend request
    /// or null if not found.
    /// </returns>
    Task<CompleteFriendRequest?> GetFriendRequestAsync(int senderId, int recipientId);

    /// <summary>
    /// Retrieves a list of identities that sent friend requests to the given identity.
    /// </summary>
    /// <param name="identityId"></param>
    /// <returns>
    /// A list of matching identities.
    /// </returns>
    Task<IReadOnlyList<CompleteIdentity>> GetFriendRequestSendersAsync(int identityId);

    /// <summary>
    /// Retrieves a list of identities that are friends with the given identity.
    /// </summary>
    /// <param name="identityId"></param>
    /// <returns>
    /// A list of matching identities.
    /// </returns>
    Task<IReadOnlyList<CompleteIdentity>> GetFriendshipsAsync(int identityId);

    /// <summary>
    /// Creates a friend request between two identities.
    /// </summary>
    /// <param name="friendRequest"></param>
    /// <returns>
    /// A complete model of the recipient's identity.
    /// </returns>
    /// <exception cref="ModelNotInsertableException">
    /// Thrown when the friend request is not insertable.
    /// </exception> 
    /// <exception cref="IdentifierNotFoundException{Identity}">
    /// Thrown when the sender or recipient does not exist.
    /// </exception>
    /// <exception cref="MutualFriendRequestException">
    /// Thrown when sender already received a request from recipient.
    /// </exception>
    /// <exception cref="AlreadyFriendsException">
    /// Thrown when sender and recipient are already friends.
    /// </exception>
    /// <exception cref="DuplicatePrimaryKeyException{FriendRequest}"> 
    /// Thrown when a friend request from the same sender 
    /// to the same recipient already exists.
    /// </exception>
    Task<CompleteIdentity?> CreateFriendRequestAsync(FriendRequest friendRequest);


    /// <summary>
    /// Accepts a friend request between two identities.
    /// Creates a friendship between them and removes the friend request.
    /// </summary>
    /// <param name="friendRequest"></param>
    /// <returns>
    /// A complete model of the recipient's identity.
    /// </returns>
    /// <exception cref="IdentifierNotFoundException{FriendRequest}">
    /// Thrown when the friend request does not exist.
    /// </exception>
    /// <exception cref="AlreadyFriendsException">
    /// Thrown when sender and recipient are already friends.
    /// </exception>
    Task<CompleteIdentity?> AcceptFriendRequestAsync(FriendRequest friendRequest);
}