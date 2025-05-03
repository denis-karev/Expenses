using Expenses.Api.Model;

namespace Expenses.Api.Database.Repositories;

public interface IExpenseCreditRepository
{
    Task CreateAsync(ExpenseCreditInfo info);
}