![Logo](Comparers.128.png)

# Comparers

The last comparison library you'll ever need! For `netstandard1.0` (including .NET 4.5, .NET Core 1.0, Xamarin.iOS 10, Xamarin.Android 7, Mono 4.6, Universal Windows 10, Windows 8, Windows Phone Applications 8.1, and Windows Phone Silverlight 8.0).

[![AppVeyor](https://img.shields.io/appveyor/ci/StephenCleary/Comparers.svg?style=plastic)](https://ci.appveyor.com/project/StephenCleary/Comparers) [![Coveralls](https://img.shields.io/coveralls/StephenCleary/Comparers.svg?style=plastic)](https://coveralls.io/r/StephenCleary/Comparers)
[![NuGet Pre Release](https://img.shields.io/nuget/vpre/Nito.Comparers.svg?style=plastic)](https://www.nuget.org/packages/Nito.Comparers/)

[API Docs](http://dotnetapis.com/pkg/Nito.Comparers)

## Creating Comparers

Install the [NuGet package](https://www.nuget.org/packages/Nito.Comparers). By default, this includes the [extension package for LINQ](https://www.nuget.org/packages/Nito.Comparers.Linq) support. There are also extension packages available for [Reactive extensions](https://www.nuget.org/packages/Nito.Comparers.Rx) and [Interactive extensions](https://www.nuget.org/packages/Nito.Comparers.Ix) support.

The comparer types are in the namespace `Nito.Comparers`.

Let's say you've got a collection of your POCOs:

    class Person
    {
      public string FirstName { get; }
      public string LastName { get; }
    }
    List<Person> list = ...;

Here's an easy way to sort them all by last name and then first name:

    IComparer<Person> nameComparer = ComparerBuilder.For<Person>().OrderBy(p => p.LastName).ThenBy(p => p.FirstName);
    list.Sort(nameComparer);

### Implementing Comparable Types

How about having Person implement it?
Let's face it: implementing comparison in .NET is a real pain. `IComparable<T>`, `IComparable`, `IEquatable<T>`, `Object.Equals`, *and* `Object.GetHashCode`?!?!
But it's easy with a base type:

    class Person : ComparableBase<Person>
    {
      static Person
      {
        DefaultComparer = ComparerBuilder.For<Person>().OrderBy(p => p.LastName).ThenBy(p => p.FirstName);
      }

      public string FirstName { get; }
      public string LastName { get; }
    }

`ComparableBase<T>` auto-magically implements all the comparable interfaces, including correct overrides of `Object.Equals` and `Object.GetHashCode`.

### Using Comparers in Hash Containers

What about hash-based containers? Every single comparer produced by the Comparers library also implements equality comparison!

    IEqualityComparer<Person> nameComparer = ComparerBuilder.For<Person>().OrderBy(p => p.LastName).ThenBy(p => p.FirstName);
    Dictionary<Person, Address> dict = new Dictionary<Person, Address>(nameComparer);

### Equality Comparers

Sometimes, you can only define equality. Well, good news: there are equality comparer types that parallel the full comparer types.

    class Entity : EquatableBase<Entity>
    {
      static Entity()
      {
        DefaultComparer = EqualityComparerBuilder.For<Entity>().EquateBy(e => e.Id);
      }

      public int Id { get; }
    }

### Working with Sequences

Sequences are sorted lexicographically. The `Sequence` operator takes an existing comparer for one type, and defines a lexicographical comparer for sequences of that type:

    var nameComparer = ComparerBuilder.For<Person>().OrderBy(p => p.LastName).ThenBy(p => p.FirstName);
    List<IEnumerable<Person>> groups = ...;
    groups.Sort(nameComparer.Sequence());

There's also natural extensions for LINQ, Rx, and Ix:

    IEnumerable<Person> people = ...;
    var anonymousProjection = people.Select(x => new { GivenName = x.FirstName, Surname = x.LastName });
    var reduced = anonymousProjection.Distinct(c => c.EquateBy(x => x.Surname));

### Dynamic Sorting

Need to sort dynamically at runtime? No problem!

    var sortByProperties = new[] { "LastName", "FirstName" };
    IComparer<Person> comparer = ComparerBuilder.For<Person>().Null();
    foreach (var propertyName in sortByProperties)
    {
      var localPropertyName = propertyName;
      Func<Person, string> selector = p => p.GetType().GetProperty(localPropertyName).GetValue(p, null) as string;
      comparer = comparer.ThenBy(selector);
    }

### Complex Sorting

Want a cute trick? Here's one: `true` is "greater than" `false`, so if you want to order by some weird condition, it's not too hard:

    // Use the default sort order (last name, then first name), EXCEPT all "Smith"s move to the head of the line.
    var comparer = ComparerBuilder.For<Person>().OrderBy(p => p.LastName == "Smith", descending: true)
        .ThenBy(ComparerBuilder.For<Person>().Default());
    list.Sort(comparer);

By default, `null` values are "less than" anything else, but you can use the same sort of trick to sort them last:

    List<int?> myInts = ...;
    var comparer = ComparerBuilder.For<int?>().OrderBy(i => i == null, specialNullHandling: true)
        .ThenBy(ComparerBuilder.For<int?>().Default());
    myInts.Sort(comparer);
    // Yeah, we need to pass "specialNullHandling"; otherwise, the default null-ordering rules will apply.

### More?!

For full details, see [the detailed docs](doc).

### What's with the flying saucer?

Other languages provide a comparison operator `<=>`, which is called the "spaceship operator". This library provides similar capabilities for C#, hence the "spaceship logo".
