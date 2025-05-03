using Dapper;
using Expenses.Api.Database.Repositories;
using Expenses.Api.Model;
using Npgsql;

namespace Expenses.Api.Database.Postgres.Repositories;

public sealed class ExpenseDebtRepository(NpgsqlConnection connection) : IExpenseDebtRepository
{
    public async Task CreateAsync(ExpenseDebtInfo info)
    {
        const String sql = """
                           INSERT INTO expenses_debts(expense_id, group_member_id, amount)
                           VALUES (@ExpenseId, @GroupMemberId, @Amount);
                           """;
        await connection.ExecuteAsync(sql, info);
    }
}