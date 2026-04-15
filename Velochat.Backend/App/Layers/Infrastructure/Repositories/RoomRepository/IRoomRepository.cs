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
    /// Retrieves all rooms by member (identity) ID.
    /// </summary>
    /// <param name="identityId">
    /// The ID of the identity of the member
    /// for whom the rooms are to be retrieved.
    /// </param>
    /// <returns>Complete models of the rooms.</returns>
    /// <exception cref="IdentifierNotFoundException{Identity}">
    /// When no identity with matching ID is found.
    /// </exception>
    Task<IReadOnlyList<CompleteRoom>> GetByMemberIdAsync(int identityId);

    /// <summary>
    /// Retrieves all rooms by invitee (identity) ID.
    /// </summary>
    /// <param name="identityId">
    /// The ID of the identity of the invitee
    /// for whom the rooms are to be retrieved.
    /// </param>
    /// <returns>Complete models of the rooms.</returns>
    /// <exception cref="IdentifierNotFoundException{Identity}">
    /// When no identity with matching ID is found.
    /// </exception>
    Task<IReadOnlyList<CompleteRoom>> GetByInviteeIdAsync(int identityId);

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