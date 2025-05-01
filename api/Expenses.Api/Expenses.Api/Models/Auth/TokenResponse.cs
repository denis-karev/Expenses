namespace Expenses.Api.Models.Auth;

public sealed record TokenResponse(String AccessToken, DateTime Expiration);