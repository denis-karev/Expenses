using Dapper;
using Expenses.Api.Database.Repositories;
using Expenses.Api.Model;
using Npgsql;

namespace Expenses.Api.Database.Postgres.Repositories;

public sealed class GroupRepository(NpgsqlConnection connection) : IGroupRepository
{
    public async Task CreateAsync(GroupInfo info)
    {
        const String sql = """
                           INSERT INTO groups (id, name, currency, created_at)
                           VALUES (@Id, @Name, @Currency, @CreatedAt)
                           """;
        await connection.ExecuteAsync(sql, info);
    }

    public async Task<GroupInfo?> FindAsync(Guid id)
    {
        const String sql = """
                           SELECT * FROM groups WHERE id = @Id;
                           """;
        return await connection.QuerySingleOrDefaultAsync<GroupInfo>(sql, new { Id = id });
    }
}