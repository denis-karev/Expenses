namespace Expenses.Api.Model;

public sealed record GroupInfo(
    Guid Id, 
    String Name, 
    String Currency,
    DateTimeOffset CreatedAt);