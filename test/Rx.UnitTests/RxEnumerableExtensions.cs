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
        public async Task Max_OrdersByComparer()
        {
            var input = new int?[] { int.MaxValue, int.MinValue, null, 0, 1, 2, 3 }.ToObservable();
            var result = await input.Max(c => c.OrderBy(x => x % 2 == 0).ThenBy(x => x)).LastAsync();
            Assert.Equal(2, result);
        }
    }
}
