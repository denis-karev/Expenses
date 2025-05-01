namespace Expenses.Api.Model;

public sealed record ExpenseInfo(
    Guid Id,
    Guid GroupId,
    String Description,
    String Currency,
    ESplitMethod Method,
    Guid CreatedBy,
    DateTimeOffset CreatedAt);