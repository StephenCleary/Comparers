using Nito.Comparers;
using Xunit;

namespace EqualityCompare_
{
    public class _Null
    {
        [Fact]
        public void ComparesUnequalElementsAsEqual()
        {
            var comparer = EqualityComparerBuilder.For<int>().Null();
            var objectComparer = comparer as System.Collections.IEqualityComparer;
            Assert.True(comparer.Equals(13, 17));
            Assert.True(objectComparer.Equals(13, 17));
            Assert.Equal(comparer.GetHashCode(13), comparer.GetHashCode(17));
            Assert.Equal(objectComparer.GetHashCode(13), objectComparer.GetHashCode(17));
        }

        [Fact]
        public void ComparesNullElementsAsEqualToValueElements()
        {
            var comparer = EqualityComparerBuilder.For<int?>().Null();
            var objectComparer = comparer as System.Collections.IEqualityComparer;
            Assert.True(comparer.Equals(null, 21));
            Assert.True(objectComparer.Equals(null, 21));
            Assert.True(comparer.Equals(19, null));
            Assert.True(objectComparer.Equals(19, null));
            Assert.Equal(comparer.GetHashCode(13), comparer.GetHashCode(null));
            Assert.Equal(objectComparer.GetHashCode(13), objectComparer.GetHashCode(null));
        }

        [Fact]
        public void ComparesEqualElementsAsEqual()
        {
            var comparer = EqualityComparerBuilder.For<int>().Null();
            var objectComparer = comparer as System.Collections.IEqualityComparer;
            Assert.True(comparer.Equals(13, 13));
            Assert.True(objectComparer.Equals(13, 13));
            Assert.Equal(comparer.GetHashCode(13), comparer.GetHashCode(13));
            Assert.Equal(objectComparer.GetHashCode(13), objectComparer.GetHashCode(13));
        }

        [Fact]
        public void ComparesNullElementsAsEqual()
        {
            var comparer = EqualityComparerBuilder.For<int?>().Null();
            var objectComparer = comparer as System.Collections.IEqualityComparer;
            Assert.True(comparer.Equals(null, null));
            Assert.True(objectComparer.Equals(null, null));
            Assert.Equal(comparer.GetHashCode(null), comparer.GetHashCode(null));
            Assert.Equal(objectComparer.GetHashCode(null), objectComparer.GetHashCode(null));
        }

        [Fact]
        public void ToString_DumpsComparer()
        {
            Assert.Equal("Null", EqualityComparerBuilder.For<int>().Null().ToString());
        }
    }
}
