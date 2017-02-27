# Pitfalls

The one major pitfall in the Comparers library is that every comparer implementation assumes that it has a `GetHashCode` implementation available.

The core comparer implementations (`ComparerBuilder<T>` and types derived from `ComparableBase<T>`) do provide `GetHashCode`. Every comparer extension provides `GetHashCode` if their source comparer(s) provide `GetHashCode`.

The pitfall comes in when a comparer extension is used on a "bare bones" comparer that does not provide `GetHashCode`. This only happens in this situation:
* A custom `IComparer<T>` implementation is used with a comparer extension from the Comparers library, and that `IComparer<T>` does not also implement `IEqualityComparer<T>`.

In this case, the resulting comparer will function correctly as a _comparer_, but it will fail at runtime if it ever attempts to execute `GetHashCode`.