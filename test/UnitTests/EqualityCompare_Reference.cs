using Nito.Comparers;
using Xunit;

namespace UnitTests
{
    public class EqualityCompare_ReferenceUnitTests
    {
        [Fact]
        public void IdenticalObjectsAreEqual()
        {
            var comparer = EqualityComparerBuilder.For<object>().Reference();
            var objectComparer = comparer as System.Collections.IEqualityComparer;
            var obj = new object();
            Assert.Equal(comparer.GetHashCode(obj), objectComparer.GetHashCode(obj));
            Assert.True(comparer.Equals(obj, obj));
            Assert.True(objectComparer.Equals(obj, obj));
        }

        [Fact]
        public void UsesReferenceEqualityComparerForSequences()
        {
            var threeA = new[] { 3 };
            var threeB = new[] { 3 };
            var comparer = EqualityComparerBuilder.For<int[]>().Reference();
            var objectComparer = comparer as System.Collections.IEqualityComparer;
            Assert.False(comparer.Equals(threeA, threeB));
            Assert.False(objectComparer.Equals(threeA, threeB));
        }

        [Fact]
        public void NullIsNotEqualToValue()
        {
            var comparer = EqualityComparerBuilder.For<object>().Reference();
            var objectComparer = comparer as System.Collections.IEqualityComparer;
            var obj = new object();
            Assert.False(comparer.Equals(obj, null));
            Assert.False(objectComparer.Equals(obj, null));
        }

        [Fact]
        public void NullSequenceNotEqualToEmptySequence()
        {
            var comparer = EqualityComparerBuilder.For<int[]>().Reference();
            var objectComparer = comparer as System.Collections.IEqualityComparer;
            Assert.False(comparer.Equals(null, new int[0]));
            Assert.False(objectComparer.Equals(null, new int[0]));
        }

        [Fact]
        public void NullSequenceIsEqualToNullSequence()
        {
            var comparer = EqualityComparerBuilder.For<int[]>().Reference();
            var objectComparer = comparer as System.Collections.IEqualityComparer;
            Assert.Equal(comparer.GetHashCode(null), comparer.GetHashCode(null));
            Assert.Equal(objectComparer.GetHashCode(null), objectComparer.GetHashCode(null));
            Assert.True(comparer.Equals(null, null));
            Assert.True(objectComparer.Equals(null, null));
        }

        [Fact]
        public void ToString_DumpsComparer()
        {
            Assert.Equal("Reference", EqualityComparerBuilder.For<int[]>().Reference().ToString());
        }
    }
}
