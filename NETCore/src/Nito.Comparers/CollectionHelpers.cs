using System;
using System.Collections;
using System.Collections.Generic;

internal static class CollectionHelpers
{
    public static int? TryGetCount<T>(this IEnumerable<T> source)
    {
        var result = source as IReadOnlyCollection<T>;
        if (result != null)
            return result.Count;
        var collection = source as ICollection<T>;
        if (collection != null)
            return collection.Count;
        var nongenericCollection = source as ICollection;
        if (nongenericCollection != null)
            return nongenericCollection.Count;

        return null;
    }
}