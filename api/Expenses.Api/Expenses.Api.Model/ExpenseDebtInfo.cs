namespace Expenses.Api.Model;

public sealed record ExpenseDebtInfo(
    Guid ExpenseId,
    Guid GroupMemberId,
    Decimal Amount
);