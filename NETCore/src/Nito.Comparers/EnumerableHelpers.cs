using System;
using System.Collections.Generic;

internal static class EnumerableHelpers
{
    public static T FirstOrDefault<T>(this IEnumerable<T> items)
    {
        using (var iterator = items.GetEnumerator())
        {
            if (!iterator.MoveNext())
                return default(T);
            return iterator.Current;
        }
    }
}