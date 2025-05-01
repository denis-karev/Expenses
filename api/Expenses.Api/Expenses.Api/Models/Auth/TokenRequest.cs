namespace Expenses.Api.Models.Auth;

public sealed record TokenRequest(EAuthMethod Method, GoogleAuthRequest? Google);