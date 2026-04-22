using Velochat.Backend.App.Infrastructure.Models;
using Velochat.Backend.App.API.Realtime.RPCManagement;

namespace Velochat.Backend.App.API.Domains.Rooms;

public interface IRoomsCommands
{
    Task<CompleteRoom> CreateRoom(IRealtimeSession session, string name);
    Task<CompleteRoom> JoinRoom(IRealtimeSession session, int roomId);
    Task LeaveRoom(IRealtimeSession session, int roomId);
    Task DestroyRoom(IRealtimeSession session, int roomId);

    Task AddUser(IRealtimeSession session, int roomId, int userId);

    Task KickUser(IRealtimeSession session, int roomId, int userId);
}

