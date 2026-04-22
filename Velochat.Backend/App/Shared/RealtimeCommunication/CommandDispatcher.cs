using System.Collections.Concurrent;
using Microsoft.AspNetCore.SignalR;
using Velochat.Backend.App.Shared.Exceptions;

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
                try
                {
                    return await handler(realtimeSession, args);
                }
                catch (Exception ex)
                {
                    if (ex is StatusCodeException scex)
                    {
                        return CommandResult.Error(scex);
                    }
                    else
                    {
                        return CommandResult.Error(
                            new InternalServerErrorException(ex.Message)
                        );
                    }
                }
            }
            catch (KeyNotFoundException)
            {
                return CommandResult.Error(
                    new BadRequestException(
                        $"Command \"{command}\" not found."
                    )
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