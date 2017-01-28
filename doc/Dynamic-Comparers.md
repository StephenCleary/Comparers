Sometimes you need to define a comparer at run-time; for example, applying a user-selected multi-column sort. The comparer extension methods work on any type of comparer, so you can construct a comparer starting from the `Null` comparer like this:

    var compareColumns = new[] { "LastName", "FirstName" };

    IComparer<Person> comparer = Compare<Person>.Null();
    foreach (var column in compareColumns)
    {
      var localColumn = column;
      Func<Person, string> selector = p => p.GetType().GetProperty(localColumn).GetValue(p, null) as string;
      comparer = comparer.ThenBy(selector);
    }

Equality comparers also support a `Null` comparer.
