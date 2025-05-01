namespace Expenses.Api.Models;

public sealed record GroupModel(
    Guid Id,
    String Name,
    String Currency,
    DateTimeOffset CreatedAt
);