using Dapper;
using Expenses.Api.Database.Repositories;
using Expenses.Api.Model;
using Npgsql;

namespace Expenses.Api.Database.Postgres.Repositories;

public sealed class CurrencyRepository(NpgsqlConnection connection) : ICurrencyRepository
{
    public async Task<CurrencyInfo?> FindAsync(String code)
    {
        const String sql = """
                           SELECT * FROM currencies WHERE code = @Code;
                           """;
        return await connection.QuerySingleOrDefaultAsync<CurrencyInfo>(sql, new { Code = code });
    }
}