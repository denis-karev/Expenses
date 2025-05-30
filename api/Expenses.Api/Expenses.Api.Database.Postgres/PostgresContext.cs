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
    public ICurrencyRepository Currencies { get; }
    public IGroupRepository Groups { get; }
    public IGroupMemberRepository GroupMembers { get; }
    public IExpenseRepository Expenses { get; }
    public IExpenseCreditRepository ExpenseCredits { get; }
    public IExpenseDebtRepository ExpenseDebts { get; }

    static PostgresContext()
    {
        Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
    }

    public PostgresContext(IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Default").ThrowIfNull();
        _connection = new NpgsqlConnection(connectionString);
        
        Users = new UserRepository(_connection);
        Currencies = new CurrencyRepository(_connection);
        Groups = new GroupRepository(_connection);
        GroupMembers = new GroupMemberRepository(_connection);
        Expenses = new ExpenseRepository(_connection);
        ExpenseCredits = new ExpenseCreditRepository(_connection);
        ExpenseDebts = new ExpenseDebtRepository(_connection);
        
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