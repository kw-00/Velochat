using Npgsql;
using Velochat.Backend.App.Infrastructure.Persistence;
using Velochat.Backend.App.Infrastructure.Models;

namespace Velochat.Backend.App.Infrastructure.Persistence;

public class RefreshTokenStateRepository(NpgsqlDataSource dataSource) : IRefreshTokenStateRepository
{
    public async Task<CompleteRefreshTokenState?> GetByTokenAsync(string token)
    {
        var query = dataSource.CreateCommand(@"
            SELECT token, user_id, status 
            FROM refresh_token_states 
            WHERE token = @token;
        ");
        query.Parameters.AddWithValue("token", token);
        await using var reader = await query.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return new CompleteRefreshTokenState
            {
                Token = reader.GetString(0),
                UserId = reader.GetInt32(1),
                Status = reader.GetString(2)
            };
        }
        return null;
    }

    public async Task CreateAsync(RefreshTokenState refreshTokenState)
    {
        refreshTokenState.EnsureInsertable();
        var query = dataSource.CreateCommand(@"
            INSERT INTO refresh_token_states (token, user_id, status) 
            VALUES (@token, @userId, @status);
        ");
        query.Parameters.AddWithValue("token", refreshTokenState.Token);
        query.Parameters.AddWithValue("userId", refreshTokenState.UserId);
        query.Parameters.AddWithValue("status", refreshTokenState.Status);
        try
        {
            await query.ExecuteNonQueryAsync();
        }
        catch (PostgresException ex)
        {
            if (ex.SqlState == PostgresErrorCodes.UniqueViolation)
            {
                throw new DuplicatePrimaryKeyException<RefreshTokenState>(refreshTokenState);
            }
            if (ex.SqlState == PostgresErrorCodes.ForeignKeyViolation)
            {
                throw new IdentifierNotFoundException<User>("user", refreshTokenState.UserId);
            }
            throw;
        }
    }

    public async Task UpdateAsync(CompleteRefreshTokenState refreshTokenState)
    {
        var query = dataSource.CreateCommand(@"
            UPDATE refresh_token_states 
            SET status = @status 
            WHERE token = @token;
        ");
        query.Parameters.AddWithValue("token", refreshTokenState.Token);
        query.Parameters.AddWithValue("status", refreshTokenState.Status);

        try
        {
            await query.ExecuteNonQueryAsync();
        }
        catch (PostgresException ex)
        when (ex.ConstraintName == "refresh_token_states_status_check")
        {
            throw new DatabaseCheckException("Status must be 'active', 'used' or 'revoked'");
        }
    }

    public async Task RevokeAsync(string token)
    {
        var query = dataSource.CreateCommand(@"
            UPDATE refresh_token_states 
            SET status = 'revoked'
            WHERE token = @token;
        ");
        query.Parameters.AddWithValue("token", token);
        await query.ExecuteNonQueryAsync();
    }
    
    public async Task RevokeByUserIdAsync(int userId)
    {
        var query = dataSource.CreateCommand(@"
            UPDATE refresh_token_states
            SET status = 'revoked'
            WHERE user_id = @userId;
        ");
        query.Parameters.AddWithValue("userId", userId);
        await query.ExecuteNonQueryAsync();
    }
}
