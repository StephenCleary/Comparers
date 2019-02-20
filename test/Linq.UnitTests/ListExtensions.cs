using System.Collections.Generic;
using System.Linq;
using Nito.Comparers.Linq;
using Xunit;
using System;
using Nito.Comparers;

namespace Linq.UnitTests
{
    public class ListExtensionsUnitTests
    {
        [Fact]
        public void Sort_UsesComparer()
        {
            var input = new List<int?> { int.MaxValue, int.MinValue, null, 0, 1, 2, 3 };
            input.Sort(c => c.OrderBy(x => x % 2 == 0).ThenBy(x => x));
            Assert.Equal(new int?[] { null, 1, 3, int.MaxValue, int.MinValue, 0, 2 }, input);
        }

        [Fact]
        public void Sort_IndexAndCount_UsesComparer()
        {
            var input = new List<int?> { int.MaxValue, int.MinValue, null, 0, 3, 2, 1 };
            input.Sort(2, 4, c => c.OrderBy(x => x % 2 == 0).ThenBy(x => x));
            Assert.Equal(new int?[] { int.MaxValue, int.MinValue, null, 3, 0, 2, 1 }, input);
        }
    }
}
