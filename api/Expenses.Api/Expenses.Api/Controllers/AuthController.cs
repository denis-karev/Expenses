using Expenses.Api.Common;
using Expenses.Api.Framework;
using Expenses.Api.Models.Auth;
using Expenses.Api.Services.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Expenses.Api.Controllers;

[ApiController, Route("/api/v1/auth")]
public sealed class AuthController(
    AuthService auth
) : ExtendedControllerBase
{
    [HttpPost("token")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(TokenResponse), 200, "application/json")]
    [ProducesResponseType(typeof(Error), 400, "application/json")]
    [ProducesResponseType(typeof(Error), 500, "application/json")]
    public async Task<IActionResult> GetTokenAsync(TokenRequest request)
    {
        request.ThrowIfNull();
        var result = await auth.AuthenticateAsync(request);
        if (!result.IsSuccess)
            return ResultBasedOnError(result.CheckedError);

        return Ok(result.CheckedResult);
    }
}