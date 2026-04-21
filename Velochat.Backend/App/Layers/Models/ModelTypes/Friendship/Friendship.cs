using System.Diagnostics.CodeAnalysis;

namespace Velochat.Backend.App.Layers.Models;

public class Friendship : IMalleableModel
{
    [PrimaryKey]
    public int? InitiatorId { get; set; }

    [PrimaryKey]
    public int? ReceiverId { get; set; }

    public bool? Accepted { get; set; }

    [MemberNotNull(nameof(InitiatorId), nameof(ReceiverId))]
    public void EnsureIdentifiable()
    {
        if (InitiatorId is null || ReceiverId is null) 
            throw new ModelNotIdentifiableException();
    }

    [MemberNotNull(nameof(InitiatorId), nameof(ReceiverId), nameof(Accepted))]
    public void EnsureInsertable()
    {
        if (InitiatorId is null || ReceiverId is null || Accepted is null) 
            throw new ModelNotInsertableException();
    }
}