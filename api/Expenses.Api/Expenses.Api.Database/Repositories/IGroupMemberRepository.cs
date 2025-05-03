using Expenses.Api.Model;

namespace Expenses.Api.Database.Repositories;

public interface IGroupMemberRepository
{
    Task CreateAsync(GroupMemberInfo info);
    Task<IReadOnlyDictionary<Guid, GroupMemberInfo?>> FindByIdsInGroupAsync(Guid groupId, ICollection<Guid> ids);
    Task<Boolean> IsGroupMemberAsync(Guid groupId, Guid userId);
}