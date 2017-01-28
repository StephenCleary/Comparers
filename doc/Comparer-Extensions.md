Comparer extensions take a "source" comparer and apply some additional logic, returning a "result" comparer.

# Reverse

`Reverse` reverses the comparison used by the source comparer.

    var ints = Enumerable.Range(0, 5).ToList(); // [0, 1, 2, 3, 4]
    ints.Sort(Compare<int>.Default().Reverse()); // [4, 3, 2, 1, 0]

There is no notion of "reverse" for equality comparers.

# ThenBy (and ThenByDescending)

`ThenBy` returns a `CompoundComparer`. A `CompoundComparer` always applies the source comparer first. If the source comparer indicates both items are equal, then another comparer is used as a "tie breaker".

For equality comparers, use `ThenEquateBy`. There is no notion of "descending" for equality comparers.

# Sequence

`Sequence` converts an `IComparer<T>` into an `IComparer<IEnumerable<T>>` by using lexicographical sorting.

For equality comparers, use `EquateSequence`.
