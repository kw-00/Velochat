using Npgsql;
using Velochat.Backend.App.Infrastructure.Persistence;
using Velochat.Backend.App.Infrastructure.Models;

namespace Velochat.Backend.App.Infrastructure.Persistence;

public class FriendshipRepository(NpgsqlDataSource dataSource) : IFriendshipRepository
{
    public async Task<CompleteFriendship?> GetFriendshipAsync(int firstUserId, int secondUserId)
    {
        var query = dataSource.CreateCommand(@" async
            SELECT initiator_id, receiver_id, accepted FROM friendships 
            WHERE
                (initiator_id = @firstUserId AND receiver_id = @secondUserId)
                OR
                (initiator_id = @secondUserId AND receiver_id = @firstUserId)
            ;
        ");
        query.Parameters.AddWithValue("firstUserId", firstUserId);
        query.Parameters.AddWithValue("secondUserId", secondUserId);

        await using var reader = await query.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return new CompleteFriendship
            {
                InitiatorId = reader.GetInt32(1),
                ReceiverId = reader.GetInt32(2),
                Accepted = reader.GetBoolean(3)
            };
        }
        return null;
    }

    public async Task<IReadOnlyList<CompleteUser>> GetPendingInitiatorsAsync(int userId)
    {
        var query = dataSource.CreateCommand(@"
            SELECT u.id, u.login FROM friendships f
            INNER JOIN users u ON u.id = f.initiator_id
            WHERE f.accepted = false AND f.receiver_id = @userId;
        ");
        query.Parameters.AddWithValue("userId", userId);
        await using var reader = await query.ExecuteReaderAsync();
        var pendingInitiators = new List<CompleteUser>();
        while (await reader.ReadAsync())
        {
            pendingInitiators.Add(new CompleteUser
            {
                Id = reader.GetInt32(0),
                Login = reader.GetString(1),
            });
        }
        return pendingInitiators;
    }

    public async Task<IReadOnlyList<CompleteUser>> GetFriendsAsync(int userId)
    {
        var query = dataSource.CreateCommand(@"
            SELECT u.id, u.login FROM friendships f
            INNER JOIN users u 
                ON u.id = f.initiator_id OR u.id = f.receiver_id
            WHERE 
                f.accepted = true 
                AND f.initiator_id = @userId
                AND (f.receiver_id != @userId OR f.initiator_id != @userId)
            ;
        ");
        query.Parameters.AddWithValue("userId", userId);
        await using var reader = await query.ExecuteReaderAsync();
        var friends = new List<CompleteUser>();
        while (await reader.ReadAsync())
        {
            friends.Add(new CompleteUser
            {
                Id = reader.GetInt32(0),
                Login = reader.GetString(1),
            });
        }
        return friends;
    }

    public async Task<CompleteUser> CreatePendingAsync(int initiatorId, int receiverId)
    {
        var batch = dataSource.CreateBatch();
        var insertionQuery = batch.CreateBatchCommand();
        insertionQuery.CommandText = (@"
            INSERT INTO friendships(initiator_id, receiver_id, accepted) 
            VALUES (@initiatorId, @receiverId, false);
        ");
        batch.BatchCommands.Add(insertionQuery);
        insertionQuery.Parameters.AddWithValue("initiatorId", initiatorId);
        insertionQuery.Parameters.AddWithValue("receiverId", receiverId);

        var retrievalQuery = batch.CreateBatchCommand();
        retrievalQuery.CommandText = (@"
            SELECT id, login From users WHERE id = @receiverId;
        ");
        batch.BatchCommands.Add(retrievalQuery);
        retrievalQuery.Parameters.AddWithValue("receiverId", receiverId);

        try
        {
            await using var reader = await batch.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new CompleteUser
                {
                    Id = reader.GetInt32(0),
                    Login = reader.GetString(1),
                };
            }
            else throw new IdentifierNotFoundException<User>("receiver", receiverId);
        }
        catch (PostgresException e)
        {
            if (e.SqlState == PostgresErrorCodes.ForeignKeyViolation)
            {
                if (e.ConstraintName == "fk_friendships_initiator_id")
                    throw new IdentifierNotFoundException<User>("initiator", initiatorId);
                else if (e.ConstraintName == "fk_friendships_receiver_id")
                    throw new IdentifierNotFoundException<User>("receiver", receiverId);
            }
            if (e.SqlState == PostgresErrorCodes.UniqueViolation)
            {
                if (e.ConstraintName == "unique_friendship")
                    throw new DuplicatePrimaryKeyException<Friendship>(new Friendship
                    {
                        InitiatorId = initiatorId,
                        ReceiverId = receiverId
                    });
            }
            throw;
        }
    }

    public async Task<CompleteUser> AcceptAsync(Friendship friendRequest)
    {
        friendRequest.EnsureIdentifiable();
        var query = dataSource.CreateCommand(@"
            WITH updated AS (
                UPDATE friendships 
                SET accepted = true 
                WHERE initiator_id = @initiatorId AND receiver_id = @receiverId
                RETURNING initiator_id, receiver_id
            )
            SELECT id, login FROM users
            CROSS JOIN updated;
        ");

        query.Parameters.AddWithValue("initiatorId", friendRequest.InitiatorId);
        query.Parameters.AddWithValue("receiverId", friendRequest.ReceiverId);

        await using var reader = await query.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return new CompleteUser
            {
                Id = reader.GetInt32(0),
                Login = reader.GetString(1),
            };
        }
        else throw new IdentifierNotFoundException<Friendship>(friendRequest);
    }

    public async Task DeleteAsync(int firstUserId, int secondUserId)
    {
        var query = dataSource.CreateCommand(@"
            DELETE FROM friendships 
            WHERE
                (initiator_id = @firstUserId AND receiver_id = @secondUserId)
                OR
                (initiator_id = @secondUserId AND receiver_id = @firstUserId)
            ;
        ");
        query.Parameters.AddWithValue("firstUserId", firstUserId);
        query.Parameters.AddWithValue("secondUserId", secondUserId);
        await query.ExecuteNonQueryAsync();
    }

    public async Task DeleteAsync(Friendship friendship)
    {
        friendship.EnsureIdentifiable();
        var query = dataSource.CreateCommand(@"
            DELETE FROM friendships 
            WHERE
                initiator_id = @initiatorId 
                AND receiver_id = @receiverId
            ;
        ");
        query.Parameters.AddWithValue("initiatorId", friendship.InitiatorId);
        query.Parameters.AddWithValue("receiverId", friendship.ReceiverId);
        await query.ExecuteNonQueryAsync();
    }
}