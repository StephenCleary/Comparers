Comparers Quick Start

So, you've got a nice collection of your POCOs:
    class Person
    {
      public string FirstName { get; }
      public string LastName { get; }
    }
    List<Person> list = ...;

Oh, if only there was an easy way to sort them all by last name and then first name!

Oh, but there is:
    IComparer<Person> nameComparer = Compare<Person>.OrderBy(p => p.LastName).ThenBy(p => p.FirstName);
    list.Sort(nameComparer);

Sweet.

How about having Person implement it?
Let's face it: implementing comparison in .NET is a real pain. IComparable<T>, IComparable, IEquatable<T>, Equals, *and* GetHashCode?!?!
But it's easy now:
    public class Person : ComparableBase<Person>
    {
      static Person
      {
        DefaultComparer = Compare<Person>.OrderBy(p => p.LastName).ThenBy(p => p.FirstName);
      }

      public string FirstName { get; }
      public string LastName { get; }
    }

Want a cute trick? Here's one: true is greater than false, so if you want to order by some weird condition, it's not too hard:
    // Use the default sort order (last name, then first name), EXCEPT all "Smith"s move to the head of the line.
    list.Sort(Compare<Person>.OrderByDescending(p => p.LastName == "Smith").ThenBy(Compare<Person>.Default());

By default, null values are less than anything else, but you can use the same sort of trick to sort them last:
    List<int?> myInts = ...;
    myInts.Sort(Compare<int?>.OrderBy(i => i == null, allowNulls: true).ThenBy(Compare<int?>.Default()));
    // Yeah, we need to pass "allowNulls"; otherwise, the default null-ordering rules will apply.

Need to sort dynamically at runtime? No problem!
    var sortByProperties = new[] { "LastName", "FirstName" };
    IComparer<Person> comparer = Compare<Person>.Null();
    foreach (var propertyName in sortByProperties)
    {
      var localPropertyName = propertyName;
      Func<Person, string> selector = p => p.GetType().GetProperty(localPropertyName).GetValue(p, null) as string;
      comparer = comparer.ThenBy(selector);
    }

What about hash-based containers? Fortunately, every single comparer produced by Comparers also implements equality comparison!
    var nameComparer = Compare<Person>.OrderBy(p => p.LastName).ThenBy(p => p.FirstName);
    Dictionary<Person, Address> dict = new Dictionary<Person, Address>(nameComparer);

Sometimes, you can only define equality. Well, good news: there's an EqualityComparers namespace that parallels the Comparers namespace.
    class Entity : EquatableBase<Entity>
    {
      static Entity()
      {
        DefaultComparer = EqualityComparer<Entity>.EquateBy(e => e.Id);
      }

      public int Id { get; }
    }

Oh, and we have lexicographical sorting, too:
    var nameComparer = Compare<Person>.OrderBy(p => p.LastName).ThenBy(p => p.FirstName);
    List<IEnumerable<Person>> groups = ...;
    groups.Sort(nameComparer.Sequence());

Seriously, what's not to love? :)

Full docs: http://comparers.codeplex.com/documentation