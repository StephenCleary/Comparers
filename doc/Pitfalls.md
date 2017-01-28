The one major pitfall in the Comparers library is that every comparer implementation assumes that it has a `GetHashCode` implementation available.

The core comparer implementations (`Compare<T>.Default()`, `Compare<T>.Null()`, and types derived from `ComparableBase<T>`) do provide `GetHashCode`. Every comparer extension provides `GetHashCode` if their source comparer(s) provide `GetHashCode`.

The pitfall comes in when a comparer extension is used on a "bare bones" comparer that does not provide `GetHashCode`. This happens in two situations:
* A custom `IComparer<T>` implementation is used with a comparer extension from the Comparers library.
* An `AnonymousComparer<T>` instance is used without defining the `GetHashCode` delegate.

In this case, the resulting comparer will function correctly as a _comparer_, but it will fail at runtime if it ever attempts to execute `GetHashCode`.