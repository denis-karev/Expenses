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

    public async Task<IReadOnlyDictionary<Guid, GroupMemberInfo?>> FindByIdsInGroupAsync(Guid groupId, ICollection<Guid> ids)
    {
        const String sql = """
                           SELECT * FROM group_members
                           WHERE group_id = @GroupId AND id = ANY(@Ids);
                           """;
        var results = (await connection.QueryAsync<GroupMemberInfo>(sql, new { GroupId = groupId, Ids = ids })).ToHashSet();
        return ids.ToDictionary(x => x, x => results.FirstOrDefault(y => y.Id == x));
    }

    public async Task<Boolean> IsGroupMemberAsync(Guid groupId, Guid userId)
    {
        const String sql = """
                           SELECT EXISTS(SELECT 1 FROM group_members WHERE group_id = @GroupId AND user_id = @UserId);
                           """;
        return await connection.QuerySingleAsync<Boolean>(sql, new { GroupId = groupId, UserId = userId });
    }
}