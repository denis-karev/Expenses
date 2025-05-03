using Expenses.Api.Common;
using Expenses.Api.Database;
using Expenses.Api.Entities;
using Expenses.Api.Framework;
using Expenses.Api.Model;
using Expenses.Api.Models;
using Expenses.Api.Models.Groups;

namespace Expenses.Api.Services;

public sealed class GroupService(
    ILogger<GroupService> logger,
    IHttpContextAccessor httpContextAccessor,
    IDatabaseContext context
) : OnRequestServiceBase(httpContextAccessor, context)
{
    private readonly IDatabaseContext _context = context;

    public async Task<InvokeResult<GroupModel>> CreateAsync(GroupSpec spec)
    {
        try
        {
            var currentUser = await GetCurrentUserAsync();
            if (!currentUser.IsSuccess)
                return InvokeResult<GroupModel>.CreateError(currentUser.CheckedError);

            var info = new GroupInfo(
                Id: Guid.NewGuid(),
                Name: spec.Name.Trim(),
                Currency: spec.Currency.Trim().ToUpperInvariant(),
                CreatedAt: DateTime.UtcNow);
            var validationResult = await ValidateAsync(info);
            if (!validationResult.IsSuccess)
                return InvokeResult<GroupModel>.CreateError(validationResult.CheckedError);

            var group = await Group.CreateAsync(_context, validationResult.CheckedResult, currentUser.CheckedResult);
            var result = new GroupModel(
                Id: group.Info.Id,
                Name: group.Info.Name,
                Currency: group.Info.Currency,
                CreatedAt: group.Info.CreatedAt);
            return InvokeResult<GroupModel>.CreateSuccess(result);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error occured while creating a group: {Group}.", spec);
            return InvokeResult<GroupModel>.CreateError(EErrorType.InternalServerError, e.Message);
        }
    }

    private async Task<InvokeResult<GroupInfo>> ValidateAsync(GroupInfo spec)
    {
        var name = spec.Name.Trim();
        if (name.IsNullOrEmpty())
            return InvokeResult<GroupInfo>.CreateError(EErrorType.BadRequest, "Name cannot be empty.");

        var currencyName = spec.Currency.Trim().ToUpperInvariant();
        var currency = await _context.Currencies.FindAsync(currencyName);
        if (currency is null)
            return InvokeResult<GroupInfo>.CreateError(EErrorType.BadRequest, $"Invalid currency {currencyName}.");

        return InvokeResult<GroupInfo>.CreateSuccess(spec);
    }
}