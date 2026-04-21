using System.Diagnostics.CodeAnalysis;

namespace Velochat.Backend.App.Layers.Models;

public class FriendRequest : IMalleableModel
{
    [PrimaryKey]
    public int? SenderId { get; set; }

    [PrimaryKey]
    public int? RecipientId { get; set; }

    [MemberNotNull(nameof(SenderId), nameof(RecipientId))]
    public void EnsureIdentifiable()
    {
        if (SenderId is null || RecipientId is null) 
            throw new ModelNotIdentifiableException();
    }

    [MemberNotNull(nameof(SenderId), nameof(RecipientId))]
    public void EnsureInsertable()
    {
        if (SenderId is null || RecipientId is null) 
            throw new ModelNotInsertableException();
    }
}