namespace Velochat.Backend.App.Infrastructure.Models;

public interface IMalleableModel : IModel
{
    void EnsureInsertable();

    void EnsureIdentifiable();
}
