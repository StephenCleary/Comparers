using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Nito.Comparers;
using Nito.EqualityComparers;
using Xunit;

namespace EqualityCompare_
{
    public class _Default
    {
        [Fact]
        public void IsEquivalentToEqualityComparerDefault()
        {
            var a = 3;
            var b = 3;
            var c = 5;

            var defaultComparer = EqualityComparerBuilder.For<int>().Default();
            var netDefaultComparer = EqualityComparer<int>.Default;
            var defaultObjectComparer = defaultComparer as System.Collections.IEqualityComparer;
            Assert.Equal(netDefaultComparer.GetHashCode(a), defaultComparer.GetHashCode(a));
            Assert.Equal(defaultComparer.GetHashCode(a), defaultObjectComparer.GetHashCode(a));
            Assert.Equal(netDefaultComparer.GetHashCode(b), defaultComparer.GetHashCode(b));
            Assert.Equal(defaultComparer.GetHashCode(b), defaultObjectComparer.GetHashCode(b));
            Assert.Equal(netDefaultComparer.GetHashCode(c), defaultComparer.GetHashCode(c));
            Assert.Equal(defaultComparer.GetHashCode(c), defaultObjectComparer.GetHashCode(c));
            Assert.Equal(netDefaultComparer.GetHashCode(7) == netDefaultComparer.GetHashCode(13), defaultComparer.GetHashCode(7) == defaultComparer.GetHashCode(13));
            Assert.Equal(defaultComparer.GetHashCode(7) == defaultComparer.GetHashCode(13), defaultObjectComparer.GetHashCode(7) == defaultObjectComparer.GetHashCode(13));
            Assert.Equal(netDefaultComparer.Equals(a, b), defaultComparer.Equals(a, b));
            Assert.Equal(defaultComparer.Equals(a, b), defaultObjectComparer.Equals(a, b));
            Assert.Equal(netDefaultComparer.Equals(a, c), defaultComparer.Equals(a, c));
            Assert.Equal(defaultComparer.Equals(a, c), defaultObjectComparer.Equals(a, c));
        }

        [Fact]
        public void UsesSequenceEqualityComparerForSequences()
        {
            var threeA = new[] { 3 };
            var threeB = new[] { 3 };
            var five = new[] { 5 };
            var comparer = EqualityComparerBuilder.For<int[]>().Default();
            var objectComparer = comparer as System.Collections.IEqualityComparer;
            Assert.Equal(comparer.GetHashCode(threeA), comparer.GetHashCode(threeB));
            Assert.Equal(objectComparer.GetHashCode(threeA), objectComparer.GetHashCode(threeB));
            Assert.True(comparer.Equals(threeA, threeB));
            Assert.True(objectComparer.Equals(threeA, threeB));
            Assert.False(comparer.Equals(threeB, five));
            Assert.False(objectComparer.Equals(threeB, five));
        }

        [Fact]
        public void NullIsNotEqualToValue()
        {
            var comparer = EqualityComparerBuilder.For<int?>().Default();
            var objectComparer = comparer as System.Collections.IEqualityComparer;
            Assert.False(comparer.Equals(3, null));
            Assert.False(objectComparer.Equals(3, null));
        }

        [Fact]
        public void NullSequenceNotEqualToEmptySequence()
        {
            var comparer = EqualityComparerBuilder.For<int[]>().Default();
            var objectComparer = comparer as System.Collections.IEqualityComparer;
            Assert.False(comparer.Equals(null, new int[0]));
            Assert.False(objectComparer.Equals(null, new int[0]));
        }

        [Fact]
        public void NullIsEqualToNull()
        {
            var comparer = EqualityComparerBuilder.For<int?>().Default();
            var objectComparer = comparer as System.Collections.IEqualityComparer;
            Assert.Equal(comparer.GetHashCode(null), comparer.GetHashCode(null));
            Assert.Equal(objectComparer.GetHashCode(null), objectComparer.GetHashCode(null));
            Assert.True(comparer.Equals(null, null));
            Assert.True(objectComparer.Equals(null, null));
        }

        [Fact]
        public void NullSequenceIsEqualToNullSequence()
        {
            var comparer = EqualityComparerBuilder.For<int[]>().Default();
            var objectComparer = comparer as System.Collections.IEqualityComparer;
            Assert.Equal(comparer.GetHashCode(null), comparer.GetHashCode(null));
            Assert.Equal(objectComparer.GetHashCode(null), objectComparer.GetHashCode(null));
            Assert.True(comparer.Equals(null, null));
            Assert.True(objectComparer.Equals(null, null));
        }

        [Fact]
        public void DefaultForString_IsDefaultComparer()
        {
            // Ensure string default comparer is not a sequence comparer over chars.
            Assert.NotEqual(EqualityComparerBuilder.For<char>().Default().EquateSequence().ToString(), EqualityComparerBuilder.For<string>().Default().ToString());
        }
    }
}
