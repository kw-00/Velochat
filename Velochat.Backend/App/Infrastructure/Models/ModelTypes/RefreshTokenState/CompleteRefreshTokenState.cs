namespace Velochat.Backend.App.Infrastructure.Models;

public class CompleteRefreshTokenState : ICompleteModel
{
    [PrimaryKey]
    public required string Token { get; set; }
    public required int UserId { get; set; }
    public required string Status { get; set; }
}
