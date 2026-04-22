using Velochat.Backend.App.Infrastructure.Models;
using Velochat.Backend.App.API.Realtime.RPCManagement;

namespace Velochat.Backend.App.API.Domains.Friendship;

public interface IFriendshipCommands
{
    /// <summary>
    /// Retrieves all friends for the user.
    /// </summary>
    /// <param name="session">
    /// The session of the caller.
    /// </param>
    /// <returns>
    /// A list of users that are friends with the caller.
    /// </returns>
    Task<IReadOnlyList<CompleteUser>> GetFriendsAsync(IRealtimeSession session);

    /// <summary>
    /// Retrieves models of users who have
    /// pending friendship requests to the caller.
    /// </summary>
    /// <param name="session">
    /// The session of the caller.
    /// </param>
    /// <returns>
    /// A list of users that requested friendship with the caller.
    /// </returns>
    Task<IReadOnlyList<CompleteUser>> GetRequestsAsync(IRealtimeSession session);

    /// <summary>
    /// Requests friendship with a user.
    /// </summary>
    /// <param name="session">
    /// The session of the caller.
    /// </param>
    /// <param name="userId">
    /// The user whose friendship is requested.
    /// </param>
    /// <returns>
    /// A model of the user whose friendship is requested.
    /// </returns>
    Task<CompleteUser> RequestAsync(IRealtimeSession session, int userId);

    /// <summary>
    /// Accepts a friend request.
    /// </summary>
    /// <param name="session">
    /// The session of the caller.
    /// </param>
    /// <param name="userId">
    /// The user whose friendship request is accepted.
    /// </param>
    /// <returns>
    /// A model of the user who was accepted.
    /// </returns>
    Task<CompleteUser> AcceptAsync(IRealtimeSession session, int userId);

    /// <summary>
    /// Declines a friend request.
    /// </summary>
    /// <param name="session">
    /// The session of the caller.
    /// </param>
    /// <param name="userId">
    /// The user to be declined.
    /// </param>
    /// <returns></returns>
    Task DeclineAsync(IRealtimeSession session, int userId);
}