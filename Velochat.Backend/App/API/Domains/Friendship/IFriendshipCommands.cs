using Velochat.Backend.App.Infrastructure.Models;
using Velochat.Backend.App.API.Realtime.RPCManagement;
using Velochat.Backend.App.API.Realtime.Session;

namespace Velochat.Backend.App.API.Domains.Friendship;

public interface IFriendshipCommands
{
    /// <summary>
    /// Gets all friends of the caller.
    /// </summary>
    /// <param name="session"></param>
    /// <returns></returns>
    Task<IReadOnlyList<CompleteUser>> GetFriendsAsync(IRealtimeSession session);

    /// <summary>
    /// Gets all users who have pending friend requests to the caller.
    /// </summary>
    /// <param name="session"></param>
    /// <returns></returns>
    Task<IReadOnlyList<CompleteUser>> GetRequestsAsync(IRealtimeSession session);

    /// <summary>
    /// Sends a friend request to a user.
    /// </summary>
    /// <param name="session"></param>
    /// <param name="userId">
    /// The user whose friendship is requested.
    /// </param>
    /// <returns></returns>
    Task RequestAsync(IRealtimeSession session, int userId);

    /// <summary>
    /// Accepts a friend request from a user,
    /// if it exists.
    /// </summary>
    /// <param name="session"></param>
    /// <param name="userId">
    /// The user whose friend request is accepted.
    /// </param>
    /// <returns></returns>
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