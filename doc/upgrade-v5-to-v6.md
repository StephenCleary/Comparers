# Upgrading from v5 to v6

Most consumers will not be affected by this upgrade.

- The builder types `ComparerBuilder<T>` and `EqualityComparerBuilder<T>` have been renamed to `ComparerBuilderFor<T>` and `EqualityComparerBuilderFor<T>`. Most consumers use the `ComparerBuilder.For<T>` / `EqualityComparerBuilder.For<T>` patterns, and will not be affected by this change.

- Some builder methods are now extension methods, so the code doing the building must be `using Nito.Comparers;`

- `EqualityComparerBuilder.For<T>()` no longer supports `Reference()` if `T` is a value type, since reference comparers don't make sense for value types. Technically, these could be created before, but they were not useful. #21

- Plain `IComparer<T>` comparers are no longer supported as source comparers by the Nito.Comparers extension methods. They must also implement `IEqualityComparer<T>` or `IEqualityComparer`. Previously, the behavior in this scenario was to throw `NotImplementedException` if `GetHashCode` was ever called. The new behavior detects this scenario at the time of comparer construction and throws `InvalidOperationException` immediately. If this is a problem, you can add an `IEqualityComparer<T>` implementation that either implements `GetHashCode` appropriately, or returns a constant value, or throws an exception, whichever is desired. #16
