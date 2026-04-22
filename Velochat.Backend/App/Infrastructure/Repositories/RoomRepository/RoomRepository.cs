using Npgsql;
using Velochat.Backend.App.Infrastructure.Repositories;
using Velochat.Backend.App.Infrastructure.Models;

namespace Velochat.Backend.App.Infrastructure.Repositories;

public class RoomRepository(NpgsqlDataSource dataSource) : IRoomRepository
{
    public async Task<CompleteRoom?> GetByIdAsync(int roomId)
    {
        var query = dataSource.CreateCommand(@"
            SELECT id, name, owner_id FROM rooms WHERE id = @roomId;
        ");
        query.Parameters.AddWithValue("roomId", roomId);
        await using var reader = await query.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return new CompleteRoom
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1),
                OwnerId = reader.GetInt32(2)
            };
        }
        return null;
    }

    public async Task<IReadOnlyList<CompleteRoom>> GetByMemberIdAsync(int userId)
    {
        var query = dataSource.CreateCommand(@"
            SELECT id, name, owner_id FROM rooms r
            INNER JOIN room_presences rp ON rp.room_id = r.id
            WHERE rp.member_id = @userId;
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
                OwnerId = reader.GetInt32(2)
            });
        }
        return rooms;
    }

    public async Task<IReadOnlyList<CompleteRoom>> GetByInviteeIdAsync(int userId)
    {
        var query = dataSource.CreateCommand(@"
            SELECT id, name, owner_id FROM rooms r
            INNER JOIN invitation i ON i.room_id = r.id
            WHERE i.invitee_id = @userId;
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
                OwnerId = reader.GetInt32(2)
            });
        }
        return rooms;
    }

    public async Task<CompleteRoom> CreateAsync(Room room)
    {
        room.EnsureInsertable();
        var query = dataSource.CreateCommand(@"
            INSERT INTO rooms (name, owner_id) VALUES (@name, @ownerId) RETURNING id, name, owner_id;
        ");
        query.Parameters.AddWithValue("name", room.Name);
        query.Parameters.AddWithValue("ownerId", room.OwnerId);
        try
        {
            await using var reader = await query.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new CompleteRoom
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    OwnerId = reader.GetInt32(2)
                };
            }
            else throw new MissingInsertionReturnValue();
        }
        catch (PostgresException ex) 
        when (
            ex.SqlState == PostgresErrorCodes.UniqueViolation
            && ex.ConstraintName == "unique_owner_and_name"
        )
        {
            throw new DuplicateRoomPathException(room);
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