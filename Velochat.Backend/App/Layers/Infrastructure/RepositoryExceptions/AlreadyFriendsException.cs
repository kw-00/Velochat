using Velochat.Backend.App.Layers.Models;

namespace Velochat.Backend.App.Layers.Infrastructure;

public class AlreadyFriendsException(FriendRequest friendRequest) : RepositoryException(
    $"User with ID of {friendRequest.SenderId} cannot send friend request"
        + $" to user with ID of {friendRequest.ReceiverId},"
        + " because they are already friends."
    );