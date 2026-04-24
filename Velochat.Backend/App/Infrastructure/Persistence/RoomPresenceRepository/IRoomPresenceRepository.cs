using Velochat.Backend.App.Infrastructure.Persistence;
using Velochat.Backend.App.Infrastructure.Models;

namespace Velochat.Backend.App.Infrastructure.Persistence;

public interface IRoomPresenceRepository
{
    /// <summary>
    /// Gets a room presence.
    /// </summary>
    /// <param name="roomPresence">
    /// A malleable model of the room presence to be retrieved.
    /// </param>
    /// <returns>
    /// A complete model of the room presence
    /// or null if the room presence does not exist.
    /// </returns>
    Task <CompleteRoomPresence?> GetAsync(RoomPresence roomPresence);
    
    /// <summary>
    /// Inserts a new room presence.
    /// </summary>
    /// <param name="roomPresence">
    /// A malleable model of the room presence to be inserted.
    /// </param>
    /// <returns>
    /// A complete model of the inserted room presence.
    /// </returns>
    /// <exception cref="DuplicatePrimaryKeyException{RoomPresence}">
    /// Thrown when the room presence already exists.
    /// </exception>
    /// <exception cref="IdentifierNotFoundException{User}">
    /// Thrown when no user with a matching ID exists.
    /// </exception> 
    /// <exception cref="IdentifierNotFoundException{Room}">
    /// Thrown when no room with a matching ID exists.
    /// </exception>
    Task<CompleteRoomPresence> CreateAsync(RoomPresence roomPresence);

    /// <summary>
    /// Deletes a room presence.
    /// </summary>
    /// <param name="roomPresence">
    /// A malleable model of the room presence to be deleted.
    /// </param>
    /// <returns></returns>
    /// <exception cref="ModelNotIdentifiableException">
    /// Thrown when the room presence is not identifiable.
    /// </exception>
    Task DeleteAsync(RoomPresence roomPresence);
}