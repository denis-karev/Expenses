namespace Expenses.Api.Models.Auth;

public sealed record TokenRequest(EAuthMethod Method, RefreshTokenAuthRequest? RefreshToken, GoogleAuthRequest? Google);