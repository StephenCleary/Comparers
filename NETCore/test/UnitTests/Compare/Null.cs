using Nito.Comparers;
using Xunit;

namespace Compare_
{
    public class _Null
    {
        [Fact]
        public void ComparesUnequalElementsAsEqual()
        {
            var comparer = ComparerBuilder.For<int>().Null();
            Assert.Equal(0, comparer.Compare(13, 17));
            Assert.True(comparer.Equals(19, 21));
            Assert.Equal(comparer.GetHashCode(13), comparer.GetHashCode(17));
        }

        [Fact]
        public void ComparesNullElementsAsEqualToValueElements()
        {
            var comparer = ComparerBuilder.For<int?>().Null();
            Assert.Equal(0, comparer.Compare(null, 17));
            Assert.Equal(0, comparer.Compare(13, null));
            Assert.True(comparer.Equals(null, 21));
            Assert.True(comparer.Equals(19, null));
            Assert.Equal(comparer.GetHashCode(13), comparer.GetHashCode((object)null));
            Assert.Equal(comparer.GetHashCode(13), comparer.GetHashCode((int?)null));
        }

        [Fact]
        public void ComparesEqualElementsAsEqual()
        {
            var comparer = ComparerBuilder.For<int>().Null();
            Assert.Equal(0, comparer.Compare(13, 13));
            Assert.True(comparer.Equals(13, 13));
            Assert.Equal(comparer.GetHashCode(13), comparer.GetHashCode(13));
        }

        [Fact]
        public void ComparesNullElementsAsEqual()
        {
            var comparer = ComparerBuilder.For<int?>().Null();
            Assert.Equal(0, comparer.Compare(null, null));
            Assert.True(comparer.Equals(null, null));
            Assert.Equal(comparer.GetHashCode((object)null), comparer.GetHashCode((object)null));
            Assert.Equal(comparer.GetHashCode((int?)null), comparer.GetHashCode((int?)null));
        }
    }
}
