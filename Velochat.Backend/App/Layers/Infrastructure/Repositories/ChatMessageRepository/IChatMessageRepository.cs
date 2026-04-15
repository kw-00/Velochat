using Velochat.Backend.App.Layers.Models;

namespace Velochat.Backend.App.Layers.Infrastructure;

public interface IChatMessageRepository
{
    /// <summary>
    /// Retrieves chat messages by room ID.
    /// Starts from newest, retrieving no more 
    /// messages than the limit.
    /// </summary>
    /// <param name="roomId"></param>
    /// <param name="limit"></param>
    /// <returns>The chat messages retrieved.</returns>
    /// <exception cref="RecordNotFoundException{Room}">
    /// Thrown when the room ID is not found.
    /// </exception>
    Task<IReadOnlyList<CompleteChatMessage>> GetByRoomIdAsync(int roomId, int limit);

    /// <summary>
    /// Retrieves chat messages by room ID.
    /// Retrieves messages newer than the message with ID of after,
    /// retrieving no more messages than the limit, starting from the oldest.
    /// </summary>
    /// <param name="roomId"></param>
    /// <param name="after"></param>
    /// <param name="limit"></param>
    /// <returns>The chat messages retrieved.</returns>
    /// <exception cref="RecordNotFoundException{Room}">
    /// Thrown when the room ID is not found.
    /// </exception>
    Task<IReadOnlyList<CompleteChatMessage>> GetByRoomIdAfterAsync(
        int roomId, int after, int limit
    );


    /// <summary>
    /// Retrieves chat messages by room ID.
    /// Retrieves messages older than the message with ID of before,
    /// retrieving no more messages than the limit, starting from the newest.
    /// </summary>
    /// <param name="roomId"></param>
    /// <param name="before"></param>
    /// <param name="limit"></param>
    /// <returns>The chat messages retrieved.</returns>
    /// <exception cref="RecordNotFoundException{Room}">
    /// Thrown when the room ID is not found.
    /// </exception>
    Task<IReadOnlyList<CompleteChatMessage>> GetByRoomIdBeforeAsync(
        int roomId, int before, int limit
    );


    /// <summary>
    /// Inserts a new chat message.
    /// </summary>
    /// <param name="message">
    /// A malleable model of chat message to create.
    /// </param>
    /// <returns>A model of the created chat message.</returns>
    /// <exception cref="ModelNotInsertableException">
    /// Thrown when the chat message to be inserted is not insertable.
    /// </exception>
    /// <exception cref="RecordNotFoundException{Identity}">
    /// Thrown when the author ID is not found.
    /// </exception>
    /// <exception cref="RecordNotFoundException{Room}">
    /// Thrown when the room ID is not found.
    /// </exception>
    Task<CompleteChatMessage> CreateAsync(ChatMessage message);
}