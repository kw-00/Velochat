namespace Velochat.Backend.App.Infrastructure.Models;

public static class RoomModelConverter
{
    
    
    /// <summary>
    /// Converts <see cref="Room"/> to <see cref="CompleteRoom"/>.
    /// Does not mutate the original object.
    /// </summary>
    /// <param name="room">The room to convert.</param>
    /// <returns>The conversion result.</returns>
    /// <exception cref="ModelNotCompleteException">
    /// Thrown when the Id, Name or OwnerId of the room is null.
    /// </exception>
    public static CompleteRoom ToCompleteModel(this Room room) => new()
    {
        Id = room.Id ?? throw new ModelNotCompleteException(),
        Name = room.Name ?? throw new ModelNotCompleteException(),
    };


    /// <summary>
    /// Converts <see cref="CompleteRoom"/> to <see cref="Room"/>.
    /// Does not mutate the original object.
    /// </summary>
    /// <param name="room">The room to convert.</param>
    /// <returns>The conversion result.</returns>
    public static Room ToModel(this CompleteRoom room) => new()
    {
        Id = room.Id,
        Name = room.Name,
    };
}