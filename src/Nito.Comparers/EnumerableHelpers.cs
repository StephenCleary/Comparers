using System;
using System.Collections.Generic;

internal static class EnumerableHelpers
{
    public static T First<T>(this IEnumerable<T> items)
    {
        using (var iterator = items.GetEnumerator())
        {
            iterator.MoveNext();
            return iterator.Current;
        }
    }
}