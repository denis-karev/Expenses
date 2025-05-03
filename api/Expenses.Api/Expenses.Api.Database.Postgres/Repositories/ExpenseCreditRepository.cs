using Dapper;
using Expenses.Api.Database.Repositories;
using Expenses.Api.Model;
using Npgsql;

namespace Expenses.Api.Database.Postgres.Repositories;

public sealed class ExpenseCreditRepository(NpgsqlConnection connection) : IExpenseCreditRepository
{
    public async Task CreateAsync(ExpenseCreditInfo info)
    {
        const String sql = """
                           INSERT INTO expenses_credits(expense_id, group_member_id, amount)
                           VALUES (@ExpenseId, @GroupMemberId, @Amount);
                           """;
        await connection.ExecuteAsync(sql, info);
    }
}