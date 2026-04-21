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

    Task Invite(int roomId, int userId);

    Task RevokeInvitation(int roomId, int userId);

    Task KickMember(int roomId, int userId);

    Task<CompleteChatMessage> SendMessage(int roomId, string content);

    /// <summary>
    /// Returns a DTO containingmessages from the currently selected room, 
    /// older than the oldest message on client (exclusive), 
    /// up to a configured limit.
    /// 
    /// The DTO also contains a "BottomReached" flag which is true if
    /// the newest available message available has been retrieved.
    /// 
    /// It also unsubscribes the client from the
    /// room's message feed.
    /// 
    /// Think of a user scrolling up to view chat history. They no longer
    /// need to see new messages in real time, since they are viewing
    /// older messages and won't see the new ones anyway.
    /// 
    /// Hence the unsubscribing.
    /// </summary>
    /// <param name="oldestMessageOnClient">
    /// The ID of oldest message on client.
    /// </param>
    /// <returns></returns>
    Task<GoOlderResponse> GoOlder(int oldestMessageOnClient);

    /// <summary>
    /// Returns a DTO containing messages from the currently selected room,
    /// starting from a certain message (exclusive), up to a configured limit.
    /// If it finds the newest message, it also subscribes the client to
    /// the room's message feed.
    /// 
    /// The DTO also contains a "TopReached" flag which is true if
    /// the oldest message available has been retrieved.
    /// 
    /// Think of a user scrolling down after looking through history. 
    /// Once they reach the bottom, their app
    /// starts receiving newer messages, since the user
    /// is now looking at new messags and wants to read any
    /// messages that come forth.
    /// </summary>
    /// <param name="newestMessageOnClient">
    /// The ID of newest message on client.
    /// </param>
    /// <returns></returns>
    Task<GoNewerResponse> GoNewer(int newestMessageOnClient);

    /// <summary>
    /// Switches the client to a new room.
    /// Unsubscribes client from all other rooms and subscribes to
    /// the specified room's message feed.
    /// Returns a DTO containing messages from the room switched to
    /// that are newer than the newest message on the client.
    /// 
    /// The messages returned are all messages from the newest one available
    /// to the client's newest message or till limit is reached.
    /// 
    /// The DTO also contains a "HasContitnuity" flag.
    /// which is true if there is continuity between the newest message
    /// the client has and the messages returned in the DTO.
    /// 
    /// Client is expected to add the returned messages to its cache,
    /// or remove its cache and replace it with the returned messages
    /// if there is no continuity between the two.
    /// </summary>
    /// <param name="roomId">The ID of the room to switch to.</param>
    /// <param name="newestMessageOnClient">
    /// The ID of the newest message on client. 
    /// No messages newer than that one will be returned.
    /// </param>
    /// <returns>
    /// A DTO containing any messages missed by the client and a flag
    /// that signifies whether there is continuity between the
    /// messages returned and the client's newest message.
    /// </returns>
    Task<SwitchRoomsResponse> SwitchRooms(
        int roomId, int? newestMessageOnClient
    );

    /// <summary>
    /// Unsubscribes the client from the current room's message feed.
    /// </summary>
    Task UnsubscribeFromMessageFeed();
}

   