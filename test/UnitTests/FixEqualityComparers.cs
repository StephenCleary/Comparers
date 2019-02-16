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
            StringComparer.Ordinal.WithStandardNullHandlingForEquality().GetHashCode(null);
        }

        [Fact]
        public void Equals_StandardizeNull_HandlesNull()
        {
            // This comparer will cause NREs.
            var dangerousComparer = EqualityComparerBuilder.For<object>().EquateBy(x => x.GetHashCode(), specialNullHandling: true);
            Assert.ThrowsAny<NullReferenceException>(() => dangerousComparer.Equals(null, null));

            // StandardizeNull protects it from null values.
            Assert.True(dangerousComparer.WithStandardNullHandlingForEquality().Equals(null, null));
        }

        [Fact]
        public void ToString_DumpsComparer()
        {
            Assert.StartsWith("StandardNullHandling", EqualityComparerBuilder.For<int?>().Null().WithStandardNullHandlingForEquality().ToString());
        }
    }
}
