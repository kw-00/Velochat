namespace Velochat.Backend.App.Layers.Models;

public class PersistedRefreshTokenState : IPersistedModel<RefreshTokenState>
{
    public required string Token { get; set; }
    public required string State { get; set; }

    public RefreshTokenState ToModel() => new() { Token = Token, Status = State };
}
