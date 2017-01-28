The `Compare<T>` class acts as the normal "entry point" for the fluent API; however, it can't be used if you can't specify the type `T`.

The `CompareSource` and `EqualityCompareSource` classes provide an alternate entry point; they have several static methods that determine the type `T` automatically. This is helpful for LINQ queries that project to anonymous types:

    var projection = people.Select(x => new { GivenName = x.FirstName, Surname = x.LastName });
    var comparer = CompareSource.ForElementsOf(projection).OrderBy(x => x.Surname);

All LINQ-to-Objects, Reactive Extensions, and Interactive Extensions methods that take comparers (or equality comparers) have overloads that permit the fluent comparer API right within the LINQ expression:

    var trimmed = people.Select(x => new { GivenName = x.FirstName, Surname = x.LastName })
        .Distinct(c => c.EquateBy(x => x.Surname));

Note that these overloads are in the `Comparers.Linq` namespace, and there are different NuGet packages for the [Rx](https://www.nuget.org/packages/Comparers.Rx/) and [Ix](https://www.nuget.org/packages/Comparers.Ix/) overloads.