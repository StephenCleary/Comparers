# v5 Releases

## 5.0.7 (2019-02-12)

- NuGet package metadata updates.

## 5.0.6 (2019-02-12)

- Fixed #12: when comparison methods are passed invalid types (using the nongeneric `IEqualityComparer` or `IComparer` interfaces), they could throw `InvalidCastException` or `NullReferenceException`.
