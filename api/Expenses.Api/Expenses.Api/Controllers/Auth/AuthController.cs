using Expenses.Api.Common;
using Expenses.Api.Models.Auth;
using Expenses.Api.Services.Auth;
using Microsoft.AspNetCore.Mvc;

namespace Expenses.Api.Controllers.Auth;

[ApiController, Route("/api/v1/auth")]
public sealed class AuthController(
    ILogger<AuthController> logger,
    AuthService auth
) : ControllerBase
{
    [HttpPost("token")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(TokenResponse), 200, "application/json")]
    [ProducesResponseType(typeof(Error), 400, "application/json")]
    [ProducesResponseType(typeof(Error), 500, "application/json")]
    public async Task<IActionResult> GetTokenAsync(TokenRequest request)
    {
        try
        {
            request.ThrowIfNull();
            var result = await auth.AuthenticateAsync(request);
            if (!result.IsSuccess)
                return BadRequest(result.CheckedError);
            
            return Ok(result.CheckedResult);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error while getting token");
            return StatusCode(500, new Error(EErrorType.InternalServerError, e.Message));
        }
    }
}