# Upgrading from the pre-netstandard `Comparers` package to the new `Nito.Comparers` package.

- Code formerly using `Comparer<T>` and `CompareSource` should now use `ComparerBuilder`.

- Similarly, `EqualityComparer<T>` and `EqualityCompareSource` have been replaced by `EqualityComparerBuilder`.
- The anonymous comparers have been removed.

- `OrderByDescending` and `ThenByDescending` have been removed; instead, use the `descending` parameter of `OrderBy`/`ThenBy`.
- The `allowNulls` parameter of all methods has been renamed to `specialNullHandling`.

- All equality comparer types are in the `Nito.Comparers` namespace instead of `Nito.EqualityComparers`.
