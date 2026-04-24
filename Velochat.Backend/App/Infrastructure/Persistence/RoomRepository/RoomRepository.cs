using Npgsql;
using Velochat.Backend.App.Infrastructure.Persistence;
using Velochat.Backend.App.Infrastructure.Models;

namespace Velochat.Backend.App.Infrastructure.Persistence;

public class RoomRepository(NpgsqlDataSource dataSource) : IRoomRepository
{
    public async Task<CompleteRoom?> GetByIdAsync(int roomId)
    {
        var query = dataSource.CreateCommand(@"
            SELECT id, name FROM rooms WHERE id = @roomId;
        ");
        query.Parameters.AddWithValue("roomId", roomId);
        await using var reader = await query.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return new CompleteRoom
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1),
            };
        }
        return null;
    }

    public async Task<IReadOnlyList<CompleteRoom>> GetByMemberIdAsync(int userId)
    {
        dataSource.CreateConnection();
        var query = dataSource.CreateCommand(@"
            SELECT id, name FROM rooms r
            INNER JOIN room_presences rp ON rp.room_id = r.id
            WHERE rp.user_id = @userId;
        ");
        query.Parameters.AddWithValue("userId", userId);
        await using var reader = await query.ExecuteReaderAsync();
        var rooms = new List<CompleteRoom>();
        while (await reader.ReadAsync())
        {
            rooms.Add(new CompleteRoom
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1),
            });
        }
        return rooms;
    }


    public async Task<CompleteRoom> CreateAsync(Room room, int userId)
    {
        room.EnsureInsertable();
        var query = dataSource.CreateCommand(@"
            WITH inserted_room AS (
                INSERT INTO rooms (name) VALUES (@name) RETURNING id, name
            ),
            inserted_room_presences AS (
                INSERT INTO room_presences (room_id, user_id) 
                SELECT id, @userId
                FROM inserted_room
            )
            SELECT id, name FROM inserted_room;
        ");
        query.Parameters.AddWithValue("name", room.Name);
        query.Parameters.AddWithValue("userId", userId);
        try
        {
            await using var reader = await query.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new CompleteRoom
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                };
            }
            else throw new MissingInsertionReturnValue();
        }
        catch (PostgresException ex) 
        when (
            ex.SqlState == PostgresErrorCodes.ForeignKeyViolation
            && ex.ConstraintName == "fk_room_presences_user_id"
        )
        {
            throw new IdentifierNotFoundException<User>("User", userId);
        }
    }


    public async Task DeleteAsync(int roomId)
    {
        var query = dataSource.CreateCommand(@"
            DELETE FROM rooms WHERE id = @roomId;
        ");
        query.Parameters.AddWithValue("roomId", roomId);
        await query.ExecuteNonQueryAsync();
    }


}