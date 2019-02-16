using Nito.Comparers;
using Nito.Comparers.Fixes;
using System;
using System.Collections.Generic;
using Xunit;

namespace UnitTests
{
    public class FixComparersUnitTests
    {
        [Fact]
        public void GetHashCode_StandardizeNull_HandlesNull()
        {
            // StringComparer by itself throws for GetHashCode(null)
            Assert.ThrowsAny<Exception>(() => StringComparer.Ordinal.GetHashCode(null));

            // StandardizeNull protects it from null values.
            StringComparer.Ordinal.WithStandardNullHandling().GetHashCode(null);
        }

        [Fact]
        public void Equals_StandardizeNull_HandlesNull()
        {
            // This comparer will cause NREs.
            var dangerousComparer = ComparerBuilder.For<object>().OrderBy(x => x.GetHashCode(), specialNullHandling: true);
            Assert.ThrowsAny<NullReferenceException>(() => dangerousComparer.Equals(null, null));

            // StandardizeNull protects it from null values.
            Assert.True(dangerousComparer.WithStandardNullHandlingForEquality().Equals(null, null));
        }

        [Fact]
        public void Compare_StandardizeNull_HandlesNull()
        {
            // This comparer will cause NREs.
            var dangerousComparer = ComparerBuilder.For<object>().OrderBy(x => x.GetHashCode(), specialNullHandling: true);
            Assert.ThrowsAny<NullReferenceException>(() => dangerousComparer.Compare(null, null));

            // StandardizeNull protects it from null values.
            Assert.Equal(0, dangerousComparer.WithStandardNullHandling().Compare(null, null));
        }

        [Fact]
        public void GetHashCode_WithGetHashCode_InvokesSpecifiedImplementation()
        {
            // Comparer<T> types do not implement GetHashCode, so cannot be used with extension methods.
            var dangerousComparer = Comparer<int>.Create((x, y) => Comparer<int>.Default.Compare(x, y));
            Assert.ThrowsAny<InvalidOperationException>(() => dangerousComparer.Reverse());

            // WithGetHashCode* adds a GetHashCode implementation.
            var comparer = dangerousComparer.WithGetHashCode(_ => 7).Reverse();
            Assert.Equal(7, comparer.GetHashCode(13));
        }

        [Fact]
        public void GetHashCode_WithGetHashCodeThrow_Throws()
        {
            // Comparer<T> types do not implement GetHashCode, so cannot be used with extension methods.
            var dangerousComparer = Comparer<int>.Create((x, y) => Comparer<int>.Default.Compare(x, y));
            Assert.ThrowsAny<InvalidOperationException>(() => dangerousComparer.Reverse());

            // WithGetHashCode* adds a GetHashCode implementation.
            var comparer = dangerousComparer.WithGetHashCodeThrow().Reverse();
            Assert.ThrowsAny<NotImplementedException>(() => comparer.GetHashCode(13));
        }

        [Fact]
        public void GetHashCode_WithGetHashCodeConstant_ReturnsZero()
        {
            // Comparer<T> types do not implement GetHashCode, so cannot be used with extension methods.
            var dangerousComparer = Comparer<int>.Create((x, y) => Comparer<int>.Default.Compare(x, y));
            Assert.ThrowsAny<InvalidOperationException>(() => dangerousComparer.Reverse());

            // WithGetHashCode* adds a GetHashCode implementation.
            var comparer = dangerousComparer.WithGetHashCodeConstant().Reverse();
            Assert.Equal(0, comparer.GetHashCode(13));
        }

        [Fact]
        public void ToString_DumpsComparer()
        {
            Assert.StartsWith("StandardNullHandling", ComparerBuilder.For<int?>().Null().WithStandardNullHandling().ToString());
        }
    }
}
