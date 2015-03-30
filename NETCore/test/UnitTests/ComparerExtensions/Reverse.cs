using System.Collections.Generic;
using System.Linq;
using Nito.Comparers;
using Xunit;

namespace ComparerExtensions_
{
    public class _Reverse
    {
        [Fact]
        public void SubstitutesCompareDefaultForComparerDefault()
        {
            var comparer = Comparer<int>.Default.Reverse();
            Assert.Equal(ComparerBuilder.For<int>().Default().Reverse().ToString(), comparer.ToString());

            var list = Enumerable.Range(0, 5).ToList();
            list.Sort(comparer);
            Assert.Equal(new[] { 4, 3, 2, 1, 0 }, list);
        }

        [Fact]
        public void SubstitutesCompareDefaultForNull()
        {
            IComparer<int> source = null;
            var comparer = source.Reverse();
            Assert.Equal(ComparerBuilder.For<int>().Default().Reverse().ToString(), comparer.ToString());

            var list = Enumerable.Range(0, 5).ToList();
            list.Sort(comparer);
            Assert.Equal(new[] { 4, 3, 2, 1, 0 }, list);
        }

        [Fact]
        public void ReversesComparer()
        {
            var list = Enumerable.Range(0, 5).ToList();
            list.Sort(ComparerBuilder.For<int>().Default().Reverse());
            Assert.Equal(new[] { 4, 3, 2, 1, 0 }, list);
        }

        [Fact]
        public void ReversesComparerWithNull()
        {
            var list = Enumerable.Repeat<int?>(null, 1).Concat(Enumerable.Range(0, 5).Cast<int?>()).ToList();
            list.Sort(ComparerBuilder.For<int?>().Default().Reverse());
            Assert.Equal(new int?[] { 4, 3, 2, 1, 0, null }, list);
        }

        [Fact]
        public void PassesGetHashCodeThrough()
        {
            var comparer = ComparerBuilder.For<int?>().Default().Reverse();
            var bclComparer = EqualityComparer<int?>.Default;
            Assert.Equal(bclComparer.GetHashCode(7) == bclComparer.GetHashCode(13), comparer.GetHashCode(7) == comparer.GetHashCode(13));
        }
    }
}
