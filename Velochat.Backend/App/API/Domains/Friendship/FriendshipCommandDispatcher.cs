using Velochat.Backend.App.API.Realtime.RPCManagement;
using Velochat.Backend.App.Shared.TypeHelpers;

namespace Velochat.Backend.App.API.Domains.Friendship;

public class FriendshipCommandDispatcher : CommandDispatcher 
{
    public FriendshipCommandDispatcher(IFriendshipCommands commands)
    {
        Register("GetFriends", async (session, args) =>
        {
            return CommandResult.From(
                await commands.GetFriendsAsync(session)
            );
        });

        Register("GetRequests", async (session, args) =>
        {
            return CommandResult.From(
                await commands.GetRequestsAsync(session)
            );
        });

        Register("Request", async (session, args) =>
        {
            var userId = args[0].MapTo<int>(); 
            await commands.RequestAsync(session, userId);
            return CommandResult.Empty();
        });

        Register("Accept", async (session, args) =>
        {
            var userId = args[0].MapTo<int>(); 
            return CommandResult.From(
                await commands.AcceptAsync(session, userId)
            );
        });

        Register("Decline", async (session, args) =>
        {
            var userId = args[0].MapTo<int>(); 
            await commands.DeclineAsync(session, userId);
            return CommandResult.Empty();
        });

        Register("RemoveFriend", async (session, args) =>
        {
            var userId = args[0].MapTo<int>(); 
            await commands.RemoveFriendAsync(session, userId);
            return CommandResult.Empty();
        });
    }
}