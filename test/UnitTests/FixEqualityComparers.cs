using Nito.Comparers;
using Nito.Comparers.Fixes;
using System;
using Xunit;

namespace UnitTests
{
    public class FixEqualityComparersUnitTests
    {
        [Fact]
        public void GetHashCode_StandardizeNull_HandlesNull()
        {
            // StringComparer by itself throws for GetHashCode(null)
            Assert.ThrowsAny<Exception>(() => StringComparer.Ordinal.GetHashCode(null));

            // StandardizeNull protects it from null values.
            var comparer = StringComparer.Ordinal.WithStandardNullHandlingForEquality();
            comparer.GetHashCode(null);

            // While passing through non-null values.
            Assert.Equal(StringComparer.Ordinal.GetHashCode("test"), comparer.GetHashCode("test"));
        }

        [Fact]
        public void Equals_StandardizeNull_HandlesNull()
        {
            // This comparer will cause NREs.
            var dangerousComparer = EqualityComparerBuilder.For<object>().EquateBy(x => x.GetHashCode(), specialNullHandling: true);
            Assert.ThrowsAny<NullReferenceException>(() => dangerousComparer.Equals(null, null));

            // StandardizeNull protects it from null values.
            var comparer = dangerousComparer.WithStandardNullHandlingForEquality();
            Assert.True(comparer.Equals(null, null));

            // While passing through non-null values.
            Assert.Equal(dangerousComparer.Equals("test", "test"), comparer.Equals("test", "test"));
        }

        [Fact]
        public void ToString_DumpsComparer()
        {
            Assert.StartsWith("StandardNullHandling", EqualityComparerBuilder.For<int?>().Null().WithStandardNullHandlingForEquality().ToString());
        }
    }
}
