using Velochat.Backend.App.Layers.Models;

namespace Velochat.Backend.App.Layers.Domains.Chat;

public interface IChatOrchestration
{
    Task<CompleteRoom> CreateRoomAsync(int identityId, string name);

    Task RemoveRoomAsync(int identityId, int roomId);

    Task<CompleteRoom> JoinRoomAsync(int identityId, int roomId);

    Task<IReadOnlyList<CompleteRoom>> GetRoomsAsync(int identityId);

    Task InviteAsync(int identityId, int roomId, int inviteeId);

    Task RevokeInvitationAsync(int identityId, int roomId, int inviteeId);

    Task KickMemberAsync(int identityId, int roomId, int memberId);

    Task<CompleteChatMessage> CreateChatMessageAsync(int identityId, int roomId, string content);

    Task<IReadOnlyList<CompleteChatMessage>> GetChatMessagesAsync(int identityId, int roomId);
}

   