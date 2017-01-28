You can specify `dynamic` as the compared type, which allows you to define a comparer that can be applied to multiple, unrelated types:

    var comparer = Compare<dynamic>.OrderByDescending(x => x.Priority);

The comparer defined above can be used with any type that has a `Priority` property.
