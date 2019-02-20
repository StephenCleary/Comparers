using System.Collections.Generic;
using System.Linq;
using Nito.Comparers.Linq;
using Xunit;
using System;
using Nito.Comparers;

namespace Linq.UnitTests
{
    public class ArrayExtensionsUnitTests
    {
        [Fact]
        public void Sort_UsesComparer()
        {
            var input = new int?[] { int.MaxValue, int.MinValue, null, 0, 1, 2, 3 };
            input.Sort(c => c.OrderBy(x => x % 2 == 0).ThenBy(x => x));
            Assert.Equal(new int?[] { null, 1, 3, int.MaxValue, int.MinValue, 0, 2 }, input);
        }

        [Fact]
        public void Sort_IndexAndCount_UsesComparer()
        {
            var input = new int?[] { int.MaxValue, int.MinValue, null, 0, 3, 2, 1 };
            input.Sort(2, 4, c => c.OrderBy(x => x % 2 == 0).ThenBy(x => x));
            Assert.Equal(new int?[] { int.MaxValue, int.MinValue, null, 3, 0, 2, 1 }, input);
        }

        [Fact]
        public void Sort_Items_UsesComparer()
        {
            var input = new int?[] { int.MaxValue, int.MinValue, null, 0, 1, 2, 3 };
            var items = new int[] { 0, 1, 2, 3, 4, 5, 6 };
            input.Sort(items, c => c.OrderBy(x => x % 2 == 0).ThenBy(x => x));
            Assert.Equal(new int?[] { null, 1, 3, int.MaxValue, int.MinValue, 0, 2 }, input);
            Assert.Equal(new int[] { 2, 4, 6, 0, 1, 3, 5 }, items);
        }

        [Fact]
        public void Sort_Items_IndexAndCount_UsesComparer()
        {
            var input = new int?[] { int.MaxValue, int.MinValue, null, 0, 3, 2, 1 };
            var items = new int[] { 0, 1, 2, 3, 4, 5, 6 };
            input.Sort(items, 2, 4, c => c.OrderBy(x => x % 2 == 0).ThenBy(x => x));
            Assert.Equal(new int?[] { int.MaxValue, int.MinValue, null, 3, 0, 2, 1 }, input);
            Assert.Equal(new int[] { 0, 1, 2, 4, 3, 5, 6 }, items);
        }
    }
}
