namespace Velochat.Backend.App.Layers.Models;

public class CompleteRefreshTokenState : ICompleteModel
{
    public required string Token { get; set; }
    public required string Status { get; set; }
}
