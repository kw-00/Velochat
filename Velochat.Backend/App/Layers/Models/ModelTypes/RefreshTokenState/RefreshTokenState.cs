using System.Diagnostics.CodeAnalysis;
using System.Runtime.ConstrainedExecution;

namespace Velochat.Backend.App.Layers.Models;

public class RefreshTokenState : IMalleableModel
{
    public static readonly string Active = "active";
    public static readonly string Used = "used";
    public static readonly string Revoked = "revoked";

    [PrimaryKey]
    public string? Token { get; set; }

    public int? UserId { get; set; }
    public string? Status { get; set; }

    [MemberNotNull(nameof(Token), nameof(UserId), nameof(Status))]
    public void EnsureInsertable()
    {
        if (Token is null || UserId is null || Status is null) throw new ModelNotInsertableException();
    }

    [MemberNotNull(nameof(Token))]
    public void EnsureIdentifiable()
    {
        if (Token is null) throw new ModelNotIdentifiableException();
    }
}
