using Velochat.Backend.App.Layers.Models;

namespace Velochat.Backend.App.Layers.Infrastructure;

public class AlreadyFriendsException(FriendRequest friendRequest) : RepositoryException(
    $"Identity with ID of {friendRequest.SenderId} cannot send friend request"
        + $" to identity with ID of {friendRequest.RecipientId},"
        + " because they are already friends."
    );