using System.Collections.Generic;
using System.Linq;
using Nito.Comparers.Linq;
using Xunit;
using System;
using Nito.Comparers;

namespace Linq.UnitTests
{
    public class EnumerableExtensionsUnitTests
    {
        [Fact]
        public void OrderBy_UsesComparer()
        {
            IEnumerable<int?> input = new int?[] { int.MaxValue, int.MinValue, null, 0, 1, 2, 3 };
            var result = input.OrderBy(x => x, c => c.OrderBy(x => x % 2 == 0).ThenBy(x => x));
            Assert.Equal(new int?[] { null, 1, 3, int.MaxValue, int.MinValue, 0, 2 }, result);
        }

        [Fact]
        public void OrderByDescending_UsesComparer()
        {
            IEnumerable<int?> input = new int?[] { int.MaxValue, int.MinValue, null, 0, 1, 2, 3 };
            var result = input.OrderByDescending(x => x, c => c.OrderBy(x => x % 2 == 0).ThenBy(x => x));
            Assert.Equal(new int?[] { 2, 0, int.MinValue, int.MaxValue, 3, 1, null }, result);
        }

        [Fact]
        public void ThenBy_UsesComparer()
        {
            IEnumerable<int?> input = new int?[] { int.MaxValue, int.MinValue, null, 0, 3, 2, 1 };
            var result = input.OrderBy(x => x, c => c.Null()).ThenBy(x => x, c => c.OrderBy(x => x));
            Assert.Equal(new int?[] { null, int.MinValue, 0, 1, 2, 3, int.MaxValue }, result);
        }

        [Fact]
        public void ThenByDescending_UsesComparer()
        {
            IEnumerable<int?> input = new int?[] { int.MaxValue, int.MinValue, null, 0, 3, 2, 1 };
            var result = input.OrderBy(x => x, c => c.Null()).ThenByDescending(x => x, c => c.OrderBy(x => x));
            Assert.Equal(new int?[] { int.MaxValue, 3, 2, 1, 0, int.MinValue, null }, result);
        }
    }
}
