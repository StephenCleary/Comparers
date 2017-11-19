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
        public void EqualReversesComparer()
        {
            var comparer = ComparerBuilder.For<int>().Default().Reverse();
            Assert.True(comparer.Equals(3, 3));
            Assert.False(comparer.Equals(3, -1));
        }
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
        public void PassesEqualThrough()
        {
            var comparer = ComparerBuilder.For<int?>().Default().Reverse();
            var bclComparer = EqualityComparer<int?>.Default;
            var num = new Random().Next();
            Assert.Equal(bclComparer.Equals(num,num),comparer.Equals(num,num));
            Assert.Equal(bclComparer.Equals(num, num+1), comparer.Equals(num, num-1));
        }

        [Fact]
        public void PassesEqualThrough_NonGeneric()
        {
            var comparer = new NongenericEqualityComparer2().Reverse();
            var num = new Random().Next();
            Assert.False(comparer.Equals(num, num));
        }

        [Fact]
        public void PassesEqualThrough_Plain()
        {
            var comparer = new PlainComparer2().Reverse();
            var num = new Random().Next();
            Assert.True(comparer.Equals(num, num+1));
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
        private sealed class NongenericEqualityComparer2 : IComparer<int>, System.Collections.IEqualityComparer
        {
            public int Compare(int x, int y)
            {
                return 0;
            }

            public new bool Equals(object x, object y)
            {
                return false;
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
        }
        private sealed class PlainComparer2 : IComparer<int>
        {
            public int Compare(int x, int y)
            {
                return 0;
            }
        }

        [Fact]
        public void ToString_DumpsComparer()
        {
            Assert.Equal("Reverse(Default(Int32: IComparable<T>))", ComparerBuilder.For<int>().Default().Reverse().ToString());
        }
    }
}
