using System.Collections.Generic;
using System.Linq;
using Nito.Comparers;
using Xunit;

namespace ComparerExtensions_
{
    public class _Sequence
    {
        private static readonly int[] three = new[] { 3 };
        private static readonly int[] four = new[] { 4 };
        private static readonly int[] five = new[] { 5 };

        [Fact]
        public void SubstitutesCompareDefaultForComparerDefault()
        {
            var comparer = Comparer<int>.Default.Sequence();
            Assert.Equal(ComparerBuilder.For<int>().Default().Sequence().ToString(), comparer.ToString());

            var list = new[] { three, five, four }.ToList();
            list.Sort(comparer);
            Assert.Equal(new[] { three, four, five }, list);
        }

        [Fact]
        public void SubstitutesCompareDefaultForNull()
        {
            IComparer<int> source = null;
            var comparer = source.Sequence();
            Assert.Equal(ComparerBuilder.For<int>().Default().Sequence().ToString(), comparer.ToString());

            var list = new[] { three, five, four }.ToList();
            list.Sort(comparer);
            Assert.Equal(new[] { three, four, five }, list);
        }

        [Fact]
        public void ShorterSequenceIsSmallerIfElementsAreEqual()
        {
            Assert.True(ComparerBuilder.For<int>().Default().Sequence().Compare(new[] { 3, 4 }, new[] { 3, 4, 5 }) < 0);
            Assert.True(ComparerBuilder.For<int>().Default().Sequence().Compare(new[] { 3, 4, 5 }, new[] { 3, 4 }) > 0);
        }

        [Fact]
        public void EqualSequencesAreEqualIfElementsAreEqual()
        {
            Assert.True(ComparerBuilder.For<int>().Default().Sequence().Compare(new[] { 3, 4 }, new[] { 3, 4 }) == 0);
            Assert.True(ComparerBuilder.For<int>().Default().Sequence().Compare(new[] { 3, 4, 5 }, new[] { 3, 4, 5 }) == 0);
            Assert.Equal(ComparerBuilder.For<int>().Default().Sequence().GetHashCode(new[] { 3, 4 }), ComparerBuilder.For<int>().Default().Sequence().GetHashCode(new[] { 3, 4 }));
        }

        [Fact]
        public void SequenceOrderDeterminedByElementsIfElementsAreNotEqual()
        {
            Assert.True(ComparerBuilder.For<int>().Default().Sequence().Compare(new[] { 3, 4 }, new[] { 3, 5 }) < 0);
            Assert.True(ComparerBuilder.For<int>().Default().Sequence().Compare(new[] { 3, 4, 5 }, new[] { 3, 3, 5 }) > 0);
        }

        [Fact]
        public void SequenceUsesSourceComparerForElementComparisons()
        {
            Assert.True(ComparerBuilder.For<int>().Default().Reverse().Sequence().Compare(new[] { 3, 4 }, new[] { 3, 5 }) > 0);
            Assert.True(ComparerBuilder.For<int>().Default().Reverse().Sequence().Compare(new[] { 3, 4, 5 }, new[] { 3, 3, 5 }) < 0);
        }

        [Fact]
        public void NullIsSmallerThanEmpty()
        {
            Assert.True(ComparerBuilder.For<int>().Default().Sequence().Compare(null, Enumerable.Empty<int>()) < 0);
            Assert.True(ComparerBuilder.For<int>().Default().Sequence().Compare(Enumerable.Empty<int>(), null) > 0);
        }
    }
}
