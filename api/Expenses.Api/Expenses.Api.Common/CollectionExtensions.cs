using System;
using System.Collections.Generic;
using System.Linq;

namespace Expenses.Api.Common;

public static class CollectionExtensions
{
    public static Boolean IsNullOrEmpty<T>(this IEnumerable<T>? collection)
    {
        if (collection is null)
            return true;
        return !collection.Any();
    }
}