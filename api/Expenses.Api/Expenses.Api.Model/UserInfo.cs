namespace Expenses.Api.Model;

public sealed record UserInfo(Guid Id, String Email, String Name, String? EncryptedGoogleToken);