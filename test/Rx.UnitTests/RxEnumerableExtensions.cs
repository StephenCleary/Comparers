using System.Collections.Generic;
using System.Linq;
using Nito.Comparers.Linq;
using Xunit;
using System;
using Nito.Comparers;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Reactive;

namespace Rx.UnitTests
{
    public class RxEnumerableExtensionsUnitTests
    {
        [Fact]
        public async Task Max_UsesComparer()
        {
            var input = new int?[] { int.MaxValue, int.MinValue, null, 0, 1, 2, 3 }.ToObservable();
            var result = await input.Max(c => c.OrderBy(x => x % 2 == 0).ThenBy(x => x)).LastAsync();
            Assert.Equal(2, result);
        }

        [Fact]
        public async Task Max_ResultSelector_UsesComparer()
        {
            var input = new int?[] { int.MaxValue, int.MinValue, null, 0, 1, 2, 3 }.ToObservable();
            var result = await input.Max(x => x, c => c.OrderBy(x => x % 2 == 0).ThenBy(x => x)).LastAsync();
            Assert.Equal(2, result);
        }

        [Fact]
        public async Task MaxBy_UsesComparer()
        {
            var input = new int?[] { int.MaxValue, int.MinValue, null, 0, 1, 2, 3 }.ToObservable();
            var result = await input.MaxBy(x => x, c => c.OrderBy(x => x % 2 == 0).ThenBy(x => x)).LastAsync();
            Assert.Equal(new int?[] { 2 }, result);
        }

        [Fact]
        public async Task Min_UsesComparer()
        {
            var input = new int?[] { int.MaxValue, int.MinValue, null, 0, 1, 2, 3 }.ToObservable();
            var result = await input.Min(c => c.OrderBy(x => x % 2 == 0).ThenBy(x => x)).LastAsync();
            Assert.Equal(1, result); // Observable.Min does not process `null` as a value
        }

        [Fact]
        public async Task Min_ResultSelector_UsesComparer()
        {
            var input = new int?[] { int.MaxValue, int.MinValue, null, 0, 1, 2, 3 }.ToObservable();
            var result = await input.Min(x => x, c => c.OrderBy(x => x % 2 == 0).ThenBy(x => x)).LastAsync();
            Assert.Equal(1, result); // Observable.Min does not process `null` as a value
        }

        [Fact]
        public async Task MinBy_UsesComparer()
        {
            var input = new int?[] { int.MaxValue, int.MinValue, null, 0, 1, 2, 3 }.ToObservable();
            var result = await input.MinBy(x => x, c => c.OrderBy(x => x % 2 == 0).ThenBy(x => x)).LastAsync();
            Assert.Equal(new int?[] { null }, result);
        }

        [Fact]
        public async Task Contains_UsesComparer()
        {
            var input = new int[] { 0, 1, 2 }.ToObservable();
            var result = await input.Contains(5, c => c.EquateBy(x => x % 3)).LastAsync();
            Assert.True(result);
        }

        [Fact]
        public async Task Distinct_UsesComparer()
        {
            var input = new int[] { 0, 1, 2, 3, 4, 5 }.ToObservable();
            var result = await input.Distinct(c => c.EquateBy(x => x % 3)).ToList().LastAsync();
            Assert.Equal(new[] { 0, 1, 2 }, result);
        }

        [Fact]
        public async Task Distinct_KeySelector_UsesComparer()
        {
            var input = new int[] { 0, 1, 2, 3, 4, 5 }.ToObservable();
            var result = await input.Distinct(x => x, c => c.EquateBy(x => x % 3)).ToList().LastAsync();
            Assert.Equal(new[] { 0, 1, 2 }, result);
        }

        [Fact]
        public async Task DistinctUntilChanged_UsesComparer()
        {
            var input = new int[] { 0, 1, 4, 2, 3, 4, 5 }.ToObservable();
            var result = await input.DistinctUntilChanged(c => c.EquateBy(x => x % 3)).ToList().LastAsync();
            Assert.Equal(new[] { 0, 1, 2, 3, 4, 5 }, result);
        }

