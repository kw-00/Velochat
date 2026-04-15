namespace Velochat.Backend.App.Layers.Models;

public static class IdentityModelConverter
{
    /// <summary>
    /// Converts <see cref="Identity"/> to <see cref="CompleteIdentity"/>.</summary>
    /// <param name="identity">The identity to convert.</param>
    /// <returns>The conversion result.</returns>
    /// <exception cref="ModelNotCompleteException">
    /// Thrown when the Id or Login of the identity is null.
    /// </exception>
    public static CompleteIdentity ToCompleteModel(this Identity identity) 
        => new()
        {
            Id = identity.Id ?? throw new ModelNotCompleteException(),
            Login = identity.Login ?? throw new ModelNotCompleteException(),
        };


    /// <summary>
    /// Converts <see cref="CompleteIdentity"/> to <see cref="Identity"/>.
    /// </summary>
    /// <param name="identity">The complete identity to convert.</param>
    /// <returns>The conversion result.</returns>
    public static Identity ToModel(this CompleteIdentity identity) => new()
    {
        Id = identity.Id,
        Login = identity.Login,
    };
}