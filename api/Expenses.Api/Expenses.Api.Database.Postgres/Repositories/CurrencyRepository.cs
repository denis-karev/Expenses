using Expenses.Api.Database.Repositories;
using Npgsql;

namespace Expenses.Api.Database.Postgres.Repositories;

public sealed class CurrencyRepository(NpgsqlConnection connection) : ICurrencyRepository
{
}