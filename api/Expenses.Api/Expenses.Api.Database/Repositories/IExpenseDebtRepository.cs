using Expenses.Api.Model;

namespace Expenses.Api.Database.Repositories;

public interface IExpenseDebtRepository
{
    Task CreateAsync(ExpenseDebtInfo info);
}