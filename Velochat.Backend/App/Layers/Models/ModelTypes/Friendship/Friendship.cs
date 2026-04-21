using System.Diagnostics.CodeAnalysis;

namespace Velochat.Backend.App.Layers.Models;

public class Friendship : IMalleableModel
{
    [PrimaryKey]
    public int? Subject1Id { get; set; }

    [PrimaryKey]
    public int? Subject2Id { get; set; }

    [MemberNotNull(nameof(Subject1Id), nameof(Subject2Id))]
    public void EnsureIdentifiable()
    {
        if (Subject1Id is null || Subject2Id is null) 
            throw new ModelNotIdentifiableException();
    }

    [MemberNotNull(nameof(Subject1Id), nameof(Subject2Id))]
    public void EnsureInsertable()
    {
        if (Subject1Id is not null || Subject2Id is not null) 
            throw new ModelNotInsertableException();
    }
}