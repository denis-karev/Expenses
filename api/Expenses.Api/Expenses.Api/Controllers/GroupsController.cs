using Expenses.Api.Common;
using Expenses.Api.Framework;
using Expenses.Api.Models;
using Expenses.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Expenses.Api.Controllers;

[ApiController, Route("/api/v1/groups"), Authorize]
public sealed class GroupsController(
    GroupService groups
) : ExtendedControllerBase
{
    [HttpPost]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(GroupModel), 200, "application/json")]
    [ProducesResponseType(typeof(Error), 400, "application/json")]
    [ProducesResponseType(typeof(Error), 500, "application/json")]
    public async Task<IActionResult> GetTokenAsync(GroupSpec request)
    {
        request.ThrowIfNull();
        var result = await groups.CreateAsync(request);
        if (!result.IsSuccess)
            return ResultBasedOnError(result.CheckedError);

        return Ok(result.CheckedResult);
    }
}