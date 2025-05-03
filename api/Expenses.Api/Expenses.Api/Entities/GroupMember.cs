using Expenses.Api.Database;
using Expenses.Api.Model;

namespace Expenses.Api.Entities;

public sealed class GroupMember
{
    private readonly IDatabaseContext _context;
    
    public GroupMemberInfo Info { get; private set; }

    public GroupMember(IDatabaseContext context, GroupMemberInfo info)
    {
        _context = context;
        Info = info;
    }
    
    public static async Task<GroupMember> CreateAsync(IDatabaseContext context, GroupMemberInfo info)
    {
        await context.GroupMembers.CreateAsync(info);
        return new GroupMember(context, info);
    }
}