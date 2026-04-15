namespace Velochat.Backend.App.Layers.Models;

public class CompleteRefreshTokenState : ICompleteModel
{
    [PrimaryKey]
    public required string Token { get; set; }
    public required int IdentityId { get; set; }
    public required string Status { get; set; }
}
