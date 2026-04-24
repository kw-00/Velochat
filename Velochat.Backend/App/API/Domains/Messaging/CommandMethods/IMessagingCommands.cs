using Velochat.Backend.App.Infrastructure.Models;
using Velochat.Backend.App.API.Realtime.RPCManagement;

namespace Velochat.Backend.App.API.Domains.Messaging;

public interface IMessagingCommands
{
    /// <summary>
    /// Creates a new message in the currently selected room
    /// and broadcasts it.
    /// </summary>
    /// <param name="session"></param>
    /// <param name="content"></param>
    /// <returns>A complete model of the message sent.</returns>
    Task<CompleteChatMessage> SendMessageAsync(IRealtimeSession session, string content);


    /// <summary>
    /// Returns a DTO containing messages from the currently selected room, 
    /// older than the oldest message on client (exclusive), 
    /// up to a configured limit.
    /// 
    /// <para>
    /// The DTO also contains a "TopReached" flag which is true if
    /// the oldest available message available has been retrieved.
    /// </para>
    /// 
    /// <para>
    /// It also unsubscribes the client from the
    /// room's message feed.
    /// </para>
    /// 
    /// <para>
    /// Think of a user scrolling up to view chat history. They no longer
    /// need to see new messages in real time, since they are viewing
    /// older messages and won't see the new ones anyway.
    /// </para>
    /// 
    /// Hence the unsubscribing.
    /// </summary>
    /// <param name="session"></param>
    /// <param name="oldestMessageOnClient">
    /// The ID of oldest message on client.
    /// </param>
    /// <returns></returns>
    Task<GoOlderResponse> GoOlderAsync(IRealtimeSession session, int oldestMessageOnClient);

    /// <summary>
    /// Returns a DTO containing messages from the currently selected room,
    /// starting from a certain message (exclusive), up to a configured limit.
    /// If it finds the newest message, it also subscribes the client to
    /// the room's message feed.
    /// 
    /// <para>
    /// The DTO also contains a "BottomReached" flag which is true if
    /// the newest message available has been retrieved.
    /// </para>
    /// 
    /// <para>
    /// Think of a user scrolling down after looking through history. 
    /// Once they reach the bottom, their app
    /// starts receiving newer messages, since the user
    /// is now looking at new messags and wants to read any
    /// messages that come forth.
    /// </para>
    /// </summary>
    /// <param name="session"></param>
    /// <param name="newestMessageOnClient">
    /// The ID of newest message on client.
    /// </param>
    /// <returns></returns>
    Task<GoNewerResponse> GoNewerAsync(IRealtimeSession session, int newestMessageOnClient);

    /// <summary>
    /// 
    /// Switches the client's focus to a different room.
    /// Then gives the client an update on any missed messages in the new room.
    /// 
    /// <para>
    /// The update is in the form of a DTO. The client may have missed messages 
    /// from the room switched to while it was focused on the prevous room.
    /// </para>
    /// 
    /// <para>
    /// The DTO returned contains those missed messages up to a certain limit.
    /// It also contains a "IsContinuity" flag which is true if
    /// all the missed messages have been returned.
    /// </para>
    /// 
    /// When there is continuity, the client might merge its cached messages
    /// with those returned. If there is no continuity, the client may discard its cache
    /// and replace it with the new messages.
    /// 
    /// </summary>
    /// <param name="session"></param>
    /// <param name="roomId">The ID of the room to switch to.</param>
    /// <param name="newestMessageOnClient">
    /// The ID of the newest message on client. 
    /// No messages older than that one will be returned.
    /// </param>
    /// <returns>
    /// A DTO containing missed messages for the newly focused room
    /// and a flag that signifies whether all missed messages have been returned.
    /// </returns>
    Task<SwitchRoomsResponse> SwitchFocusAsync(
        IRealtimeSession session, int roomId, int? newestMessageOnClient
    );

    /// <summary>
    /// Unsubscribes the client from the currently selected room's message feed
    /// and clears the client's current room cache.
    /// </summary>
    /// <param name="session"></param>
    /// <returns></returns>
    Task ZoneOutAsync(IRealtimeSession session);
}