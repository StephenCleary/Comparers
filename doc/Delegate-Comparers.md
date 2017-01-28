Delegate comparers are also called "anonymous comparers". This kind of "anonymous comparer" is completely different than [[comparers for anonymous types]].

# Why You Shouldn't Use Delegate Comparers

* It can be difficult to maintain the _comparer invariants_ with two unrestricted delegates. The invariants are:
 * `Compare(x, x) == 0`
 * `Compare(x, y) == 0` implies `Compare(y, x) == 0`
 * `(Compare(x, y) == 0) && (Compare(y, z) == 0)` implies `Compare(x, z) == 0`
 * `Compare(x, y) < 0` implies `Compare(y, x) > 0`
 * `Compare(x, y) > 0` implies `Compare(y, x) < 0`
 * `(Compare(x, y) < 0) && (Compare(y, z) < 0)` implies `Compare(x, z) < 0`
 * `(Compare(x, y) > 0) && (Compare(y, z) > 0)` implies `Compare(x, z) > 0`
 * `Compare(x, y) == 0` implies `GetHashCode(x) == GetHashCode(y)`
* All correctly-implemented anonymous comparers are some combination of key comparers (`OrderBy/ThenBy`), compound comparers (`ThenBy`), reverse comparers (`Reverse`), and sequence comparers (`Sequence`); and it's cleaner to use the fluent API to spell out the comparison algorithm explicitly.
* The fluent API will create the correct `GetHashCode` implementation automatically.

For these reasons, I strongly recommend _against_ using the `AnonymousComparer` and `AnonymousEqualityComparer` classes. Anonymous comparers cannot be created using the normal `Compare` class; you have to create them explicitly.

# Using Anonymous Comparers

Anonymous comparers can be created from a delegate:

    // Treat all 0s and 1s as equivalent, all 2s and 3s as equivalent, etc.
    IComparer<int> comparer = new AnonymousComparer<int>
    {
      Compare = (x, y) => Compare<int>.Default().Compare(x / 2, y / 2),
    };

However, it's highly recommended that you provide a corresponding delegate for `GetHashCode` as well:

    // Treat all 0s and 1s as equivalent, all 2s and 3s as equivalent, etc.
    IComparer<int> comparer = new AnonymousComparer<int>
    {
      Compare = (x, y) => Compare<int>.Default().Compare(x / 2, y / 2),
      GetHashCode = x => Compare<int>.Default().GetHashCode(x / 2),
    };

That allows hash-based collections and algorithms to work as expected.

Note that any `AnonymousComparer` can be written more succinctly with the fluent API. The code above can also be written as:

    // Treat all 0s and 1s as equivalent, all 2s and 3s as equivalent, etc.
    IComparer<int> comparer = Compare<int>.OrderBy(x => x / 2);

Not only is the code shorter and more clearly defines the comparer algorithm, it is also easier to maintain since it automatically creates a correct `GetHashCode` implementation.

For equality comparers, you can create a `AnonymousEqualityComparer` instance and set the `Equals` and `GetHashCode` delegates. Note that `GetHashCode` is not optional for equality comparers.