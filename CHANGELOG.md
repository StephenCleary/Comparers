# Changelog
This project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [6.2.2] - 2021-09-25
### Changed
- Bump Rx and Ix dependencies.

## [6.2.1] - 2021-09-25
### Fixed
- Explicity support net461 so that shim dlls are not required on that platform.

## [6.1.0] - 2020-10-24
### Added
- `IFullComparer<string>.ToStringComparer` to easily create a `StringComparer` instance.
- `FixedStringComparer` to provide `StringComparer` instances that do not throw on `null`.
- Nullable reference type checking.
- Added `AdvancedComparerBase<T>` and `AdvancedEqualityComparerBase<T>` for advanced scenarios.

### Fixed
- Static constructors for derived types are always invoked (so they set `DefaultComparer`), even if an instance is not ever created. [#34](https://github.com/StephenCleary/Comparers/issues/34)

### Changed
- Hash algorithm is now Murmur3.

## [6.0.0] - 2019-03-16

See [v6 upgrade guide](doc/upgrade-notes/v5-to-v6.md).

### Changed
- Some type names used in the builder pattern. Most users will not be affected.
- Plain `IComparer<T>` types must now declare a `GetHashCode` implementation to be used with `Nito.Comparers`.
- `ReferenceEqualityComparer` is now restricted to reference types.

### Added
- "Fix comparer" extension methods to apply default `null` handling to misbehaving comparers.

### Removed
- `BinarySearch` overloads that allowed defining the comparer inline.

## [5.0.7] - 2019-02-12
### Added
- Source link support.

## [5.0.6] - 2019-02-12
### Fixed
- Corner cases for comparers and equality comparers.
- `IEqualityComparer.GetHashCode` when called with an unexpected type.
- Stack overflow when `IComparer.Compare` is called for objects of different types that each forward their `IComparable` implementation to that method.
- Stack overflow in `EqualityComparerBase.IEqualityComparer.Equals` corner case.
- Fixed [#12](https://github.com/StephenCleary/Comparers/issues/12): when comparison methods are passed invalid types (using the nongeneric `IEqualityComparer` or `IComparer` interfaces), they could throw `InvalidCastException` or `NullReferenceException`.

## [5.0.5] - 2017-09-09
### Fixed
- Revert public signing.

## [5.0.4] - 2017-08-26
### Added
- Public signing.

## [5.0.3] - 2017-08-15
### Fixed
- Work around .NET Core SDK bug.

## [5.0.2] - 2017-04-14
### Changed
- Build scripts.

## [5.0.0] - 2017-02-27

See [v5 upgrade guide](doc/upgrade-notes/v4-to-v5.md).

### Changed
- NuGet package is now `Nito.Comparers` instead of `Comparers`.
- Better names for types used in the "builder" pattern for comparers.
- `allowNulls` parameter name changed to `specialNullHandling`.
- Equality comparer types moved from `Nito.EqualityComparers` namespace to `Nito.Comparers`.

### Removed
- The anonymous comparers have been removed.
- `*Descending` methods have been removed; use the `descending` parameter instead.

## [4.0.0] - 2014-05-13
### Fixed
- Bug in determining default comparer for sequences.

### Added
- WPA target.

## [3.1.0] - 2014-04-22
### Added
- LINQ/Rx/Ix support with implicit type detection.
- PCL targets.

### Changed
- Better hash code implementations.

## [3.0.0] - 2012-05-16
### Added
- Equality comparers.
- New targets: .NET 4.5, Silverlight 4 and 5, Metro, and XBox.

### Fixed
- `CompoundComparer.GetHashCode`

## [2.0.2] - 2012-05-01
### Fixed
- NuGet package metadata.

## [2.0.1] - 2012-05-01
### Fixed
- Removed contract exceptions when passing `null` to `GetHashCode`.

## [2.0.0] - 2012-05-01
### Changed
- More consistent API.

### Added
- Sequence comparers.
- Comparison operators.
- Ability to override how `null` is treated.

## [1.1.0] - 2012-04-22
### Added
- `ToString()` implementations to all comparers to assist debugging.

## [1.0.0] - 2012-04-21
Initial release.
