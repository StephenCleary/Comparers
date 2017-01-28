# A Lap around the Comparers Library

The Comparers library has four main parts:

1. Comparer implementations. You can use the `Compare`/`EqualityCompare` and `CompareSource`/`EqualityCompareSource` classes to create comparer instances with a fluent API.
1. Extension methods for comparers. The `ComparerExtensions` and `EqualityComparerExtensions` classes provide extensions that can be used to modify any comparer (including custom or built-in comparers).
1. Assistance for a type defining its own default comparer.
1. Extension methods for LINQ to Objects/Rx/Ix. All operators that take a comparer get overloads that allow a fluent API syntax for defining a comparer right within the LINQ query.

Every full comparer provided by this library implements `IFullComparer<T>`, which derives from all four interfaces `IComparer<T>`, `IComparer`, `IEqualityComparer<T>`, and `IEqualityComparer`. Every equality comparer provided by this library implements `IFullEqualityComparer<T>`, which derives from both `IEqualityComparer<T>` and `IEqualityComparer`.

This means they can be used with any generic or non-generic container or algorithm. Also, all the comparers implement equality comparison as well, so they can be used with hash-based containers and algorithms.

Since the .NET `Comparer<T>.Default` type does not implement `IEqualityComparer<T>`, this library provides its own [[default comparer]].