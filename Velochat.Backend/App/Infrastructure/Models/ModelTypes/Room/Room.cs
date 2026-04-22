using System.Diagnostics.CodeAnalysis;

namespace Velochat.Backend.App.Infrastructure.Models;

public class Room : IMalleableModel
{
    [PrimaryKey]
    public int? Id { get; set; }
    public string? Name { get; set; }
    public int? OwnerId { get; set; }

    [MemberNotNull(nameof(Name), nameof(OwnerId))]
    public void EnsureInsertable()
    {
        if (Id is not null || Name is null || OwnerId is null) throw new ModelNotInsertableException();
    }

    [MemberNotNull(nameof(Id))]
    public void EnsureIdentifiable()
    {
        if (Id is null) throw new ModelNotIdentifiableException();
    }
}
