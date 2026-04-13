namespace Velochat.Backend.App.Layers.Models;


public class PersistedIdentity : IPersistedModel<Identity>
{
    public required int Id { get; init; }
    public required string Login { get; init; }

    public Identity ToModel() => new()
    {
        Id = Id,
        Login = Login
    };
    
}
