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
        public void Max_OrdersByComparer()
        {
            IEnumerable<int?> input = new int?[] { int.MaxValue, int.MinValue, null, 0, 1, 2, 3 };
            var result = input.Max(c => c.OrderBy(x => x % 2 == 0).ThenBy(x => x));
            Assert.Equal(2, result);
        }
    }
}
