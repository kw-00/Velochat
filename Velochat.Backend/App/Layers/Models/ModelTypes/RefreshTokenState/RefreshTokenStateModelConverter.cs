namespace Velochat.Backend.App.Layers.Models;

public static class RefreshTokenStateModelConverter
{
    
    /// <summary>
    /// Converts a <see cref="RefreshTokenState"/>  to a <see cref="CompleteRefreshTokenState"/>.
    /// </summary>
    /// <param name="refreshTokenState">The RefreshTokenState to convert.</param>
    /// <returns>A CompleteRefreshTokenState.</returns>
    /// <exception cref="ModelNotCompleteException">
    /// Thrown when either the Token or Status of the RefreshTokenState is null.
    /// </exception>
    public static CompleteRefreshTokenState ToCompleteModel(this RefreshTokenState refreshTokenState) 
        => new()
        {
            Token = refreshTokenState.Token ?? throw new ModelNotCompleteException(),
            IdentityId = refreshTokenState.IdentityId ?? throw new ModelNotCompleteException(),
            Status = refreshTokenState.Status ?? throw new ModelNotCompleteException()
        };
    
    /// <summary>
    /// Converts a <see cref="RefreshTokenState"/>  to a <see cref="CompleteRefreshTokenState"/>.
    /// </summary>
    /// <param name="refreshTokenState">The CompleteRefreshTokenState to convert.</param>
    /// <returns>A RefreshTokenState.</returns>
    public static RefreshTokenState ToModel(this CompleteRefreshTokenState refreshTokenState) 
        => new()
        {
            Token = refreshTokenState.Token,
            IdentityId = refreshTokenState.IdentityId,
            Status = refreshTokenState.Status
        };
}