using Velochat.Backend.App.Shared.Exceptions;

namespace Velochat.Backend.App.API.Realtime.RPCManagement;

public class CommandResult
{
    public required int Status { get; init; }
    public required string Message { get; init; }
    public required bool HasData { get; init; }
    public required object? Data { get; init; }

    public static CommandResult Empty() => new()
    {
        Status = 204,
        Message = "No content",
        HasData = false,
        Data = null
    };

    public static CommandResult From(object? data) => new()
    {
        Status = 200,
        Message = "OK",
        HasData = true,
        Data = data
    };

    public static CommandResult Error(StatusCodeException ex)
    {
        return new()
        {
            Status = ex.StatusCode,
            Message = ex.Message,
            HasData = false,
            Data = ex.Message
        };
    }
}