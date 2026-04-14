using Velochat.Backend.App.Exceptions.RepositoryExceptions;
using Velochat.Backend.App.Layers.Models;

namespace Velochat.Backend.App.Layers.Infrastructure;

public interface IRoomPresenceRepository
{
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
    /// <exception cref="RecordNotFoundException{Identity}">
    /// Thrown when no identity with a matching ID exists.
    /// </exception> 
    /// <exception cref="RecordNotFoundException{Room}">
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
    Task DeleteAsync(CompleteRoomPresence roomPresence);
}