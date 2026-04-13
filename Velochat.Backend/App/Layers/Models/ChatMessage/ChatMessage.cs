using System.Diagnostics.CodeAnalysis;

namespace Velochat.Backend.App.Layers.Models;

public class ChatMessage : IModel
{
    public int? Id { get; set; }
    public int? RoomId { get; set; }
    public int? AuthorId { get; set; }
    public string? Content { get; set; }

    [MemberNotNullWhen(true, nameof(RoomId), nameof(AuthorId), nameof(Content))]
    public bool CanBePeristed() 
        => Id is null && RoomId is not null && AuthorId is not null && Content is not null;
}
