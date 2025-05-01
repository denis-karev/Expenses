namespace Expenses.Api.Common;

public sealed record Error(EErrorType Type, String Message);