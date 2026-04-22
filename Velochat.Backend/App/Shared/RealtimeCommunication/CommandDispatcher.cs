using System.Collections.Concurrent;
using Microsoft.AspNetCore.SignalR;

namespace Velochat.Backend.App.Shared.RealtimeCommunication
{
    public abstract class CommandDispatcher
    {
        private ConcurrentDictionary<
            string, Func<IRealtimeSession, object[], Task<CommandResult>>
        > _handlers = new();

        public async Task<CommandResult> ExecuteAsync(
            IRealtimeSession realtimeSession,
            string command,
            object[] args
        ) 
        {
            try
            {
                var handler = _handlers[command];
                return await handler(realtimeSession, args);
            }
            catch (KeyNotFoundException)
            {
                throw new KeyNotFoundException(
                    $"Command \"{command}\" not found."
                );
            }
        }

        public void Register(
            string command, Func<IRealtimeSession, object[], Task<CommandResult>> handler
        )
        {
            _handlers[command] = handler;
        }
    }
}