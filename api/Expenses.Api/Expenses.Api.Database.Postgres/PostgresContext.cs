using System.Data;
using Expenses.Api.Common;
using Expenses.Api.Database.Postgres.Repositories;
using Expenses.Api.Database.Repositories;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Expenses.Api.Database.Postgres;

internal sealed class PostgresContext : IDatabaseContext
{
    private readonly NpgsqlConnection _connection;
    public IDbConnection Connection => _connection;
    public IUserRepository Users { get; }

    static PostgresContext()
    {
        Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
    }

    public PostgresContext(IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Default").ThrowIfNull();
        _connection = new NpgsqlConnection(connectionString);
        
        Users = new UserRepository(_connection);
        
        _connection.Open();
    }

    public void Dispose()
    {
        _connection.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await _connection.DisposeAsync();
    }
}