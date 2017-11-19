using System;
using System.Collections;
using System.Collections.Generic;

internal static class CollectionHelpers
{
    public static int? TryGetCount<T>(this IEnumerable<T> source)
    {
        if (source is IReadOnlyCollection<T> result)
            return result.Count;
        if (source is ICollection<T> collection)
            return collection.Count;
        if (source is ICollection nongenericCollection)
            return nongenericCollection.Count;

        return null;
    }
}