using System.Data;
using Expenses.Api.Database.Repositories;

namespace Expenses.Api.Database;

public interface IDatabaseContext : IDisposable, IAsyncDisposable
{
    IDbConnection Connection { get; }
    IUserRepository Users { get; }
}