namespace Velochat.Backend.App.Layers.Models;


public class PersistedChatMessage : IPersistedModel<ChatMessage>
{
    public required int Id { get; init; }
    public required int RoomId { get; init; }
    public required int AuthorId { get; init; }
    public required string Content { get; init; }

    public ChatMessage ToModel() => new()
    {
        Id = Id,
        RoomId = RoomId,
        AuthorId = AuthorId,
        Content = Content
    };
}
