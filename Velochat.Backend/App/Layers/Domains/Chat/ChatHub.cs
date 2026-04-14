using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.VisualBasic;
using Velochat.Backend.App.Exceptions.StatusExceptions;
using Velochat.Backend.App.Layers.Models;

namespace Velochat.Backend.App.Layers.Domains.Chat;

public class ChatHub(IChatOrchestration chatOrchestration) : Hub
{
    [Authorize]
    public async Task<CompleteRoom> JoinRoomAsync(int roomId)
    {
        if (Context.User is null)
            throw new UnauthorizedException("User session is invalid.");
        
        var claimsDictionary = Context.User.Claims.ToDictionary(c => c.Type, c => c.Value);
        var subExists = claimsDictionary.TryGetValue("sub", out var rawSub);
        if (!subExists) throw new UnauthorizedException("User identifier (sub) is missing.");

        var parsedSuccessfully = int.TryParse(rawSub, out var identityId);
        if (!parsedSuccessfully) 
            throw new UnauthorizedException("User identifier (sub) is not parsable to int.");
        
        return await chatOrchestration.JoinRoomAsync(roomId, identityId);
    }
}