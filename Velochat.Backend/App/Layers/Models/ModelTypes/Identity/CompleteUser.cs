namespace Velochat.Backend.App.Layers.Models;


public class CompleteUser : ICompleteModel
{
    [PrimaryKey]
    public required int Id { get; init; }
    public required string Login { get; init; }

    public User ToModel() => new()
    {
        Id = Id,
        Login = Login
    };
    
}
