using Velochat.Backend.App.Layers.Infrastructure;
using Velochat.Backend.App.Layers.Models;

namespace Velochat.Backend.App.Layers.Infrastructure;

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
    /// Retrieves all rooms by member (user) ID.
    /// </summary>
    /// <param name="userId">
    /// The ID of the user of the member
    /// for whom the rooms are to be retrieved.
    /// </param>
    /// <returns>Complete models of the rooms.</returns>
    /// <exception cref="IdentifierNotFoundException{User}">
    /// When no user with matching ID is found.
    /// </exception>
    Task<IReadOnlyList<CompleteRoom>> GetByMemberIdAsync(int userId);

    /// <summary>
    /// Retrieves all rooms by invitee (user) ID.
    /// </summary>
    /// <param name="userId">
    /// The ID of the user of the invitee
    /// for whom the rooms are to be retrieved.
    /// </param>
    /// <returns>Complete models of the rooms.</returns>
    /// <exception cref="IdentifierNotFoundException{User}">
    /// When no user with matching ID is found.
    /// </exception>
    Task<IReadOnlyList<CompleteRoom>> GetByInviteeIdAsync(int userId);

    /// <summary>
    /// Inserts a new room.
    /// </summary>
    /// <param name="room">
    /// A malleable model of the room to insert.
    /// </param>
    /// <returns>
    /// A complete model of the inserted room.
    /// </returns>
    /// <exception cref="ModelNotInsertableException">
    /// When the room model is not insertable.
    /// </exception>
    /// <exception cref="DuplicateRoomPathException">
    /// When room owner/name combination is not unique.
    /// </exception>
    Task<CompleteRoom> CreateAsync(Room room);

    /// <summary>
    /// Deletes a room.
    /// </summary>
    /// <param name="roomId"></param>
    /// <returns></returns>
    Task DeleteAsync(int roomId);


}