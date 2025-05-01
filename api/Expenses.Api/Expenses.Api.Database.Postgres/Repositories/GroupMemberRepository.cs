using Dapper;
using Expenses.Api.Database.Repositories;
using Expenses.Api.Model;
using Npgsql;

namespace Expenses.Api.Database.Postgres.Repositories;

public sealed class GroupMemberRepository(NpgsqlConnection connection) : IGroupMemberRepository
{
    public async Task CreateAsync(GroupMemberInfo info)
    {
        const String sql = """
                           INSERT INTO group_members (id, group_id, user_id, name, joined_at)
                           VALUES (@Id, @GroupId, @UserId, @Name, @JoinedAt);        
                           """;

        await connection.ExecuteAsync(sql, info);
    }
}