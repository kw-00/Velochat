namespace Velochat.Backend.App.Infrastructure.Models;

public static class ChatMessageModelConverter
{
    /// <summary>
    /// Converts a <see cref="ChatMessage"/> to a <see cref="CompleteChatMessage"/>.
    /// Does not mutate the original object.
    /// </summary>
    /// <param name="chatMessage">The chat message to convert.</param>
    /// <returns>The conversion result.</returns>
    /// <exception cref="ModelNotCompleteException">
    /// Thrown when the Id, RoomId, AuthorId or Content of the chat message is null.
    /// </exception>
    public static CompleteChatMessage ToCompleteModel(this ChatMessage chatMessage)
        => new()
        {
            Id = chatMessage.Id ?? throw new ModelNotCompleteException(),
            RoomId = chatMessage.RoomId ?? throw new ModelNotCompleteException(),
            AuthorId = chatMessage.AuthorId ?? throw new ModelNotCompleteException(),
            Content = chatMessage.Content ?? throw new ModelNotCompleteException()
        };


    /// <summary>
    /// Converts a <see cref="CompleteChatMessage"/> to a <see cref="ChatMessage"/>.
    /// Does not mutate the original object.
    /// </summary>
    /// <param name="completeChatMessage">The complete chat message to convert.</param>
    /// <returns>The conversion result.</returns>
    public static ChatMessage ToModel(this CompleteChatMessage completeChatMessage)
        => new()
        {
            Id = completeChatMessage.Id,
            RoomId = completeChatMessage.RoomId,
            AuthorId = completeChatMessage.AuthorId,
            Content = completeChatMessage.Content
        };
}