using Npgsql;
using Velochat.Backend.App.Layers.Infrastructure;
using Velochat.Backend.App.Layers.Infrastructure.RepositoryExceptions;
using Velochat.Backend.App.Layers.Models;

namespace Velochat.Backend.App.Layers.Infrastructure;

public class RoomPresenceRepository(NpgsqlDataSource dataSource) : IRoomPresenceRepository
{

    public async Task <CompleteRoomPresence?> GetAsync(RoomPresence roomPresence)
    {
        roomPresence.EnsureIdentifiable();
        var query = dataSource.CreateCommand(@"
            SELECT room_id, member_id 
            FROM room_presences 
            WHERE room_id = @roomId AND member_id = @memberId;
        ");
        query.Parameters.AddWithValue("roomId", roomPresence.RoomId);
        query.Parameters.AddWithValue("memberId", roomPresence.MemberId);
        await using var reader = await query.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return new CompleteRoomPresence
            {
                RoomId = reader.GetInt32(0),
                MemberId = reader.GetInt32(1)
            };
        }
        return null;
    }
    

    public async Task<CompleteRoomPresence> CreateAsync(RoomPresence roomPresence)
    {
        roomPresence.EnsureInsertable();
        var query = dataSource.CreateCommand(@"
            INSERT INTO room_presences (room_id, member_id) 
            VALUES (@roomId, @memberId) 
            RETURNING room_id, member_id;
        ");
        query.Parameters.AddWithValue("roomId", roomPresence.RoomId);
        query.Parameters.AddWithValue("memberId", roomPresence.MemberId);
        try
        {
            await using var reader = await query.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new CompleteRoomPresence
                {
                    RoomId = reader.GetInt32(0),
                    MemberId = reader.GetInt32(1)
                };
            }
            throw new MissingInsertionReturnValue();
        }
        catch (PostgresException ex)
        when (ex.SqlState == PostgresErrorCodes.UniqueViolation)
        {
            throw new DuplicatePrimaryKeyException<RoomPresence>(roomPresence);
        }

    }

    public async Task DeleteAsync(RoomPresence roomPresence)
    {
        roomPresence.EnsureIdentifiable();
        var query = dataSource.CreateCommand(@"
            DELETE FROM room_presences 
            WHERE room_id = @roomId AND member_id = @memberId;
        ");
        query.Parameters.AddWithValue("roomId", roomPresence.RoomId);
        query.Parameters.AddWithValue("memberId", roomPresence.MemberId);
        await query.ExecuteNonQueryAsync();
    }
}