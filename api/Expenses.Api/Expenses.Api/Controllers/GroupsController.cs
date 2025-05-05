using Expenses.Api.Common;
using Expenses.Api.Framework;
using Expenses.Api.Models;
using Expenses.Api.Models.Expenses;
using Expenses.Api.Models.GroupMembers;
using Expenses.Api.Models.Groups;
using Expenses.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Expenses.Api.Controllers;

[ApiController, Route("/api/v1/groups"), Authorize]
public sealed class GroupsController(
    GroupService groups,
    ExpenseService expenses
) : ExtendedControllerBase
{
    [HttpPost]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(GroupModel), 200, "application/json")]
    [ProducesResponseType(typeof(Error), 400, "application/json")]
    [ProducesResponseType(typeof(Error), 500, "application/json")]
    public async Task<IActionResult> CreateGroupAsync(GroupSpec request)
    {
        request.ThrowIfNull();
        var result = await groups.CreateAsync(request);
        if (!result.IsSuccess)
            return ResultBasedOnError(result.CheckedError);

        return Ok(result.CheckedResult);
    }

    [HttpPost("{id:guid}/expenses")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(ExpenseResponse), 200, "application/json")]
    [ProducesResponseType(typeof(Error), 400, "application/json")]
    [ProducesResponseType(typeof(Error), 500, "application/json")]
    public async Task<IActionResult> CreateExpenseAsync(Guid id, ExpenseSpec request)
    {
        request.ThrowIfNull();
        var result = await expenses.CreateAsync(id, request);
        if (!result.IsSuccess)
            return ResultBasedOnError(result.CheckedError);
        
        return Ok(result.CheckedResult);
    }
    
    [HttpPost("{id:guid}/members")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(GroupMemberModel), 200, "application/json")]
    [ProducesResponseType(typeof(Error), 400, "application/json")]
    [ProducesResponseType(typeof(Error), 500, "application/json")]
    public async Task<IActionResult> CreateExpenseAsync(Guid id, AddGroupMemberSpec request)
    {
        request.ThrowIfNull();
        var result = await groups.AddMemberAsync(id, request);
        if (!result.IsSuccess)
            return ResultBasedOnError(result.CheckedError);
        
        return Ok(result.CheckedResult);
    }
}