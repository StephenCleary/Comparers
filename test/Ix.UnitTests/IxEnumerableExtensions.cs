using System.Collections.Generic;
using System.Linq;
using Nito.Comparers.Linq;
using Xunit;
using System;
using Nito.Comparers;

namespace Ix.UnitTests
{
    public class IxEnumerableExtensionsUnitTests
    {
        [Fact]
        public void Max_UsesComparer()
        {
            IEnumerable<int?> input = new int?[] { int.MaxValue, int.MinValue, null, 0, 1, 2, 3 };
            var result = input.Max(c => c.OrderBy(x => x % 2 == 0).ThenBy(x => x));
            Assert.Equal(2, result);
        }

        [Fact]
        public void MaxBy_UsesComparer()
        {
            IEnumerable<int?> input = new int?[] { int.MaxValue, int.MinValue, null, 0, 1, 2, 3 };
            var result = input.MaxBy(x => x, c => c.OrderBy(x => x % 2 == 0).ThenBy(x => x));
            Assert.Equal(new int?[] { 2 }, result);
        }

        [Fact]
        public void Min_UsesComparer()
        {
            IEnumerable<int?> input = new int?[] { int.MaxValue, int.MinValue, null, 0, 1, 2, 3 };
            var result = input.Min(c => c.OrderBy(x => x % 2 == 0).ThenBy(x => x));
            Assert.Null(result);
        }

        [Fact]
        public void MinBy_UsesComparer()
        {
            IEnumerable<int?> input = new int?[] { int.MaxValue, int.MinValue, null, 0, 1, 2, 3 };
            var result = input.MinBy(x => x, c => c.OrderBy(x => x % 2 == 0).ThenBy(x => x));
            Assert.Equal(new int?[] { null }, result);
        }

        [Fact]
        public void Distinct_UsesComparer()
        {
            IEnumerable<int> values = new int[] { 1, 2, 3, 4, 5 };
            var result = values.Distinct(x => x, c => c.EquateBy(x => x % 3));
            Assert.Equal(new int[] { 1, 2, 3 }, result);
        }

        [Fact]
        public void DistinctUntilChanged_UsesComparer()
        {
            var input = new int[] { 0, 1, 4, 2, 3, 4, 5 };
            var result = input.DistinctUntilChanged(c => c.EquateBy(x => x % 3));
            Assert.Equal(new[] { 0, 1, 2, 3, 4, 5 }, result);
        }

        [Fact]
        public void DistinctUntilChanged_KeySelector_UsesComparer()
        {
            var input = new int[] { 0, 1, 4, 2, 3, 4, 5 };
            var result = input.DistinctUntilChanged(x => x, c => c.EquateBy(x => x % 3));
            Assert.Equal(new[] { 0, 1, 2, 3, 4, 5 }, result);
        }
    }
}
