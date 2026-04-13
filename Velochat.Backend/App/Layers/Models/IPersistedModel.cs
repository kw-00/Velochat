namespace Velochat.Backend.App.Layers.Models;

public interface IPersistedModel<T> where T : IModel
{
    T ToModel();
}
