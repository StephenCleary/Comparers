# Comparers for Dynamic Types

You can specify `dynamic` as the compared type, which allows you to define a comparer that can be applied to multiple, unrelated types:

    var comparer = ComparerBuilder.For<dynamic>().OrderBy(x => x.Priority, descending: true);

The comparer defined above can be used with any type that has a `Priority` property.