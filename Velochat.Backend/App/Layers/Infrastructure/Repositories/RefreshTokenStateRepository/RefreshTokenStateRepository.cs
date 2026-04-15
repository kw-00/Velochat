using Npgsql;
using Velochat.Backend.App.Layers.Models;

namespace Velochat.Backend.App.Layers.Infrastructure;

public class RefreshTokenStateRepository(NpgsqlDataSource dataSource) : IRefreshTokenStateRepository
{
    public async Task<CompleteRefreshTokenState?> GetByTokenAsync(string token)
    {
        var query = dataSource.CreateCommand(@"
            SELECT token, status 
            FROM refresh_token_state 
            WHERE token = @token;
        ");
        query.Parameters.AddWithValue("token", token);
        await using var reader = await query.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return new CompleteRefreshTokenState
            {
                Token = reader.GetString(0),
                Status = reader.GetString(1)
            };
        }
        return null;
    }

    public async Task CreateAsync(RefreshTokenState refreshTokenState)
    {
        refreshTokenState.EnsureInsertable();
        var query = dataSource.CreateCommand(@"
            INSERT INTO refresh_token_state (token, status) 
            VALUES (@token, @status);
        ");
        query.Parameters.AddWithValue("token", refreshTokenState.Token);
        query.Parameters.AddWithValue("status", refreshTokenState.Status);
        await query.ExecuteNonQueryAsync();
    }

    public async Task UpdateAsync(CompleteRefreshTokenState refreshTokenState)
    {
        var query = dataSource.CreateCommand(@"
            UPDATE refresh_token_state 
            SET status = @status 
            WHERE token = @token;
        ");
        query.Parameters.AddWithValue("token", refreshTokenState.Token);
        query.Parameters.AddWithValue("status", refreshTokenState.Status);
        await query.ExecuteNonQueryAsync();
    }

    public async Task RevokeAsync(string token)
    {
        var query = dataSource.CreateCommand(@"
            UPDATE refresh_token_state
            SET status = 'revoked'
            WHERE token = @token;
        ");
        query.Parameters.AddWithValue("token", token);
        await query.ExecuteNonQueryAsync();
    }
    public async Task RevokeByIdentityIdAsync(int identityId)
    {
        var query = dataSource.CreateCommand(@"
            UPDATE refresh_token_state
            SET status = 'revoked'
            WHERE identity_id = @identityId;
        ");
        query.Parameters.AddWithValue("identityId", identityId);
        await query.ExecuteNonQueryAsync();
    }
}
