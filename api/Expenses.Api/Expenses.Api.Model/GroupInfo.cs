namespace Expenses.Api.Model;

public sealed record GroupInfo(Guid Id, String Name, String CurrencyCode, String? ShortCode, DateTimeOffset CreatedAt);