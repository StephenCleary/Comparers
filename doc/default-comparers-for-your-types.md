# Default Comparers for Your Types

## Implementing `IComparable<T>`, `IEquatable<T>`, and `IComparable`

A type may easily define its default comparer by deriving from `ComparableBase`:

```C#
public sealed class Person : ComparableBase<Person>
{
  static Person()
  {
    DefaultComparer = ComparerBuilder.For<Person>().OrderBy(p => p.LastName).ThenBy(p => p.FirstName);
  }

  public string FirstName { get; set; }
  public string LastName { get; set; }
}
```

`Person.DefaultComparer` is defined by `ComparableBase<Person>`, and is used by both `ComparerBuilder.For<T>().Default()` and `Comparer<T>.Default`.

Note: `DefaultComparer` should only be set once, in the static constructor!

If a class can only implement `IEquatable<T>`, then it can derive from `EquatableBase<T>`, which is identical to `ComparableBase<T>` except `DefaultComparer` is an `IFullEqualityComparer<T>`.

If `Person` has another base class it wants to derive from, it can still define a default comparer by implementing the relevant interfaces directly and using the `Nito.Comparers.Util.ComparableImplementations` class:

```C#
public sealed class Person : MyBase, IEquatable<Person>, IComparable<Person>, IComparable
{
  public static readonly IFullComparer<Person> DefaultComparer = ComparerBuilder.For<Person>().OrderBy(p => p.LastName).ThenBy(p => p.FirstName);

  public string FirstName { get; set; }
  public string LastName { get; set; }

  public override int GetHashCode() =>
      ComparableImplementations.ImplementGetHashCode(DefaultComparer, this);

  public override bool Equals(object obj) =>
      ComparableImplementations.ImplementEquals(DefaultComparer, this, obj);

  public bool Equals(Person other) =>
      ComparableImplementations.ImplementEquals(DefaultComparer, this, other);

  int IComparable.CompareTo(object obj) =>
      ComparableImplementations.ImplementCompareTo(DefaultComparer, this, obj);

  public int CompareTo(Person other) =>
      ComparableImplementations.ImplementCompareTo(DefaultComparer, this, other);
}
```

The XML documentation for the `ComparableImplementations` methods will remind you to provide all the correct overloads and implementations.

## Operator Overloads

Operator overloads (`==`, `!=`, `<`, `<=`, `>`, and `>=`) are provided for any type deriving from `ComparableBaseWithOperators`:

```C#
public sealed class Person : ComparableBaseWithOperators<Person>
{
  static Person()
  {
    DefaultComparer = ComparerBuilder.For<Person>().OrderBy(p => p.LastName).ThenBy(p => p.FirstName);
  }

  public Person(string firstName, string lastName)
  {
    FirstName = firstName;
    LastName = lastName;
  }

  public string FirstName { get; private set; }
  public string LastName { get; private set; }
}
```

Note that operator overloading is only recommended for _immutable types_.

There is also an `EquatableBaseWithOperators<T>` for equatable types.

If you want to implement operators without using these base types, `Nito.Comparers.Util.ComparableImplementations` provides `ImplementOp*` methods that you can use as operator implementations.