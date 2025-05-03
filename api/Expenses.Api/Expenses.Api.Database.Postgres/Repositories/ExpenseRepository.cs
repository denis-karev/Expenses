using Dapper;
using Expenses.Api.Database.Repositories;
using Expenses.Api.Model;
using Npgsql;

namespace Expenses.Api.Database.Postgres.Repositories;

public sealed class ExpenseRepository(NpgsqlConnection connection) : IExpenseRepository
{
    public async Task CreateAsync(ExpenseInfo info)
    {
        const String sql = """
                           INSERT INTO expenses(id, group_id, description, currency, method, created_by, created_at)
                           VALUES (@Id, @GroupId, @Description, @Currency, @Method, @CreatedBy, @CreatedAt);
                           """;
        await connection.ExecuteAsync(sql, info);
    }
}