namespace Expenses.Api.Models.Auth;

public sealed record TokenResponse(String AccessToken, String RefreshToken, DateTime Expiration);