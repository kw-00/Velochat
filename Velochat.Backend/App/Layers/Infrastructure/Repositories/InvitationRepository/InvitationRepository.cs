using Npgsql;
using Velochat.Backend.App.Layers.DTOs;
using Velochat.Backend.App.Layers.Infrastructure;
using Velochat.Backend.App.Layers.Infrastructure.RepositoryExceptions;
using Velochat.Backend.App.Layers.Models;

namespace Velochat.Backend.App.Layers.Infrastructure;

public class InvitationRepository(NpgsqlDataSource dataSource) : IInvitationRepository
{

    public async Task<CompleteInvitation?> GetAsync(Invitation invitation)
    {
        invitation.EnsureIdentifiable();
        var query = dataSource.CreateCommand(@"
            SELECT room_id, invitee_id 
            FROM invitations
            WHERE room_id = @roomId AND invitee_id = @inviteeId;
        ");
        query.Parameters.AddWithValue("roomId", invitation.RoomId);
        query.Parameters.AddWithValue("inviteeId", invitation.InviteeId);
        await using var reader = await query.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return new CompleteInvitation
            {
                RoomId = reader.GetInt32(0),
                InviteeId = reader.GetInt32(1)
            };
        }
        return null;
    }

    public async Task<List<CompleteInvitation>> GetAsync(int identityId)
    {
        var query = dataSource.CreateCommand(@"
            SELECT room_id, invitee_id 
            FROM invitation 
            WHERE invitee_id = @inviteeId;
        ");
        query.Parameters.AddWithValue("inviteeId", identityId);
        await using var reader = await query.ExecuteReaderAsync();
        var invitations = new List<CompleteInvitation>();
        while (await reader.ReadAsync())
        {
            invitations.Add(new CompleteInvitation
            {
                RoomId = reader.GetInt32(0),
                InviteeId = reader.GetInt32(1)
            });
        }
        return invitations;
    }

    public async Task<CompleteInvitation> CreateAsync(Invitation invitation)
    {
        invitation.EnsureInsertable();
        var query = dataSource.CreateCommand(@"
            INSERT INTO invitations (room_id, invitee_id) 
            VALUES (@roomId, @inviteeId) 
            RETURNING room_id, invitee_id;
        ");
        query.Parameters.AddWithValue("roomId", invitation.RoomId);
        query.Parameters.AddWithValue("inviteeId", invitation.InviteeId);
        try
        {
            await using var reader = await query.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new CompleteInvitation
                {
                    RoomId = reader.GetInt32(0),
                    InviteeId = reader.GetInt32(1)
                };
            }
            throw new MissingInsertionReturnValue();
        }
        catch (PostgresException ex)
        when (ex.SqlState == PostgresErrorCodes.UniqueViolation)
        {
            throw new DuplicatePrimaryKeyException<Invitation>(invitation);
        }
    }

    public async Task DeleteAsync(Invitation invitation)
    {
        invitation.EnsureIdentifiable();
        var query = dataSource.CreateCommand(@"
            DELETE FROM invitations 
            WHERE room_id = @roomId AND invitee_id = @inviteeId;
        ");
        query.Parameters.AddWithValue("roomId", invitation.RoomId);
        query.Parameters.AddWithValue("inviteeId", invitation.InviteeId);
        await query.ExecuteNonQueryAsync();
    }

    public async Task<FullInvitationDTO?> GetFullInvitationDataAsync(Invitation invitation)
    {
        invitation.EnsureIdentifiable();
        var query = dataSource.CreateCommand(@"
            SELECT r.id, r.name, sub.id, sub.login, o.id, o.login
            FROM invitations i
            INNER JOIN rooms r ON r.id = i.room_id
            INNER JOIN identities sub ON sub.id = i.invitee_id
            INNER JOIN identities o ON o.id = r.owner_id
            WHERE i.room_id = @roomId AND i.invitee_id = @inviteeId;
        ");
        query.Parameters.AddWithValue("roomId", invitation.RoomId);
        query.Parameters.AddWithValue("inviteeId", invitation.InviteeId);
        await using var reader = await query.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return new FullInvitationDTO
            {
                RoomId = reader.GetInt32(0),
                RoomName = reader.GetString(1),
                InviteeId = reader.GetInt32(2),
                InviteeLogin = reader.GetString(3),
                RoomOwnerId = reader.GetInt32(4),
                RoomOwnerLogin = reader.GetString(5)
            };
        }
        return null;
    }

    public async Task<IReadOnlyList<FullInvitationDTO>> GetFullInvitationDataAsync(int inviteeId)
    {
        var query = dataSource.CreateCommand(@"
            SELECT r.id, r.name, sub.id, sub.login, o.id, o.login
            FROM invitations i
            INNER JOIN rooms r ON r.id = i.room_id
            INNER JOIN identities sub ON sub.id = i.invitee_id
            INNER JOIN identities o ON o.id = r.owner_id
            WHERE i.invitee_id = @inviteeId;
        ");
        query.Parameters.AddWithValue("inviteeId", inviteeId);

        var invitationDTOs = new List<FullInvitationDTO>();
        await using var reader = await query.ExecuteReaderAsync();
        while (await reader.ReadAsync()) 
        {
            invitationDTOs.Add(new FullInvitationDTO
            {
                RoomId = reader.GetInt32(0),
                RoomName = reader.GetString(1),
                InviteeId = reader.GetInt32(2),
                InviteeLogin = reader.GetString(3),
                RoomOwnerId = reader.GetInt32(4),
                RoomOwnerLogin = reader.GetString(5)
            });
        }

        return invitationDTOs;
    }
}