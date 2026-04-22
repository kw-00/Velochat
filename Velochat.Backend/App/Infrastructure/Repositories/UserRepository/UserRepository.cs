using Npgsql;
using Velochat.Backend.App.Infrastructure.Models;

namespace Velochat.Backend.App.Infrastructure.Repositories;

public class UserRepository(NpgsqlDataSource dataSource) : IUserRepository
{
    public async Task<CompleteUser?> GetByIdAsync(int id)
    {
        var query = dataSource.CreateCommand(@"
            SELECT id, login FROM users WHERE id = @id;
        ");
        query.Parameters.AddWithValue("id", id);
        await using var reader = await query.ExecuteReaderAsync();
        if (await reader.ReadAsync()) {
            return new CompleteUser
            {
                Id = reader.GetInt32(0),
                Login = reader.GetString(1),
            };
        }
        return null;
    }
    public async Task<CompleteUser?> GetByCredentialsAsync(
        string login, string passwordHash
    )
    {
        var query = dataSource.CreateCommand(@"
            SELECT id, login FROM users
            WHERE login = @login AND password_hash = @passwordHash;
        ");
        query.Parameters.AddWithValue("login", login);
        query.Parameters.AddWithValue("passwordHash", passwordHash);
        await using var reader = await query.ExecuteReaderAsync();
        if (await reader.ReadAsync()) {
            return new CompleteUser
            {
                Id = reader.GetInt32(0),
                Login = reader.GetString(1),
            };
        }
        return null;
    }
    public async Task<CompleteUser> CreateAsync(User user)
    {
        try
        { 
            user.EnsureInsertable();
            var query = dataSource.CreateCommand(@"
                INSERT INTO users (login, password_hash) 
                VALUES (@login, @passwordHash) 
                RETURNING id, login;
            ");
            query.Parameters.AddWithValue("login", user.Login);
            query.Parameters.AddWithValue("passwordHash", user.PasswordHash);

            await using var reader = await query.ExecuteReaderAsync();
            await reader.ReadAsync();
            return new CompleteUser
            {
                Id = reader.GetInt32(0),
                Login = reader.GetString(1),
            };
        }
        catch (PostgresException ex)
        when (ex.SqlState == PostgresErrorCodes.UniqueViolation)
        {
            throw new DuplicatePrimaryKeyException<User>(user);
        }
    }
}
