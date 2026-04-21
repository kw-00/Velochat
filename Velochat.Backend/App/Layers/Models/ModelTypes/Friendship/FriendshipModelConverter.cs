namespace Velochat.Backend.App.Layers.Models;

public static class FriendshipModelConverter
{
    /// <summary>
    /// Converts <see cref="Friendship"/> to <see cref="CompleteFriendship"/>
    /// Does not mutate the original object.
    /// </summary>
    /// <param name="friendship">The friendship to convert</param>
    /// <returns>The conversion result</returns>
    /// <exception cref="ModelNotCompleteException">
    /// Thrown when the Subject1Id or Subject2Id of the friendship is null.
    /// </exception>
    public static CompleteFriendship ToCompleteModel(this Friendship friendship) => new()
    {
        Subject1Id = friendship.Subject1Id ?? throw new ModelNotCompleteException(),
        Subject2Id = friendship.Subject2Id ?? throw new ModelNotCompleteException()
    };

    /// <summary>
    /// Converts <see cref="CompleteFriendship"/> to <see cref="Friendship"/>
    /// Does not mutate the original object.
    /// </summary>
    /// <param name="completeFriendship">The complete friendship to convert</param>
    /// <returns>The conversion result</returns>
    public static Friendship ToModel(this CompleteFriendship completeFriendship) => new()
    {
        Subject1Id = completeFriendship.Subject1Id,
        Subject2Id = completeFriendship.Subject2Id
    };
}