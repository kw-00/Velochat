using Velochat.Backend.App.Shared.RealtimeCommunication;
using Velochat.Backend.App.Shared.TypeHelpers;

namespace Velochat.Backend.App.Layers.Domains.Messaging;


public class MessagingCommandDispatcher : CommandDispatcher
{
    public MessagingCommandDispatcher(IMessagingCommands commands)
    {
        Register("SendMessage", async (session, args) =>
        {
            var content = args[0].MapTo<string>();
            return CommandResult.From(
                await commands.SendMessage(session, content)
            );
        });

        Register("GoOlder", async (session, args) =>
        {
            var oldestMessageOnClient = args[0].MapTo<int>(); 
            return CommandResult.From(
                await commands.GoOlder(session, oldestMessageOnClient)
            );
        });

        Register("GoNewer", async (session, args) =>
        {
            var newestMessageOnClient = args[0].MapTo<int>(); 
            return CommandResult.From(
                await commands.GoNewer(session, newestMessageOnClient)
            );
        });

        Register("SwitchFocusedRoom", async (session, args) =>
        {
            var toRoomId = args[0].MapTo<int>(); 
            var newestMessageOnClient = args[1].MapTo<int?>(); 
            return CommandResult.From(
                await commands.SwitchFocus(session, toRoomId, newestMessageOnClient)
            );
        });

        Register("ZoneOut", async (session, args) =>
        {
            await commands.ZoneOut(session);
            return CommandResult.Empty();
        });
    }
}