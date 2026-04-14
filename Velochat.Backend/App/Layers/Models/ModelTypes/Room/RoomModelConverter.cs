namespace Velochat.Backend.App.Layers.Models;

public static class RoomModelConverter
{
    public static CompleteRoom ToCompleteModel(this Room room) => new()
    {
        Id = room.Id ?? throw new ModelNotCompleteException(),
        Name = room.Name ?? throw new ModelNotCompleteException(),
        OwnerId = room.OwnerId ?? throw new ModelNotCompleteException()
    };

    public static Room ToModel(this CompleteRoom room) => new()
    {
        Id = room.Id,
        Name = room.Name,
        OwnerId = room.OwnerId
    };
}