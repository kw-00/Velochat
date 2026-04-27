using Velochat.Backend.App.API.Realtime.RPCManagement;
using Velochat.Backend.App.Shared.TypeHelpers;

namespace Velochat.Backend.App.API.Domains.Messaging;


public class MessagingCommandDispatcher : CommandDispatcher
{
    public MessagingCommandDispatcher(IMessagingCommands commands)
    {
        Register("SendMessage", async (session, args) =>
        {
            var content = args[0].MapTo<string>();
            return CommandResult.From(
                await commands.SendMessageAsync(session, content)
            );
        });

        Register("GoOlder", async (session, args) =>
        {
            var oldestMessageOnClient = args[0].MapTo<int>(); 
            return CommandResult.From(
                await commands.GoOlderAsync(session, oldestMessageOnClient)
            );
        });

        Register("GoNewer", async (session, args) =>
        {
            var newestMessageOnClient = args[0].MapTo<int>(); 
            return CommandResult.From(
                await commands.GoNewerAsync(session, newestMessageOnClient)
            );
        });

        Register("SubscribeFeed", async (session, args) =>
        {
            var newestMessageOnClient = args[0].MapTo<int?>();
            await commands.SubscribeFeedAsync(session, newestMessageOnClient);
            return CommandResult.Empty();
        });

        Register("UnsubscribeFeed", async (session, args) =>
        {
            await commands.UnsubscribeFeedAsync(session);
            return CommandResult.Empty();
        });

        Register("SwitchFocus", async (session, args) =>
        {
            var toRoomId = args[0].MapTo<int>(); 
            var newestMessageOnClient = args[1].MapTo<int?>(); 
            return CommandResult.From(
                await commands.SwitchFocusAsync(session, toRoomId, newestMessageOnClient)
            );
        });

        Register("ZoneOut", async (session, args) =>
        {
            await commands.ZoneOutAsync(session);
            return CommandResult.Empty();
        });
    }
}