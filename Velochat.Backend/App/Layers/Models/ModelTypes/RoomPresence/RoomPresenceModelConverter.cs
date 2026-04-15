namespace Velochat.Backend.App.Layers.Models;

public static class RoomPresenceModelConverter
{

    /// <summary>
    /// Converts <see cref="RoomPresence"/> to <see cref="CompleteRoomPresence"/>
    /// Does not mutate the original object.
    /// </summary>
    /// <param name="roomPresence"></param>
    /// <returns>The conversion result.</returns>
    /// <exception cref="ModelNotCompleteException"></exception>
    public static CompleteRoomPresence ToCompleteModel(this RoomPresence roomPresence) 
        => new()
        {
            RoomId = roomPresence.RoomId ?? throw new ModelNotCompleteException(),
            MemberId = roomPresence.MemberId ?? throw new ModelNotCompleteException()
        };
    
    /// <summary>
    /// Converts <see cref="CompleteRoomPresence"/> to <see cref="RoomPresence"/>.
    /// Does not mutate the original object.
    /// </summary>
    /// <param name="roomPresence"></param>
    /// <returns>The conversion result.</returns>
    public static RoomPresence ToModel(this CompleteRoomPresence roomPresence) 
        => new()
        {
            RoomId = roomPresence.RoomId,
            MemberId = roomPresence.MemberId
        };
}