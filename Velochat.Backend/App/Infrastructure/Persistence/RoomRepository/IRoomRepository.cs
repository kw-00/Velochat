using Velochat.Backend.App.Infrastructure.Persistence;
using Velochat.Backend.App.Infrastructure.Models;

namespace Velochat.Backend.App.Infrastructure.Persistence;

public interface IRoomRepository
{
    /// <summary>
    /// Retrieves a room by ID.
    /// </summary>
    /// <param name="roomId">The ID of the room.</param>
    /// <returns>
    /// A complete model of the room
    /// or null if no room with matching ID is found.
    /// </returns>
    Task<CompleteRoom?> GetByIdAsync(int roomId);

    /// <summary>
    /// Retrieves all rooms in which there is
    /// a given user.
    /// </summary>
    /// <param name="userId">
    /// The ID of the user
    /// for whom the rooms are to be retrieved.
    /// </param>
    /// <returns>Complete models of the rooms.</returns>
    /// <exception cref="IdentifierNotFoundException{User}">
    /// When no user with matching ID is found.
    /// </exception>
    Task<IReadOnlyList<CompleteRoom>> GetByMemberIdAsync(int userId);

    /// <summary>
    /// Inserts a new room and immediately
    /// adds a specified user to it.
    /// </summary>
    /// <param name="room">
    /// A malleable model of the room to insert.
    /// </param>
    /// <param name="userId">
    /// The IDs of the user to add to the room
    /// upon creating it.
    /// </param>
    /// <returns>
    /// A complete model of the inserted room.
    /// </returns>
    /// <exception cref="ModelNotInsertableException">
    /// When the room model is not insertable.
    /// </exception>
    /// <exception cref="IdentifierNotFoundException{User}">
    /// When no user with matching ID is found.
    /// </exception>
    Task<CompleteRoom> CreateAsync(Room room, int userId);

    /// <summary>
    /// Deletes a room.
    /// </summary>
    /// <param name="roomId"></param>
    /// <returns></returns>
    Task DeleteAsync(int roomId);


}