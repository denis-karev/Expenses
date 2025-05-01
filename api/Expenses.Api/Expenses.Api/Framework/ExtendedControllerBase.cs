using Expenses.Api.Common;
using Microsoft.AspNetCore.Mvc;

namespace Expenses.Api.Framework;

public abstract class ExtendedControllerBase : ControllerBase
{
    [NonAction]
    protected IActionResult ResultBasedOnError(Error error)
    {
        return error.Type switch
        {
            EErrorType.BadRequest => BadRequest(error),
            EErrorType.Unauthorized => Unauthorized(error),
            EErrorType.InternalServerError => StatusCode(500, error),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}