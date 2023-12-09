# Comparers for Anonymous Types

The `ComparerBuilder.For<T>()` method acts as the normal "entry point" for the fluent API; however, it can't be used if you can't specify the type `T`.

There is a `ForElementsOf` method that is helpful for LINQ queries that project to anonymous types:

```C#
var projection = people.Select(x => new { GivenName = x.FirstName, Surname = x.LastName });
var comparer = ComparerBuilder.ForElementsOf(projection).OrderBy(x => x.Surname);
```

You can also pass delegates to `For` or `ForElementsOf` to determine the type `T` automatically. These delegates are only used to determine the type; they are not actually executed:

```C#
var sample = people.Select(x => new { GivenName = x.FirstName, Surname = x.LastName }).FirstOrDefault();
var comparer = ComparerBuilder.For(() => sample).OrderBy(x => x.Surname);
```

All LINQ-to-Objects methods that take comparers (or equality comparers) have overloads that permit the fluent comparer API right within the LINQ expression:

```C#
var trimmed = people.Select(x => new { GivenName = x.FirstName, Surname = x.LastName })
    .Distinct(c => c.EquateBy(x => x.Surname));
```

Note that these overloads are in the `Nito.Comparers.Linq` namespace.