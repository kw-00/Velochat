namespace Velochat.Backend.App.Layers.Models;

public static class FriendRequestModelConverter
{
    /// <summary>
    /// Converts <see cref="FriendRequest"/> to <see cref="CompleteFriendRequest"/>
    /// Does not mutate the original object.
    /// </summary>
    /// <param name="friendRequest">
    /// The <see cref="FriendRequest"/> to convert.
    /// </param>
    /// <returns>
    /// The conversion result.
    /// </returns>
    /// <exception cref="ModelNotCompleteException"></exception>
    public static CompleteFriendRequest ToCompleteModel(this FriendRequest friendRequest)
    {
        return new()
        {
            SenderId = friendRequest.SenderId ?? throw new ModelNotCompleteException(),
            RecipientId = friendRequest.RecipientId ?? throw new ModelNotCompleteException()
        };
    }


    /// <summary>
    /// Converts <see cref="CompleteFriendRequest"/> to <see cref="FriendRequest"/>
    /// Does not mutate the original object.
    /// </summary>
    /// <param name="completeFriendRequest">
    /// The <see cref="CompleteFriendRequest"/> to convert.
    /// </param>
    /// <returns>
    /// The conversion result.
    /// </returns>
    public static FriendRequest ToModel(this CompleteFriendRequest completeFriendRequest)
    {
        return new()
        {
            SenderId = completeFriendRequest.SenderId,
            RecipientId = completeFriendRequest.RecipientId
        };
    }
}