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
        public void OrderBy_OrdersByComparer()
        {
            IEnumerable<int?> input = new int?[] { int.MaxValue, int.MinValue, null, 0, 1, 2, 3 };
            var result = input.OrderBy(x => x, c => c.OrderBy(x => x % 2 == 0).ThenBy(x => x));
            Assert.Equal(new int?[] { null, 1, 3, int.MaxValue, int.MinValue, 0, 2 }, result);
        }
    }
}
