using Npgsql;
using Velochat.Backend.App.Exceptions.RepositoryExceptions;
using Velochat.Backend.App.Layers.Models;

namespace Velochat.Backend.App.Layers.Infrastructure;

public class IdentityRepository(NpgsqlDataSource dataSource) : IIdentityRepository
{
    public async Task<CompleteIdentity?> GetByIdAsync(int id)
    {
        var query = dataSource.CreateCommand(@"
            SELECT id, login FROM identity WHERE id = @id;
        ");
        query.Parameters.AddWithValue("id", id);
        await using var reader = await query.ExecuteReaderAsync();
        if (await reader.ReadAsync()) {
            return new CompleteIdentity
            {
                Id = reader.GetInt32(0),
                Login = reader.GetString(1),
            };
        }
        return null;
    }
    public async Task<CompleteIdentity?> GetByCredentialsAsync(
        string login, string passwordHash
    )
    {
        var query = dataSource.CreateCommand(@"
            SELECT id, login FROM identity 
            WHERE login = @login AND password_hash = @passwordHash;
        ");
        query.Parameters.AddWithValue("login", login);
        query.Parameters.AddWithValue("passwordHash", passwordHash);
        await using var reader = await query.ExecuteReaderAsync();
        if (await reader.ReadAsync()) {
            return new CompleteIdentity
            {
                Id = reader.GetInt32(0),
                Login = reader.GetString(1),
            };
        }
        return null;
    }
    public async Task<CompleteIdentity> CreateAsync(Identity identity)
    {
        try
        { 
            identity.EnsureInsertable();
            var query = dataSource.CreateCommand(@"
                INSERT INTO identity (login, password_hash) 
                VALUES (@login, @passwordHash) 
                RETURNING id, login;
            ");
            query.Parameters.AddWithValue("login", identity.Login);
            query.Parameters.AddWithValue("passwordHash", identity.PasswordHash);

            await using var reader = await query.ExecuteReaderAsync();
            await reader.ReadAsync();
            return new CompleteIdentity
            {
                Id = reader.GetInt32(0),
                Login = reader.GetString(1),
            };
        }
        catch (NpgsqlException ex)
        {
            if (ex.SqlState == PostgresErrorCodes.UniqueViolation)
                throw new DuplicatePrimaryKeyException<Identity>(identity);
            
            throw;
        }
    }
}
