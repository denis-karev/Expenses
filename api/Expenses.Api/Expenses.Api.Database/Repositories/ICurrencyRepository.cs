using Expenses.Api.Model;

namespace Expenses.Api.Database.Repositories;

public interface ICurrencyRepository
{
    Task<CurrencyInfo?> FindAsync(String code);
}