using Npgsql;
using Velochat.Backend.App.Layers.Infrastructure.RepositoryExceptions;
using Velochat.Backend.App.Layers.Models;

namespace Velochat.Backend.App.Layers.Infrastructure;

public class RefreshTokenStateRepository(NpgsqlDataSource dataSource) : IRefreshTokenStateRepository
{
    public async Task<CompleteRefreshTokenState?> GetByTokenAsync(string token)
    {
        var query = dataSource.CreateCommand(@"
            SELECT token, identity_id, status 
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
                IdentityId = reader.GetInt32(1),
                Status = reader.GetString(2)
            };
        }
        return null;
    }

    public async Task CreateAsync(RefreshTokenState refreshTokenState)
    {
        refreshTokenState.EnsureInsertable();
        var query = dataSource.CreateCommand(@"
            INSERT INTO refresh_token_states (token, identity_id, status) 
            VALUES (@token, @identityId, @status);
        ");
        query.Parameters.AddWithValue("token", refreshTokenState.Token);
        query.Parameters.AddWithValue("identityId", refreshTokenState.IdentityId);
        query.Parameters.AddWithValue("status", refreshTokenState.Status);
        try
        {
            await query.ExecuteNonQueryAsync();
        }
        catch (PostgresException ex)
        when (ex.SqlState == PostgresErrorCodes.UniqueViolation)
        {
            throw new DuplicatePrimaryKeyException<RefreshTokenState>(refreshTokenState);
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
    
    public async Task RevokeByIdentityIdAsync(int identityId)
    {
        var query = dataSource.CreateCommand(@"
            UPDATE refresh_token_states
            SET status = 'revoked'
            WHERE identity_id = @identityId;
        ");
        query.Parameters.AddWithValue("identityId", identityId);
        await query.ExecuteNonQueryAsync();
    }
}
