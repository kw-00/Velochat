using Velochat.Backend.App.API.Realtime.Channels;
using Velochat.Backend.App.API.Realtime.RPCManagement;
using Velochat.Backend.App.API.Realtime.Session;
using Velochat.Backend.App.Infrastructure.Models;
using Velochat.Backend.App.Infrastructure.Persistence;
using Velochat.Backend.App.Shared.Exceptions;

namespace Velochat.Backend.App.API.Domains.Friendship;

public class FriendshipCommands(
    FriendshipRepository friendshipRepository,
    UserRepository userRepository,
    UserNotificationChannels userNotificationChannels
) : IFriendshipCommands
{
    public Task<IReadOnlyList<CompleteUser>> GetFriendsAsync(IRealtimeSession session)
    {
        var friends = friendshipRepository.GetFriendsAsync(session.UserId);
        return friends;
    }

    public Task<IReadOnlyList<CompleteUser>> GetRequestsAsync(IRealtimeSession session)
    {
        var requestedFriendships = friendshipRepository
            .GetPendingInitiatorsAsync(session.UserId);
        return requestedFriendships;
    }

    public async Task RequestAsync(IRealtimeSession session, int userId)
    {
        try
        {
            var potentialFriend = friendshipRepository
                .CreatePendingAsync(session.UserId, userId);
            var self = await userRepository.GetByIdAsync(session.UserId)
                ?? throw new RaceConditionException(
                    "User not found in database despite having been added."
                    + " Likely race condition."
                );
            await userNotificationChannels.SendFriendshipRequested(session, userId, self);
        }
        catch (RepositoryException ex)
        {
            if (ex is IdentifierNotFoundException<User>)
            {
                throw new NotFoundException(ex.Message);
            }
            if (ex is EquivalentAlreadyExistsException<Infrastructure.Models.Friendship>)
            {
                throw new ConflictException(ex.Message);
            }
            throw;
        }
    }

    public async Task<CompleteUser> AcceptAsync(IRealtimeSession session, int userId)
    {
        try
        {
            var newFriend = await friendshipRepository.AcceptAsync(
                new Infrastructure.Models.Friendship
                {
                    InitiatorId = session.UserId,
                    ReceiverId = userId
                }
            );
            var self = await userRepository.GetByIdAsync(session.UserId) 
                ?? throw new RaceConditionException(
                    "User not found in database despite having been added."
                    + " Likely race condition."
                );
            await userNotificationChannels.SendFriendshipAccepted(session, userId, self);
            return newFriend;
        }
        catch (IdentifierNotFoundException<Infrastructure.Models.Friendship> ex)
        {
            throw new NotFoundException(ex.Message);
        }
    }

    public async Task DeclineAsync(IRealtimeSession session, int userId)
    {
        await friendshipRepository.DeleteAsync(session.UserId, userId);
    }

    public async Task RemoveFriendAsync(IRealtimeSession session, int userId)
    {
        await friendshipRepository.DeleteAsync(session.UserId, userId);
        await userNotificationChannels.SendUnfriended(session, userId, session.UserId);
    }
}