        [Fact]
        public async Task DistinctUntilChanged_KeySelector_UsesComparer()
        {
            var input = new int[] { 0, 1, 4, 2, 3, 4, 5 }.ToObservable();
            var result = await input.DistinctUntilChanged(x => x, c => c.EquateBy(x => x % 3)).ToList().LastAsync();
            Assert.Equal(new[] { 0, 1, 2, 3, 4, 5 }, result);
        }

        [Fact]
        public async Task GroupBy_UsesComparer()
        {
            var values = new int[] { 1, 2, 3, 4, 5 }.ToObservable();
            var result = await values.GroupBy(x => x, c => c.EquateBy(x => x % 3)).SelectMany(async x => await x.ToList().LastAsync()).ToList().LastAsync();
            Assert.Equal(3, result.Count);
            Assert.Equal(new[] { 1, 4 }, result[0]);
            Assert.Equal(new[] { 2, 5 }, result[1]);
            Assert.Equal(new[] { 3 }, result[2]);
        }

        [Fact]
        public async Task GroupBy_ElementSelector_UsesComparer()
        {
            var values = new int[] { 1, 2, 3, 4, 5 }.ToObservable();
            var result = await values.GroupBy(x => x, x => x, c => c.EquateBy(x => x % 3)).SelectMany(async x => await x.ToList().LastAsync()).ToList().LastAsync();
            Assert.Equal(3, result.Count);
            Assert.Equal(new[] { 1, 4 }, result[0]);
            Assert.Equal(new[] { 2, 5 }, result[1]);
            Assert.Equal(new[] { 3 }, result[2]);
        }

        [Fact]
        public async Task GroupBy_Capacity_UsesComparer()
        {
            var values = new int[] { 1, 2, 3, 4, 5 }.ToObservable();
            var result = await values.GroupBy(x => x, 10, c => c.EquateBy(x => x % 3)).SelectMany(async x => await x.ToList().LastAsync()).ToList().LastAsync();
            Assert.Equal(3, result.Count);
            Assert.Equal(new[] { 1, 4 }, result[0]);
            Assert.Equal(new[] { 2, 5 }, result[1]);
            Assert.Equal(new[] { 3 }, result[2]);
        }

        [Fact]
        public async Task GroupBy_ElementSelectorAndCapacity_UsesComparer()
        {
            var values = new int[] { 1, 2, 3, 4, 5 }.ToObservable();
            var result = await values.GroupBy(x => x, x => x, 10, c => c.EquateBy(x => x % 3)).SelectMany(async x => await x.ToList().LastAsync()).ToList().LastAsync();
            Assert.Equal(3, result.Count);
            Assert.Equal(new[] { 1, 4 }, result[0]);
            Assert.Equal(new[] { 2, 5 }, result[1]);
            Assert.Equal(new[] { 3 }, result[2]);
        }

        [Fact]
        public async Task GroupByUntil_UsesComparer()
        {
            var values = new int[] { 1, 2, 3, 4, 5 }.ToObservable();
            var result = await values.GroupByUntil(x => x, x => Observable.Never<Unit>(), c => c.EquateBy(x => x % 3)).SelectMany(async x => await x.ToList().LastAsync()).ToList().LastAsync();
            Assert.Equal(3, result.Count);
            Assert.Contains(new[] { 1, 4 }, result);
            Assert.Contains(new[] { 2, 5 }, result);
            Assert.Contains(new[] { 3 }, result);
        }

        [Fact]
        public async Task GroupByUntil_ElementSelector_UsesComparer()
        {
            var values = new int[] { 1, 2, 3, 4, 5 }.ToObservable();
            var result = await values.GroupByUntil(x => x, x => x, x => Observable.Never<Unit>(), c => c.EquateBy(x => x % 3)).SelectMany(async x => await x.ToList().LastAsync()).ToList().LastAsync();
            Assert.Equal(3, result.Count);
            Assert.Contains(new[] { 1, 4 }, result);
            Assert.Contains(new[] { 2, 5 }, result);
            Assert.Contains(new[] { 3 }, result);
        }

