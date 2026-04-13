using System.Diagnostics.CodeAnalysis;

namespace Velochat.Backend.App.Layers.Models;

public class Room : IModel
{
    public int? Id { get; set; }
    public string? Name { get; set; }
    public int? OwnerId { get; set; }

    [MemberNotNullWhen(true, nameof(Name), nameof(OwnerId))]
    public bool IsInsertable => Id is null && Name is not null && OwnerId is not null;
}
