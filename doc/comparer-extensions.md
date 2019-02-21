# Comparer Extensions

Comparer extensions take a "source" comparer and apply some additional logic, returning a "result" comparer.

## Reverse

`Reverse` reverses the comparison used by the source comparer.

```C#
var ints = Enumerable.Range(0, 5).ToList(); // [0, 1, 2, 3, 4]
ints.Sort(ComparerBuilder.For<int>().Default().Reverse()); // [4, 3, 2, 1, 0]
```

There is no notion of "reverse" for equality comparers.

## ThenBy

`ThenBy` returns a `CompoundComparer`. A `CompoundComparer` always applies the source comparer first. If the source comparer indicates both items are equal, then another comparer is used as a "tie breaker". If you wish to apply the "tie breaker" comparer in a descending way, you can set the `descending` argument of `ThenBy` to `true`.

For equality comparers, use `ThenEquateBy`. There is no notion of "descending" for equality comparers.

## Sequence

`Sequence` converts an `IComparer<T>` into an `IComparer<IEnumerable<T>>` by using lexicographical sorting.

For equality comparers, use `EquateSequence`.

# Fixing Incomplete Comparers

Some of the .NET built-in comparers have incomplete semantics. The `Nito.Comparers.Fixes` namespace includes extension methods for fixing incomplete built-in and custom comparers.

## Standardized Null Handling

Some comparers (e.g., the .NET `StringComparer` instances) [do not have complete treatment of `null`](https://github.com/StephenCleary/Comparers/blob/0dbf1c7f7452fc5069b8266214305897faf90958/test/UnitTests/WeirdFrameworkEdgeConditions.cs#L43). You can create a wrapper comparer that adds standard `null`-handling semantics by calling `WithStandardNullHandling`:

```C#
var fixedStringComparer = StringComparer.Ordinal.WithStandardNullHandling();
fixedStringComparer.GetHashCode(null); // no longer throws
```

## `GetHashCode` Implementations

Some comparers only support `CompareTo`. Nito Comparers only work with comparers that also implement equality comparison; `Equals` is auto-implemented using `CompareTo` semantics, but `GetHashCode` cannot be automatically implemented for a comparer that only supports `CompareTo`. There are a couple of options for adding a `GetHashCode` implementation.

The best option is to implement `GetHashCode` with equivalent logic as `CompareTo`:

```C#
var fixedComparer = myComparer.WithGetHashCode(x => /* my implementation */);
```

Alternatively, if the logic is unknown or too complex, you can use a `GetHashCode` implementation that returns a constant. Note that this has performance implications; since all objects have the same hash code, hash-based collections are forced to their least efficient performance:

```C#
var fixedComparer = myComparer.WithGetHashCodeConstant();
```

If you're *absolutely sure* that `GetHashCode` won't be called (or *shouldn't* be called), then you can use a `GetHashCode` implementation that throws `NotImplementedException`:

```C#
var fixedComparer = myComparer.WithGetHashCodeThrow();
```
