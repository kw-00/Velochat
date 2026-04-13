using System.Diagnostics.CodeAnalysis;

namespace Velochat.Backend.App.Layers.Models;

public class Identity : IModel
{
    public int? Id { get; set; }
    public string? Login { get; set; }
    public string? PasswordHash { get; set; }

    [MemberNotNullWhen(true, nameof(Login), nameof(PasswordHash))]
    public void EnsureInsertable()
    {
        if (Id is not null || Login is null || PasswordHash is null) throw new ModelNotInsertableException();
    }
}
