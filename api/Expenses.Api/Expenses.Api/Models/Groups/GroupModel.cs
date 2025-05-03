namespace Expenses.Api.Models.Groups;

public sealed record GroupModel(
    Guid Id,
    String Name,
    String Currency,
    DateTimeOffset CreatedAt
);