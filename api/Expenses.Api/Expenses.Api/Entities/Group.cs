using Expenses.Api.Database;
using Expenses.Api.Model;

namespace Expenses.Api.Entities;

public sealed class Group
{
    private readonly IDatabaseContext _context;
    
    public GroupInfo Info { get; private set; }

    private Group(IDatabaseContext context, GroupInfo info)
    {
        _context = context;
        Info = info;
    }

    public static async Task<Group> CreateAsync(IDatabaseContext context, GroupInfo info, User user)
    {
        using var transaction = context.Connection.BeginTransaction();
        await context.Groups.CreateAsync(info);
        
        var memberInfo = new GroupMemberInfo(Guid.NewGuid(), info.Id, user.Info.Id, null, info.CreatedAt);
        await GroupMember.CreateAsync(context, memberInfo);
        
        transaction.Commit();
        
        return new Group(context, info);
    }
}