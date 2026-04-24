using Velochat.Backend.App.API.Realtime.RPCManagement;
using Velochat.Backend.App.Shared.TypeHelpers;

namespace Velochat.Backend.App.API.Domains.Rooms;


public class RoomsCommandDispatcher : CommandDispatcher
{
    public RoomsCommandDispatcher(IRoomsCommands commands)
    {
        Register("GetRooms", async (session, args) =>
        {
            return CommandResult.From(await commands.GetRoomsAsync(session));
        });

        Register("CreateRoom", async (session, args) =>
        {
            var name = args[0].MapTo<string>();
            return CommandResult.From(await commands.CreateRoomAsync(session, name));
        });

        Register("LeaveRoom", async (session, args) =>
        {
            var roomId = args[0].MapTo<int>();
            await commands.LeaveRoomAsync(session, roomId);
            return CommandResult.Empty();
        });

        Register("AddUser", async (session, args) =>
        {
            var roomId = args[0].MapTo<int>();
            var userId = args[1].MapTo<int>();
            await commands.AddUserAsync(session, roomId, userId);
            return CommandResult.Empty();
        });
    }
}