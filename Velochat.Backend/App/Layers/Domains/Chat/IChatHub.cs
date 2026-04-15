using Velochat.Backend.App.Layers.DTOs;
using Velochat.Backend.App.Layers.Models;

namespace Velochat.Backend.App.Layers.Domains.Chat;

public interface IChatHub
{
    Task<InitialChatInformation> GetInitialChatInformation();

    Task<CompleteRoom> CreateRoom(string name);

    Task DestroyRoom(int roomId);

    Task<CompleteRoom> JoinRoom(int roomId);

    Task LeaveRoom(int roomId);

    Task Invite(int roomId, int identityId);

    Task RevokeInvitation(int roomId, int identityId);

    Task KickMember(int roomId, int identityId);

    Task<CompleteChatMessage> SendMessage(int roomId, string content);

    Task<IReadOnlyList<CompleteChatMessage>> GetMessagesBefore(int roomId, int before);

    Task<IReadOnlyList<CompleteChatMessage>> GetMessagesAfter(int roomId, int after);

    Task<IReadOnlyList<CompleteChatMessage>> GetRecentMessages(int roomId);
}

   