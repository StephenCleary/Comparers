![Logo](Comparers.128.png)

# Comparers

The last comparison library you'll ever need! For .NET 4.0, Windows 8.0, Windows Phone Silverlight 7.0, Windows Phone Applications 8.1, .NET 4.0 Client, and Silverlight 4.0.

## Creating Comparers

Install the [NuGet package](https://www.nuget.org/packages/Comparers). There are also NuGet packages for [Reactive extensions](https://www.nuget.org/packages/Comparers.Rx) and [Interactive extensions](https://www.nuget.org/packages/Comparers.Ix) support.

The core comparer types are in the namespace `Comparers`, with extensions in `Comparers.Linq` and equality comparers in `EqualityComparers`.

Let's say you've got a collection of your POCOs:

    class Person
    {
      public string FirstName { get; }
      public string LastName { get; }
    }
    List<Person> list = ...;

Here's an easy way to sort them all by last name and then first name:

    IComparer<Person> nameComparer = Compare<Person>.OrderBy(p => p.LastName).ThenBy(p => p.FirstName);
    list.Sort(nameComparer);

### Implementing Comparable Types

How about having Person implement it?
Let's face it: implementing comparison in .NET is a real pain. `IComparable<T>`, `IComparable`, `IEquatable<T>`, `Object.Equals`, *and* `Object.GetHashCode`?!?!
But it's easy with a base type:

    public class Person : ComparableBase<Person>
    {
      static Person
      {
        DefaultComparer = Compare<Person>.OrderBy(p => p.LastName).ThenBy(p => p.FirstName);
      }

      public string FirstName { get; }
      public string LastName { get; }
    }

`ComparableBase<T>` auto-magically implements all the comparable interfaces, including correct overrides of `Object.Equals` and `Object.GetHashCode`.

### Using Comparers in Hash Containers

What about hash-based containers? Every single comparer produced by the Comparers library also implements equality comparison!

    var nameComparer = Compare<Person>.OrderBy(p => p.LastName).ThenBy(p => p.FirstName);
    Dictionary<Person, Address> dict = new Dictionary<Person, Address>(nameComparer);

### Equality Comparers

Sometimes, you can only define equality. Well, good news: there's an EqualityComparers namespace that parallels the Comparers namespace.

    class Entity : EquatableBase<Entity>
    {
      static Entity()
      {
        DefaultComparer = EqualityComparer<Entity>.EquateBy(e => e.Id);
      }

      public int Id { get; }
    }

### Working with Sequences

Sequences are sorted lexicographically. The `Sequence` operator takes an existing comparer for one type, and defines a lexicographical comparer for sequences of that type:

    var nameComparer = Compare<Person>.OrderBy(p => p.LastName).ThenBy(p => p.FirstName);
    List<IEnumerable<Person>> groups = ...;
    groups.Sort(nameComparer.Sequence());

There's also natural extensions for LINQ, Rx, and Ix:

    IEnumerable<Person> people = ...;
    var anonymousProjection = people.Select(x => new { GivenName = x.FirstName, Surname = x.LastName });
    var reduced = anonymousProjection.Distinct(c => c.EquateBy(x => x.Surname));

### Dynamic Sorting

Need to sort dynamically at runtime? No problem!

    var sortByProperties = new[] { "LastName", "FirstName" };
    IComparer<Person> comparer = Compare<Person>.Null();
    foreach (var propertyName in sortByProperties)
    {
      var localPropertyName = propertyName;
      Func<Person, string> selector = p => p.GetType().GetProperty(localPropertyName).GetValue(p, null) as string;
      comparer = comparer.ThenBy(selector);
    }

### Complex Sorting

Want a cute trick? Here's one: `true` is "greater than" `false`, so if you want to order by some weird condition, it's not too hard:

    // Use the default sort order (last name, then first name), EXCEPT all "Smith"s move to the head of the line.
    list.Sort(Compare<Person>.OrderByDescending(p => p.LastName == "Smith").ThenBy(Compare<Person>.Default());

By default, `null` values are "less than" anything else, but you can use the same sort of trick to sort them last:

    List<int?> myInts = ...;
    myInts.Sort(Compare<int?>.OrderBy(i => i == null, allowNulls: true).ThenBy(Compare<int?>.Default()));
    // Yeah, we need to pass "allowNulls"; otherwise, the default null-ordering rules will apply.

### More?!

For full details, see [the Wiki](https://github.com/StephenCleary/Comparers/wiki).

### What's with the flying saucer?

Other languages provide a comparison operator `<=>`, which is called the "spaceship operator". This library provides similar capabilities for C#, hence the "spaceship logo".

### Alternatives

[ComparerExtensions](https://github.com/jehugaleahsa/ComparerExtensions) is a library that was part of [NList](https://www.nuget.org/packages/NList/) at the time I wrote Comparers; ComparerExtensions has been split from NList and is now a separate library. ComparerExtensions takes a slightly different fluent API approach, particularly around handling null values.