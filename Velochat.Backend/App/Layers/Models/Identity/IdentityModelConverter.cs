namespace Velochat.Backend.App.Layers.Models;

public static class IdentityModelConverter
{
    public static CompleteIdentity ToCompleteModel(this Identity identity) => new()
    {
        Id = identity.Id ?? throw new ModelNotCompleteException(),
        Login = identity.Login ?? throw new ModelNotCompleteException(),
    };

    public static Identity ToModel(this CompleteIdentity identity) => new()
    {
        Id = identity.Id,
        Login = identity.Login,
    };
}