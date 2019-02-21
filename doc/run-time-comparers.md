# Run-Time Comparers

Sometimes you need to define comparer logic at run-time; for example, applying a user-selected multi-column sort. The comparer extension methods work on any type of comparer, so you can construct a comparer starting from the `Null` comparer like this:

```C#
var compareColumns = new[] { "LastName", "FirstName" };

IComparer<Person> comparer = ComparerBuilder.For<Person>().Null();
foreach (var column in compareColumns)
{
  var localColumn = column;
  Func<Person, string> selector = p => p.GetType().GetProperty(localColumn).GetValue(p, null) as string;
  comparer = comparer.ThenBy(selector);
}
```

Equality comparers also support a `Null` comparer.

The `Null` comparer by itself will consider all objects equivalent, even `null` values. `Null` comparers are generally only used when dynamically building comparers at runtime.