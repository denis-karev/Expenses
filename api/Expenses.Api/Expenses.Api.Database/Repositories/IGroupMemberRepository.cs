using Expenses.Api.Model;

namespace Expenses.Api.Database.Repositories;

public interface IGroupMemberRepository
{
    Task CreateAsync(GroupMemberInfo info);
}