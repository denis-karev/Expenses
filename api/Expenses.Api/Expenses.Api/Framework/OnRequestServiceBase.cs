using Expenses.Api.Common;
using Expenses.Api.Database;
using Expenses.Api.Entities;

namespace Expenses.Api.Framework;

public abstract class OnRequestServiceBase(IHttpContextAccessor httpContextAccessor, IDatabaseContext context)
{
    protected HttpContext HttpContext => httpContextAccessor.HttpContext.ThrowIfNull();

    protected async Task<InvokeResult<User>> GetCurrentUserAsync()
    {
        if (HttpContext.User.Identity?.Name is null)
            return InvokeResult<User>.CreateError(EErrorType.Unauthorized, "Unauthorized.");
            
        if (!Guid.TryParse(HttpContext.User.Identity.Name, out var userId))
            return InvokeResult<User>.CreateError(EErrorType.Unauthorized, "Invalid user ID in token.");
            
        var user = await User.FindAsync(context, userId);
        if (user is null)
            return InvokeResult<User>.CreateError(EErrorType.Unauthorized, "Invalid user ID in token.");
        
        return InvokeResult<User>.CreateSuccess(user);
    }
}