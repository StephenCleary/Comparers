# Simple Key Comparers

You can create a simple key comparer by invoking `Compare<T>.OrderBy` (or `OrderByDescending`).

Consider a simple `Person` class:

    public sealed class Person
    {
      public string Name { get; set; }
    }

You can write code like this to create a comparer for people that uses `Name` as a key:

    IComparer<Person> comparer = Compare<Person>.OrderBy(p => p.Name);

Of course, you don't have to save the comparer in a variable. If you're only going to use it once, you can just create the comparer as needed:

    List<Person> people = ...;
    people.Sort(Compare<Person>.OrderBy(p => p.Name));

For equality comparers, use `EqualityCompare<T>.EquateBy`:

    IEqualityComparer<Person> comparer = EqualityCompare<Person>.EquateBy(p => p.Name);

# Compound Key Comparers

If we change `Person` to have both first and last names:

    public sealed class Person
    {
      public string FirstName { get; set; }
      public string LastName { get; set; }
    }

Then we can create an ordering by last name and then by first name:

    var comparer = Compare<Person>.OrderBy(p => p.LastName).ThenBy(p => p.FirstName);

`ThenBy` is an [[extension method|Comparer Extensions]] that can actually be applied to _any_ comparable.

Similarly, equality comparers may use `ThenEquateBy`.

# Null Values

By default, `OrderBy` and `ThenBy` handle `null` values behind the scenes, using the standard approach (every `null` value is less than every non-`null` value, and every `null` value is equal to every other `null` value).

If you want to override that handling, you can set the `allowNulls` parameter to `true`. For example, here is a comparer that works exactly like the default `int?` comparer, except that `null`s are last instead of first:

    var comparer = Compare<int?>.OrderBy(x => x == null, allowNulls: true)
        .ThenBy(Compare<int?>.Default());

The equality comparer methods also provide `allowNulls` overloads.
