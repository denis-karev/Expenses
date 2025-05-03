using Expenses.Api.Common;
using Expenses.Api.Database;
using Expenses.Api.Entities;
using Expenses.Api.Framework;
using Expenses.Api.Model;
using Expenses.Api.Models.Expenses;

namespace Expenses.Api.Services;

public sealed class ExpenseService(
    IHttpContextAccessor httpContextAccessor,
    IDatabaseContext context,
    ILogger<ExpenseService> logger
) : OnRequestServiceBase(httpContextAccessor, context)
{
    private readonly IDatabaseContext _context = context;
    private sealed record ExpenseData(ExpenseInfo Info, IReadOnlyList<ExpenseCreditInfo> Creditors, IReadOnlyList<ExpenseDebtInfo> Debtors);

    public async Task<InvokeResult<ExpenseResponse>> CreateAsync(Guid groupId, ExpenseSpec spec)
    {
        try
        {
            var currentUser = await GetCurrentUserAsync();
            if (!currentUser.IsSuccess)
                return InvokeResult<ExpenseResponse>.CreateError(currentUser.CheckedError);

            var group = await Group.FindAsync(_context, groupId);
            if (group is null)
                return InvokeResult<ExpenseResponse>.CreateError(EErrorType.BadRequest, "Invalid group ID.");
            
            var data = await ValidateAsync(group, spec, currentUser.CheckedResult);
            if (!data.IsSuccess)
                return InvokeResult<ExpenseResponse>.CreateError(data.CheckedError);
            
            var expense = await Expense.CreateAsync(_context, data.CheckedResult.Info, data.CheckedResult.Creditors, data.CheckedResult.Debtors);
            return InvokeResult<ExpenseResponse>.CreateSuccess(new());
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error occured while creating an expense: {Expense}.", spec);
            return InvokeResult<ExpenseResponse>.CreateError(EErrorType.InternalServerError, e.Message);
        }
    }

    private async Task<InvokeResult<ExpenseData>> ValidateAsync(Group group, ExpenseSpec spec, User currentUser)
    {
        if (!await group.IsGroupMember(currentUser.Info.Id))
            return InvokeResult<ExpenseData>.CreateError(EErrorType.Unauthorized, "You do not have permission to create expenses in this group.");
            
        var expenseInfo = await ValidateAndCreateExpenseInfoAsync(group, spec, currentUser);;
        if (!expenseInfo.IsSuccess)
            return InvokeResult<ExpenseData>.CreateError(expenseInfo.CheckedError);
            
        var creditors = ValidateAndCreateExpenseCreditsAsync(expenseInfo.CheckedResult.Id, spec);
        if (!creditors.IsSuccess)
            return InvokeResult<ExpenseData>.CreateError(creditors.CheckedError);
            
        var debtors = ValidateAndCreateExpenseDebtAsync(creditors.CheckedResult.Total, expenseInfo.CheckedResult.Id, spec);
        if (!debtors.IsSuccess)
            return InvokeResult<ExpenseData>.CreateError(debtors.CheckedError);

        var groupMembersResult = await ValidateGroupMembersAsync(creditors.CheckedResult.Item2, debtors.CheckedResult, group);
        if (!groupMembersResult.IsSuccess)
            return InvokeResult<ExpenseData>.CreateError(groupMembersResult.CheckedError);
        
        return InvokeResult<ExpenseData>.CreateSuccess(new(expenseInfo.CheckedResult, creditors.CheckedResult.Item2, debtors.CheckedResult));
    }

    private async Task<InvokeResult<ExpenseInfo>> ValidateAndCreateExpenseInfoAsync(Group group, ExpenseSpec spec, User user)
    {
        var name = spec.Description.Trim();
        if (name.IsNullOrEmpty())
            return InvokeResult<ExpenseInfo>.CreateError(EErrorType.BadRequest, "Description cannot be empty.");
        
        var currencyName = spec.Currency.Trim().ToUpperInvariant();
        var currency = await _context.Currencies.FindAsync(currencyName);
        if (currency is null)
            return InvokeResult<ExpenseInfo>.CreateError(EErrorType.BadRequest, $"Invalid currency {currencyName}.");
        
        var result = new ExpenseInfo(
            Id: Guid.NewGuid(),
            GroupId: group.Info.Id,
            Description: spec.Description.Trim(),
            Method: spec.Split,
            Currency: spec.Currency.Trim().ToUpperInvariant(),
            CreatedAt: DateTimeOffset.UtcNow,
            CreatedBy: user.Info.Id
        );
        
        return InvokeResult<ExpenseInfo>.CreateSuccess(result); 
    }

    private InvokeResult<(Decimal Total, IReadOnlyList<ExpenseCreditInfo>)> ValidateAndCreateExpenseCreditsAsync(Guid expenseId, ExpenseSpec spec)
    {
        var creditors = spec.Creditors.Select(x => new ExpenseCreditInfo(expenseId, x.MemberId, x.Amount)).ToList();
        
        var totalAmount = creditors.Sum(x => x.Amount);
        if (totalAmount <= 0)
            return InvokeResult<(Decimal, IReadOnlyList<ExpenseCreditInfo>)>.CreateError(EErrorType.BadRequest, "Total expense amount must be greater than 0.");

        return InvokeResult<(Decimal, IReadOnlyList<ExpenseCreditInfo>)>.CreateSuccess((totalAmount, creditors));
    }

    private InvokeResult<IReadOnlyList<ExpenseDebtInfo>> ValidateAndCreateExpenseDebtAsync(Decimal total, Guid expenseId, ExpenseSpec spec)
    {
        var debtors = spec.Debtors.Select(x => new ExpenseDebtInfo(expenseId, x.MemberId, x.Amount)).ToList();
        
        var totalDebt = debtors.Sum(x => x.Amount);
        if (spec.Split == ESplitMethod.Amounts && totalDebt != total)
            return InvokeResult<IReadOnlyList<ExpenseDebtInfo>>.CreateError(EErrorType.BadRequest, "Total debt must be equal to total amount.");
        if (spec.Split == ESplitMethod.Percentages && totalDebt != 100)
            return InvokeResult<IReadOnlyList<ExpenseDebtInfo>>.CreateError(EErrorType.BadRequest, "Total debt must be equal to 100%.");
        if (spec.Split == ESplitMethod.Shares && totalDebt < 1)
            return InvokeResult<IReadOnlyList<ExpenseDebtInfo>>.CreateError(EErrorType.BadRequest, "Total amount of debt shares must be greater than 0.");
        
        return InvokeResult<IReadOnlyList<ExpenseDebtInfo>>.CreateSuccess(debtors);
    }
    private async Task<InvokeResult<Boolean>> ValidateGroupMembersAsync(IReadOnlyList<ExpenseCreditInfo> creditors, IReadOnlyList<ExpenseDebtInfo> debtors, Group group)
    {
        var memberIds = creditors.Select(x => x.GroupMemberId)
            .Concat(debtors.Select(x => x.GroupMemberId))
            .Distinct().ToList();
        var members = await group.FindGroupMembersByIds(memberIds);
        var nonExistingMembers = members.Where(x => x.Value is null).Select(x => x.Key).ToList();
        if (nonExistingMembers.Count != 0)
            return InvokeResult<Boolean>.CreateError(EErrorType.BadRequest, $"Members do not exist: {String.Join(", ", nonExistingMembers)}.");

        return InvokeResult<Boolean>.CreateSuccess(true);
    }
}