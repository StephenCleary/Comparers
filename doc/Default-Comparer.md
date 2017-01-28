# A Word on Terminology

The term "default comparer" can refer to several different things:
* Any type implementing `IComparable<T>` (or `IComparable`) has a built-in .NET "default comparer" `Comparer<T>.Default` (or `Comparer.Default`). Similarly, every type has a built-in .NET "default equality comparer" `EqualityComparer<T>.Default` (or `Object.Equals`).
* The Comparers library also provides a "default comparer" `Compare<T>.Default()` and a "default equality comparer" `EqualityCompare<T>.Default()`.
* Several methods in the Comparers library take an `IComparer<T>` (or `IEqualityComparer<T>`) argument, which may be `null`. When it is `null`, the method uses the default comparer (or equality comparer) provided by the Comparers library.

# Comparer<T>.Default and Compare<T>.Default()

The built-in .NET `Comparer<T>.Default` does not implement `IEqualityComparable<T>` (or `IEqualityComparable`). For this reason, the Comparers library provides `Compare<T>.Default()`, which does implement all relevant interfaces. `Compare<T>.Default()` is just a combination of `Comparer<T>.Default` and `EqualityComparer<T>.Default`, unless `T` is a sequence type without a default comparer (see below).

Note: the .NET `StringComparer` type _does_ implement `IEqualityComparable<string>` (and `IEqualityComparable`), so the Comparers library does not provide any string comparers.

# Default Comparers for Sequences

If the compared type implements `IEnumerable<U>` and does not define its own default comparer (i.e., `IComparable<T>`), then `Compare<T>.Default()` will return a sequence comparer that uses lexicographical sorting. Similarly, `EqualityCompare<T>.Default()` will return a sequence comparer that will test lexicographic equality.

For example, this allows `Compare<int[]>.Default()` to have expected semantics.

# Default Comparer Arguments

If an `IComparer<T>` argument is `null`, the Comparers library will use `Compare<T>.Default()`. Furthermore, if the .NET `Comparer<T>.Default` is passed as an argument, the Comparers library will replace that argument with `Compare<T>.Default()`.

Similarly, `IEqualityComparer<T>` arguments that are `null` or `EqualityComparer<T>.Default` are replaced with `EqualityCompare<T>.Default()`.
