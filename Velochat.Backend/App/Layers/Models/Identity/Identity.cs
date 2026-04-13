using System.Diagnostics.CodeAnalysis;

namespace Velochat.Backend.App.Layers.Models;

public class Identity : IModel
{
    public int? Id { get; set; }
    public string? Login { get; set; }
    public string? PasswordHash { get; set; }

    [MemberNotNullWhen(true, nameof(Login), nameof(PasswordHash))]
    public bool IsInsertable => Id is null && Login is not null && PasswordHash is not null;
}
