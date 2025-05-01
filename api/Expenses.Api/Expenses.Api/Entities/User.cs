using Expenses.Api.Database;
using Expenses.Api.Model;

namespace Expenses.Api.Entities;

public sealed class User
{
    private readonly IDatabaseContext _context;
    
    public UserInfo Info { get; private set; }

    private User(IDatabaseContext context, UserInfo info)
    {
        _context = context;
        Info = info;
    }

    public static async Task<User> CreateAsync(IDatabaseContext context, UserInfo info)
    {
        await context.Users.CreateAsync(info);
        return new User(context, info);
    }

    public static async Task<User?> FindAsync(IDatabaseContext context, Guid id)
    {
        var info = await context.Users.FindAsync(id);
        if (info is null)
            return null;
        return new User(context, info);
    }

    public static async Task<User?> FindByEmailAsync(IDatabaseContext context, String email)
    {
        var info = await context.Users.FindByEmailAsync(email);
        if (info is null) 
            return null;
        return new User(context, info);
    }

    public async Task UpdateAsync(UserInfo info)
    {
        await _context.Users.UpdateAsync(Info.Id, info);
        Info = info;
    }
}