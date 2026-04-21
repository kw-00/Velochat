using Npgsql;
using Velochat.Backend.App.Layers.Models;

namespace Velochat.Backend.App.Layers.Infrastructure;

public class FriendshipRepository(NpgsqlDataSource dataSource) : IFriendshipRepository
{
    public async Task<CompleteFriendRequest?> GetFriendRequestAsync(int senderId, int recipientId)
    {
        var query = dataSource.CreateCommand(@"
            SELECT sender_id, recipient_id
            FROM friend_requests
            WHERE sender_id = @senderId AND recipient_id = @recipientId;"
        );
        query.Parameters.AddWithValue("senderId", senderId);
        query.Parameters.AddWithValue("recipientId", recipientId);

        await using var reader = await query.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return new CompleteFriendRequest
            {
                SenderId = reader.GetInt32(0),
                RecipientId = reader.GetInt32(1)
            };
        }
        return null;
    }

    public async Task<IReadOnlyList<CompleteIdentity>> GetFriendRequestSendersAsync(int identityId)
    {
        var query = dataSource.CreateCommand(@"
            SELECT u.id, u.login
            FROM friend_requests r
            INNER JOIN identities u ON u.id = r.recipient_id
            WHERE recipient_id = @identityId;"
        );
        query.Parameters.AddWithValue("identityId", identityId);
        await using var reader = await query.ExecuteReaderAsync();
        var friends = new List<CompleteIdentity>();
        while (await reader.ReadAsync())
        {
            friends.Add(new CompleteIdentity
            {
                Id = reader.GetInt32(0),
                Login = reader.GetString(1)
            });
        }
        return friends;
    }

    public async Task<IReadOnlyList<CompleteIdentity>> GetFriendshipsAsync(int identityId)
    {
        throw new NotImplementedException();
    }

    public async Task<CompleteIdentity?> CreateFriendRequestAsync(FriendRequest friendRequest)
    {
        throw new NotImplementedException();
    }

    public async Task<CompleteIdentity?> AcceptFriendRequestAsync(FriendRequest friendRequest)
    {
        throw new NotImplementedException();
    }
}