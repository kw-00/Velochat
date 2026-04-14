namespace Velochat.Backend.App.Layers.Models;

public interface IMalleableModel : IModel
{
    void EnsureInsertable();

    void EnsureIdentifiable();
}
