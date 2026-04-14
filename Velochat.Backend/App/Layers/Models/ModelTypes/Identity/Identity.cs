using System.Diagnostics.CodeAnalysis;

namespace Velochat.Backend.App.Layers.Models;

public class Identity : IMalleableModel
{
    [PrimaryKey]
    public int? Id { get; set; }
    public string? Login { get; set; }
    public string? PasswordHash { get; set; }

    [MemberNotNull(nameof(Login), nameof(PasswordHash))]
    public void EnsureInsertable()
    {
        if (Id is not null || Login is null || PasswordHash is null) throw new ModelNotInsertableException();
    }
}
