# Key Comparers

Most custom comparers are key comparers; that is, they compare their parent objects using a specific "key" property.

## Simple Key Comparers

You can create a simple key comparer by invoking `ComparerBuilder.For<T>().OrderBy`.

Consider a simple `Person` class:

```C#
public sealed class Person
{
    public string Name { get; set; }
}
```

You can write code like this to create a comparer for people that uses `Name` as a key:

```C#
IComparer<Person> comparer = ComparerBuilder.For<Person>().OrderBy(p => p.Name);
```

For equality comparers, use `EqualityComparerBuilder.For<T>().EquateBy`:

```C#
IEqualityComparer<Person> comparer = EqualityComparerBuilder.For<Person>().EquateBy(p => p.Name);
```

# Compound Key Comparers

If we change `Person` to have both first and last names:

```C#
public sealed class Person
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
}
```

Then we can create an ordering by last name and then by first name:

```C#
var comparer = ComparerBuilder.For<Person>().OrderBy(p => p.LastName).ThenBy(p => p.FirstName);
```

`ThenBy` is an [extension method](comparer-extensions.md) that can actually be applied to _any_ comparable.

Similarly, equality comparers may use `ThenEquateBy`.

# Null Values

By default, `OrderBy` and `ThenBy` handle `null` values behind the scenes, using the standard approach (every `null` value is less than every non-`null` value, and every `null` value is equal to every other `null` value).

If you want to override that handling, you can set the `specialNullHandling` parameter to `true`. For example, here is a comparer that works exactly like the default `int?` comparer, except that `null`s are last instead of first:

```C#
var comparer = ComparerBuilder.For<int?>().OrderBy(x => x == null, specialNullHandling: true)
    .ThenBy(ComparerBuilder.For<int?>().Default());
```

The equality comparer methods also provide `specialNullHandling` overloads.