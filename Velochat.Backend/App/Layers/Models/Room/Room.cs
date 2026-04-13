using System.Diagnostics.CodeAnalysis;

namespace Velochat.Backend.App.Layers.Models;

public class Room : IModel
{
    public int? Id { get; set; }
    public string? Name { get; set; }
    public int? OwnerId { get; set; }

    [MemberNotNull(nameof(Name), nameof(OwnerId))]
    public void EnsureInsertable()
    {
        if (Id is not null || Name is null || OwnerId is null) throw new ModelNotInsertableException();
    }
}
