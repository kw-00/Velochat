namespace Velochat.Backend.App.Layers.Models;

public static class RoomPresenceModelConverter
{

    public static CompleteRoomPresence ToCompleteModel(this RoomPresence roomPresence) => new()
    {
        RoomId = roomPresence.RoomId ?? throw new ModelNotCompleteException(),
        IdentityId = roomPresence.IdentityId ?? throw new ModelNotCompleteException()
    };
    
    public static RoomPresence ToModel(this CompleteRoomPresence roomPresence) => new()
    {
        RoomId = roomPresence.RoomId,
        IdentityId = roomPresence.IdentityId
    };
}