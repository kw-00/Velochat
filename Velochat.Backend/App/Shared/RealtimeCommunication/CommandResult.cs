namespace Velochat.Backend.App.Shared.RealtimeCommunication;

public class CommandResult
{
    public bool HasData { get; private set; }
    public object? Data { get; private set; }

    public static CommandResult Empty() => new()
    {
        HasData = false,
        Data = null
    };

    public static CommandResult From(object? data) => new()
    {
        HasData = true,
        Data = data
    };
}