        [Fact]
        public async Task GroupByUntil_Capacity_UsesComparer()
        {
            var values = new int[] { 1, 2, 3, 4, 5 }.ToObservable();
            var result = await values.GroupByUntil(x => x, x => Observable.Never<Unit>(), 10, c => c.EquateBy(x => x % 3)).SelectMany(async x => await x.ToList().LastAsync()).ToList().LastAsync();
            Assert.Equal(3, result.Count);
            Assert.Contains(new[] { 1, 4 }, result);
            Assert.Contains(new[] { 2, 5 }, result);
            Assert.Contains(new[] { 3 }, result);
        }

        [Fact]
        public async Task GroupByUntil_ElementSelectorAndCapacity_UsesComparer()
        {
            var values = new int[] { 1, 2, 3, 4, 5 }.ToObservable();
            var result = await values.GroupByUntil(x => x, x => x, x => Observable.Never<Unit>(), 10, c => c.EquateBy(x => x % 3)).SelectMany(async x => await x.ToList().LastAsync()).ToList().LastAsync();
            Assert.Equal(3, result.Count);
            Assert.Contains(new[] { 1, 4 }, result);
            Assert.Contains(new[] { 2, 5 }, result);
            Assert.Contains(new[] { 3 }, result);
        }

        [Fact]
        public async Task SequenceEqual_UsesComparer()
        {
            var first = new int[] { 0, 1, 2 }.ToObservable();
            var second = new int[] { 15, 4, 2 }.ToObservable();
            var result = await first.SequenceEqual(second, c => c.EquateBy(x => x % 3)).LastAsync();
            Assert.True(result);
        }

        [Fact]
        public async Task SequenceEqual_Enumerable_UsesComparer()
        {
            var first = new int[] { 0, 1, 2 }.ToObservable();
            var second = new int[] { 15, 4, 2 };
            var result = await first.SequenceEqual(second, c => c.EquateBy(x => x % 3)).LastAsync();
            Assert.True(result);
        }

        [Fact]
        public async Task ToDictionary_UsesComparer()
        {
            var values = new int[] { 1, 2, 3, 4 }.ToObservable();
            await Assert.ThrowsAnyAsync<ArgumentException>(async () => await values.ToDictionary(x => x, c => c.EquateBy(x => x % 3)).LastAsync());
        }

        [Fact]
        public async Task ToDictionary_ElementSelector_UsesComparer()
        {
            var values = new int[] { 1, 2, 3, 4 }.ToObservable();
            await Assert.ThrowsAnyAsync<ArgumentException>(async () => await values.ToDictionary(x => x, x => x, c => c.EquateBy(x => x % 3)).LastAsync());
        }

        [Fact]
        public async Task ToLookup_UsesComparer()
        {
            var values = new int[] { 1, 2, 3, 4 }.ToObservable();
            var observableResult = await values.ToLookup(x => x, c => c.EquateBy(x => x % 3)).SelectMany(x => x).ToList().LastAsync();
            var result = observableResult.Select(x => x.AsEnumerable()).ToList();
            Assert.Contains(new[] { 3 }, result);
            Assert.Contains(new[] { 1, 4 }, result);
            Assert.Contains(new[] { 2 }, result);
        }

        [Fact]
        public async Task ToLookup_ElementSelector_UsesComparer()
        {
            var values = new int[] { 1, 2, 3, 4 }.ToObservable();
            var observableResult = await values.ToLookup(x => x, x => x, c => c.EquateBy(x => x % 3)).SelectMany(x => x).ToList().LastAsync();
            var result = observableResult.Select(x => x.AsEnumerable()).ToList();
            Assert.Contains(new[] { 3 }, result);
            Assert.Contains(new[] { 1, 4 }, result);
            Assert.Contains(new[] { 2 }, result);
        }
    }
}
