namespace Velochat.Backend.App.Layers.Models;


public class CompleteChatMessage : ICompleteModel
{
    [PrimaryKey]
    public required int Id { get; init; }
    public required int RoomId { get; init; }
    public required int AuthorId { get; init; }
    public required string Content { get; init; }
}
