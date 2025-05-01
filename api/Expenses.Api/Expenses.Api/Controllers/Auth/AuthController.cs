using Expenses.Api.Models.Auth;
using Microsoft.AspNetCore.Mvc;

namespace Expenses.Api.Controllers.Auth;

[ApiController, Route("/api/v1/auth")]
public sealed class AuthController : ControllerBase
{
    [HttpPost("token")]
    public async Task<IActionResult> GetTokenAsync(TokenRequest request)
    {
        return Ok();
    } 
}