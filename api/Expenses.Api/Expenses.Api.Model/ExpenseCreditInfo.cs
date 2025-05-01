namespace Expenses.Api.Model;

public sealed record ExpenseCreditInfo(
    Guid ExpenseId,
    Guid GroupMemberId,
    Decimal Amount
);