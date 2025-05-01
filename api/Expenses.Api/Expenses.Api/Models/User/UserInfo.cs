namespace Expenses.Api.Models.User;

public sealed record UserInfo(Guid Id, String Email, String Name, String? EncryptedGoogleToken);