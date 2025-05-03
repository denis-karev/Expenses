using Expenses.Api.Model;

namespace Expenses.Api.Database.Repositories;

public interface IExpenseRepository
{
    Task CreateAsync(ExpenseInfo info);
}