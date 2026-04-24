namespace Velochat.Backend.App.Infrastructure.Models;

public static class UserModelConverter
{
    /// <summary>
    /// Converts <see cref="User"/> to <see cref="CompleteUser"/>.
    /// Does not mutate the original object.
    /// </summary>
    /// <param name="user">The user to convert.</param>
    /// <returns>The conversion result.</returns>
    /// <exception cref="ModelNotCompleteException">
    /// Thrown when the Id or Login of the user is null.
    /// </exception>
    public static CompleteUser ToCompleteModel(this User user) 
        => new()
        {
            Id = user.Id ?? throw new ModelNotCompleteException(),
            Login = user.Login ?? throw new ModelNotCompleteException(),
        };


    /// <summary>
    /// Converts <see cref="CompleteUser"/> to <see cref="User"/>.
    /// Does not mutate the original object.
    /// </summary>
    /// <param name="user">The complete user to convert.</param>
    /// <returns>The conversion result.</returns>
    public static User ToModel(this CompleteUser user) => new()
    {
        Id = user.Id,
        Login = user.Login,
    };
}