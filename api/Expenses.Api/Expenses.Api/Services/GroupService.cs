using Expenses.Api.Common;
using Expenses.Api.Database;
using Expenses.Api.Entities;
using Expenses.Api.Framework;
using Expenses.Api.Model;
using Expenses.Api.Models.GroupMembers;
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

    public async Task<InvokeResult<GroupMemberModel>> AddMemberAsync(Guid id, AddGroupMemberSpec spec)
    {
        try
        {
            if (spec.Type is EGroupMemberType.User && spec.User is null)
                return InvokeResult<GroupMemberModel>.CreateError(EErrorType.BadRequest, "User model cannot be null.");
            if (spec.Type is EGroupMemberType.Offline && spec.Offline is null)
                return InvokeResult<GroupMemberModel>.CreateError(EErrorType.BadRequest,
                    "Offline model cannot be null.");

            var currentUser = await GetCurrentUserAsync();
            if (!currentUser.IsSuccess)
                return InvokeResult<GroupMemberModel>.CreateError(currentUser.CheckedError);

            var group = await Group.FindAsync(_context, id);
            if (group is null)
                return InvokeResult<GroupMemberModel>.CreateError(EErrorType.BadRequest, "Invalid group ID.");

            if (!await group.IsGroupMember(currentUser.CheckedResult.Info.Id))
                return InvokeResult<GroupMemberModel>.CreateError(EErrorType.Unauthorized,
                    "You do not have permission to add members to this group.");

            if (spec.Type is EGroupMemberType.User)
            {
                var user = await User.FindAsync(_context, spec.User!.UserId);
                if (user is null)
                    return InvokeResult<GroupMemberModel>.CreateError(EErrorType.BadRequest, "Invalid user ID.");
            }

            var groupMember = new GroupMemberInfo(
                Id: Guid.NewGuid(),
                GroupId: group.Info.Id,
                UserId: spec.User?.UserId,
                Name: spec.Offline?.Name,
                JoinedAt: DateTimeOffset.UtcNow
            );
            await GroupMember.CreateAsync(_context, groupMember);

            var result = new GroupMemberModel(
                Id: groupMember.Id,
                GroupId: groupMember.GroupId,
                UserId: groupMember.UserId,
                Name: groupMember.Name,
                JoinedAt: groupMember.JoinedAt
            );
            return InvokeResult<GroupMemberModel>.CreateSuccess(result);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error occured while adding a member to a group: {GroupMember}.", spec);
            return InvokeResult<GroupMemberModel>.CreateError(EErrorType.InternalServerError, e.Message);
        }
    }
}