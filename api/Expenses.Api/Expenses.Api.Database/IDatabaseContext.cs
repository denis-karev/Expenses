using System.Data;
using Expenses.Api.Database.Repositories;

namespace Expenses.Api.Database;

public interface IDatabaseContext : IDisposable, IAsyncDisposable
{
    IDbConnection Connection { get; }
    
    IUserRepository Users { get; }
    ICurrencyRepository Currencies { get; }
    IGroupRepository Groups { get; }
    IGroupMemberRepository GroupMembers { get; }
    IExpenseRepository Expenses { get; }
    IExpenseCreditRepository ExpenseCredits { get; }
    IExpenseDebtRepository ExpenseDebts { get; }
}