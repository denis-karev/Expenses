using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Expenses.Api.Common;

public static class ObjectExtensions
{
    [return:NotNull]
    public static T ThrowIfNull<T>(this T? obj, [CallerArgumentExpression("obj")] String? paramName = null)
    {
        if (obj is null)
            throw new ArgumentNullException(paramName, "Object is null");
        return obj;
    }
}