using Npgsql;
using Velochat.Backend.App.Layers.Infrastructure.RepositoryExceptions;
using Velochat.Backend.App.Layers.Models;

namespace Velochat.Backend.App.Layers.Infrastructure;

public class ChatMessageRepository(NpgsqlDataSource dataSource)
{

    public async Task<IReadOnlyList<CompleteChatMessage>> GetByRoomIdAsync(int roomId, int limit)
    {
        var query = dataSource.CreateCommand(@"
            SELECT id, room_id, author_id, content 
            FROM chat_messages 
            WHERE room_id = @roomId 
            ORDER BY id DESC 
            LIMIT @limit;
        ");
        query.Parameters.AddWithValue("roomId", roomId);
        query.Parameters.AddWithValue("limit", limit);
        await using var reader = await query.ExecuteReaderAsync();
        var messages = new List<CompleteChatMessage>();
        while (await reader.ReadAsync())
        {
            messages.Add(new CompleteChatMessage
            {
                Id = reader.GetInt32(0),
                RoomId = reader.GetInt32(1),
                AuthorId = reader.GetInt32(2),
                Content = reader.GetString(3),
            });
        }
        return messages;
    }


    public async Task<IReadOnlyList<CompleteChatMessage>> GetByRoomIdAfterAsync(
        int roomId, int after, int limit
    )
    {
        var query = dataSource.CreateCommand(@"
            SELECT id, room_id, author_id, content 
            FROM chat_messages 
            WHERE room_id = @roomId AND id > @after 
            ORDER BY id ASC 
            LIMIT @limit;
        ");
        query.Parameters.AddWithValue("roomId", roomId);
        query.Parameters.AddWithValue("after", after);
        query.Parameters.AddWithValue("limit", limit);
        await using var reader = await query.ExecuteReaderAsync();
        var messages = new List<CompleteChatMessage>();
        while (await reader.ReadAsync())
        {
            messages.Add(new CompleteChatMessage
            {
                Id = reader.GetInt32(0),
                RoomId = reader.GetInt32(1),
                AuthorId = reader.GetInt32(2),
                Content = reader.GetString(3),
            });
        }
        return messages;
    }



    public async Task<IReadOnlyList<CompleteChatMessage>> GetByRoomIdBeforeAsync(
        int roomId, int before, int limit
    )
    {
        var query = dataSource.CreateCommand(@"
            SELECT id, room_id, author_id, content 
            FROM chat_messages 
            WHERE room_id = @roomId AND id < @before 
            ORDER BY id DESC 
            LIMIT @limit;
        ");
        query.Parameters.AddWithValue("roomId", roomId);
        query.Parameters.AddWithValue("before", before);
        query.Parameters.AddWithValue("limit", limit);
        await using var reader = await query.ExecuteReaderAsync();
        var messages = new List<CompleteChatMessage>();
        while (await reader.ReadAsync())
        {
            messages.Add(new CompleteChatMessage
            {
                Id = reader.GetInt32(0),
                RoomId = reader.GetInt32(1),
                AuthorId = reader.GetInt32(2),
                Content = reader.GetString(3),
            });
        }
        return messages;
    }

    public async Task<CompleteChatMessage> CreateAsync(ChatMessage message)
    {
        message.EnsureInsertable();
        var query = dataSource.CreateCommand(@"
            INSERT INTO chat_messages (room_id, author_id, content) 
            VALUES (@roomId, @authorId, @content) 
            RETURNING id, room_id, author_id, content;
        ");
        query.Parameters.AddWithValue("roomId", message.RoomId);
        query.Parameters.AddWithValue("authorId", message.AuthorId);
        query.Parameters.AddWithValue("content", message.Content);
        try
        {
            await using var reader = await query.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {  
                return new CompleteChatMessage
                {
                    Id = reader.GetInt32(0),
                    RoomId = reader.GetInt32(1),
                    AuthorId = reader.GetInt32(2),
                    Content = reader.GetString(3),
                };
            }
            throw new MissingInsertionReturnValue();
        }
        catch (PostgresException ex)
        {
            if (ex.SqlState == PostgresErrorCodes.ForeignKeyViolation)
            {
                if (ex.ConstraintName == "fk_chat_messages_room_id")
                {
                    throw new IdentifierNotFoundException<Room>(
                        null, message.RoomId
                    );
                }
                if (ex.ConstraintName == "fk_chat_messages_author_id")
                {
                    throw new IdentifierNotFoundException<Identity>(
                        "Author", message.AuthorId
                    );
                }
            }
            throw;
        }
    }
}