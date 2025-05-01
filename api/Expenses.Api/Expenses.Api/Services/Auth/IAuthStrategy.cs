using Expenses.Api.Common;
using Expenses.Api.Models.Auth;

namespace Expenses.Api.Services.Auth;

public interface IAuthStrategy<in T> where T : IAuthRequest
{
    Task<InvokeResult<TokenResponse>> AuthenticateAsync(T request);
}