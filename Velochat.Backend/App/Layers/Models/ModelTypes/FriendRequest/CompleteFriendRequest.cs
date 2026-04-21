namespace Velochat.Backend.App.Layers.Models;

public class CompleteFriendRequest : ICompleteModel
{
    [PrimaryKey]
    public int SenderId { get; set; }

    [PrimaryKey]
    public int RecipientId { get; set; }
}