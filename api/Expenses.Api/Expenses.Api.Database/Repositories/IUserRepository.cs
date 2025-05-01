using Expenses.Api.Model;

namespace Expenses.Api.Database.Repositories;

public interface IUserRepository
{
    Task CreateAsync(UserInfo info);
    Task UpdateAsync(Guid id, UserInfo info);
    Task<UserInfo?> FindAsync(Guid id);
    Task<UserInfo?> FindByEmailAsync(String email);
}