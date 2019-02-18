using System.Collections.Generic;
using System.Linq;
using Nito.Comparers.Linq;
using Xunit;
using System;
using Nito.Comparers;
using System.Reactive.Linq;
using System.Threading.Tasks;

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
            Assert.Equal(1, result);
        }

        [Fact]
        public async Task Min_ResultSelector_UsesComparer()
        {
            var input = new int?[] { int.MaxValue, int.MinValue, null, 0, 1, 2, 3 }.ToObservable();
            var result = await input.Min(x => x, c => c.OrderBy(x => x % 2 == 0).ThenBy(x => x)).LastAsync();
            Assert.Equal(1, result);
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
    }
}
