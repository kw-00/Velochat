namespace Velochat.Backend.App.Layers.Models;

public static class RefreshTokenStateModelConverter
{
    public static CompleteRefreshTokenState ToCompleteModel(this RefreshTokenState refreshTokenState) => new()
    {
        Token = refreshTokenState.Token ?? throw new ModelNotCompleteException(),
        Status = refreshTokenState.Status ?? throw new ModelNotCompleteException()
    };
    
    public static RefreshTokenState ToModel(this CompleteRefreshTokenState refreshTokenState) => new()
    {
        Token = refreshTokenState.Token,
        Status = refreshTokenState.Status
    };
}