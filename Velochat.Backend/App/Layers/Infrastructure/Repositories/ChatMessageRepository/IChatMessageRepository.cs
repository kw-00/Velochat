using Velochat.Backend.App.Exceptions.RepositoryExceptions;
using Velochat.Backend.App.Layers.Models;

namespace Velochat.Backend.App.Layers.Infrastructure;

public interface IChatMessageRepository
{
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

    /// <summary>
    /// Retrieves all chat messages by room ID.
    /// </summary>
    /// <param name="roomId"></param>
    /// <returns>The chat messages retrieved.</returns>
    /// <exception cref="RecordNotFoundException{Room}">
    /// Thrown when the room ID is not found.
    /// </exception>
    Task<IReadOnlyList<CompleteChatMessage>> GetAllByRoomIdAsync(int roomId);

    /// <summary>
    /// Deletes all chat messages by room ID.
    /// </summary>
    /// <param name="roomId"></param>
    /// <exception cref="RecordNotFoundException{Room}">
    /// Thrown when the room ID is not found.
    /// </exception>
    Task DeleteAllByRoomIdAsync(int roomId);
}