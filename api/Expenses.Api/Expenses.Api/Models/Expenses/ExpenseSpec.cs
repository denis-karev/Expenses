using Expenses.Api.Model;

namespace Expenses.Api.Models.Expenses;

public sealed record ExpenseSpec(
    String Description,
    String Currency,
    ESplitMethod Split,
    IReadOnlyList<ExpenseCreditorSpec> Creditors,
    IReadOnlyList<ExpenseDebtorSpec> Debtors
);

public sealed record ExpenseCreditorSpec(
    Guid MemberId,
    Decimal Amount
);

public sealed record ExpenseDebtorSpec(
    Guid MemberId,
    Decimal Amount
);

public sealed record ExpenseResponse();