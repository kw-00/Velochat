using System.Diagnostics.CodeAnalysis;
using System.Runtime.ConstrainedExecution;

namespace Velochat.Backend.App.Layers.Models;

public class RefreshTokenState : IModel
{
    public static readonly string Active = "active";
    public static readonly string Used = "used";
    public static readonly string Revoked = "revoked";
    public string? Token { get; set; }
    public string? Status { get; set; }

    [MemberNotNullWhen(true, nameof(Token), nameof(Status))]
    public void EnsureInsertable()
    {
        if (Token is not null || Status is null) throw new ModelNotInsertableException();
    }
}
