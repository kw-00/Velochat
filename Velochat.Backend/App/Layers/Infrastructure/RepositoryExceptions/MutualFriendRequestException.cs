using Velochat.Backend.App.Layers.Models;

namespace Velochat.Backend.App.Layers.Infrastructure;

public class MutualFriendRequestException(FriendRequest friendRequest) : RepositoryException(
    $"Identity with ID of {friendRequest.SenderId} cannot send friend request"
    + $" to identity with ID of {friendRequest.RecipientId},"
    + " because they already received a request from the other identity."
);