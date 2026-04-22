using System.Diagnostics.CodeAnalysis;

namespace Velochat.Backend.App.Infrastructure.Models;

public class ChatMessage : IMalleableModel
{
    [PrimaryKey]
    public int? Id { get; set; }
    public int? RoomId { get; set; }
    public int? AuthorId { get; set; }
    public string? Content { get; set; }

    [MemberNotNull(nameof(RoomId), nameof(AuthorId), nameof(Content))]
    public void EnsureInsertable()
    {
        if (Id is not null || RoomId is null || AuthorId is null || Content is null) 
            throw new ModelNotInsertableException();
    }

    [MemberNotNull(nameof(Id))]
    public void EnsureIdentifiable()
    {
        if (Id is null) throw new ModelNotIdentifiableException();
    }
}
