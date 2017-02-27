# Default Comparers for Your Types

## Implementing IComparable<T>, IEquatable<T>, and IComparable

A type may easily define its default comparer by deriving from `ComparableBase`:

    public sealed class Person : ComparableBase<Person>
    {
      static Person()
      {
        DefaultComparer = ComparerBuilder.For<Person>().OrderBy(p => p.LastName).ThenBy(p => p.FirstName);
      }
    
      public string FirstName { get; set; }
      public string LastName { get; set; }
    }

`Person.DefaultComparer` is defined by `ComparableBase<Person>`, and is used by both `ComparerBuilder.For<T>().Default()` and `Comparer<T>.Default`.

Note: `DefaultComparer` should only be set once, in the static constructor!

If a class can only implement `IEquatable<T>`, then it can derive from `EquatableBase<T>`, which is identical to `ComparableBase<T>` except `DefaultComparer` is an `IFullEqualityComparer<T>`.

If `Person` has another base class it wants to derive from, it can still define a default comparer by implementing the relevant interfaces directly and using the `ComparableImplementations` class:

    public sealed class Person : MyBase, IEquatable<Person>, IComparable<Person>, IComparable
    {
      static public readonly IComparer<Person> DefaultComparer = ComparerBuilder.For<Person>().OrderBy(p => p.LastName).ThenBy(p => p.FirstName);

      public string FirstName { get; set; }
      public string LastName { get; set; }

      public override int GetHashCode()
      {
        return ComparableImplementations.ImplementGetHashCode(DefaultComparer, this);
      }

      public override bool Equals(object obj)
      {
        return ComparableImplementations.ImplementEquals(DefaultComparer, this, obj);
      }

      public bool Equals(Person other)
      {
        return ComparableImplementations.ImplementEquals(DefaultComparer, this, other);
      }

      int IComparable.CompareTo(object obj)
      {
        return ComparableImplementations.ImplementCompareTo(DefaultComparer, this, obj);
      }

      public int CompareTo(Person other)
      {
        return ComparableImplementations.ImplementCompareTo(DefaultComparer, this, other);
      }
    }

The XML documentation for the `ComparableImplementations` methods will remind you to provide all the correct overloads and implementations.

## Operator Overloads

Operator overloads (`==`, `!=`, `<`, `<=`, `>`, and `>=`) are provided for any type deriving from `ComparableBaseWithOperators`:

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

Note that operator overloading is only recommended for _immutable types_.

There is also an `EquatableBaseWithOperators<T>` for equatable types.