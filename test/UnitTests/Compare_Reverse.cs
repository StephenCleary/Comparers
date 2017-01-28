using System.Collections.Generic;
using System.Linq;
using Nito.Comparers;
using Xunit;
using System;

namespace UnitTests
{
    public class Compare_ReverseUnitTests
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

        [Fact]
        public void PassesGetHashCodeThrough_NonGeneric()
        {
            var comparer = new NongenericEqualityComparer().Reverse();
            Assert.Equal(17, comparer.GetHashCode(13));
        }

        private sealed class NongenericEqualityComparer : IComparer<int>, System.Collections.IEqualityComparer
        {
            public int Compare(int x, int y)
            {
                throw new Exception();
            }

            public new bool Equals(object x, object y)
            {
                throw new Exception();
            }

            public int GetHashCode(object obj)
            {
                return 17;
            }
        }

        [Fact]
        public void NoGetHashCodeImplementation_ThrowsNotImplementedException()
        {
            var comparer = new PlainComparer().Reverse();
            Assert.Throws<NotImplementedException>(() => comparer.GetHashCode(0));
        }

        private sealed class PlainComparer : IComparer<int>
        {
            public int Compare(int x, int y)
            {
                throw new Exception();
            }

            public new bool Equals(object x, object y)
            {
                throw new Exception();
            }
        }

        [Fact]
        public void ToString_DumpsComparer()
        {
            Assert.Equal("Reverse(Default(Int32: IComparable<T>))", ComparerBuilder.For<int>().Default().Reverse().ToString());
        }
    }
}
