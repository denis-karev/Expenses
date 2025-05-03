using Expenses.Api.Model;

namespace Expenses.Api.Database.Repositories;

public interface IGroupRepository
{
    Task CreateAsync(GroupInfo info);
    Task<GroupInfo?> FindAsync(Guid id);
}