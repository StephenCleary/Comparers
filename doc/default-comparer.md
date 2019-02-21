# Default Comparers

## A Word on Terminology

The term "default comparer" can refer to several different things:
* Any type implementing `IComparable<T>` (or `IComparable`) has a built-in .NET "default comparer" `Comparer<T>.Default` (or `Comparer.Default`). Similarly, all types have a built-in .NET "default equality comparer" `EqualityComparer<T>.Default` (or `Object.Equals`).
* The Comparers library also provides a "default comparer" `ComparerBuilder.For<T>().Default()` and a "default equality comparer" `EqualityComparerBuilder.For<T>().Default()`.
* Several methods in the Comparers library take an `IComparer<T>` (or `IEqualityComparer<T>`) argument, which may be `null`. When it is `null`, the method uses the default comparer (or equality comparer) provided by the Comparers library.

## `Comparer<T>.Default` and `ComparerBuilder.For<T>().Default()`

The built-in .NET `Comparer<T>.Default` does not implement `IEqualityComparable<T>` (or `IEqualityComparable`). For this reason, the Comparers library provides `ComparerBuilder.For<T>().Default()`, which does implement all relevant interfaces. `ComparerBuilder.For<T>().Default()` is just a combination of `Comparer<T>.Default` and `EqualityComparer<T>.Default`, unless `T` is a sequence type without a default comparer (see below).

Note: the .NET `StringComparer` type _does_ implement `IEqualityComparable<string>` (and `IEqualityComparable`), so the Comparers library does not provide any string comparers.

## Default Comparers for Sequences

If the compared type implements `IEnumerable<U>` and does not define its own default comparer (i.e., `IComparable<T>`), then `ComparerBuilder.For<T>().Default()` will return a sequence comparer that uses lexicographical sorting. Similarly, `EqualityComparerBuilder.For<T>().Default()` will return a sequence comparer that will test lexicographic equality.

For example, this allows `ComparerBuilder.For<int[]>().Default()` to have expected semantics.

## Default Comparer Arguments

If an `IComparer<T>` argument is `null`, the Comparers library will use `ComparerBuilder.For<T>().Default()`. Furthermore, if the .NET `Comparer<T>.Default` is passed as an argument, the Comparers library will replace that argument with `ComparerBuilder.For<T>().Default()`.

Similarly, `IEqualityComparer<T>` arguments that are `null` or `EqualityComparer<T>.Default` are replaced with `EqualityComparerBuilder.For<T>().Default()`.