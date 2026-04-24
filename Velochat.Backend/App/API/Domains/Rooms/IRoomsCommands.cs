using Velochat.Backend.App.Infrastructure.Models;
using Velochat.Backend.App.API.Realtime.RPCManagement;
using Velochat.Backend.App.API.Realtime.Session;

namespace Velochat.Backend.App.API.Domains.Rooms;

public interface IRoomsCommands
{
    /// <summary>
    /// Retrieves all rooms for the caller.
    /// </summary>
    /// <param name="session"></param>
    /// <returns></returns>
    Task<IReadOnlyList<CompleteRoom>> GetRoomsAsync(IRealtimeSession session);

    /// <summary>
    /// Creates a room and adds the caller to it.
    /// </summary>
    /// <param name="session"></param>
    /// <param name="name">
    /// The name of the room.
    /// </param>
    /// <returns></returns>
    Task<CompleteRoom> CreateRoomAsync(IRealtimeSession session, string name);

    /// <summary>
    /// Removes the caller from a room.
    /// </summary>
    /// <param name="session"></param>
    /// <param name="roomId"></param>
    /// <returns></returns>
    Task LeaveRoomAsync(IRealtimeSession session, int roomId);

    /// <summary>
    /// Adds a user to the room. This is only allowed
    /// if the caller is in the room
    /// and is friends with the user to be added.
    /// </summary>
    /// <param name="session"></param>
    /// <param name="roomId"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task AddUserAsync(IRealtimeSession session, int roomId, int userId);
}

