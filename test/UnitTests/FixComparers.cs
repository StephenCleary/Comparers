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
            var comparer = StringComparer.Ordinal.WithStandardNullHandling();
            comparer.GetHashCode(null);

            // While passing through non-null values.
            Assert.Equal(StringComparer.Ordinal.GetHashCode("test"), comparer.GetHashCode("test"));
        }

        [Fact]
        public void Equals_StandardizeNull_HandlesNull()
        {
            // This comparer will cause NREs.
            var dangerousComparer = ComparerBuilder.For<object>().OrderBy(x => x.GetHashCode(), specialNullHandling: true);
            Assert.ThrowsAny<NullReferenceException>(() => dangerousComparer.Equals(null, null));

            // StandardizeNull protects it from null values.
            var comparer = dangerousComparer.WithStandardNullHandling();
            Assert.True(comparer.Equals(null, null));

            // While passing through non-null values.
            Assert.Equal(dangerousComparer.Equals("test", "test"), comparer.Equals("test", "test"));
        }

        [Fact]
        public void Compare_StandardizeNull_HandlesNull()
        {
            // This comparer will cause NREs.
            var dangerousComparer = ComparerBuilder.For<object>().OrderBy(x => x.GetHashCode(), specialNullHandling: true);
            Assert.ThrowsAny<NullReferenceException>(() => dangerousComparer.Compare(null, null));

            // StandardizeNull protects it from null values.
            var comparer = dangerousComparer.WithStandardNullHandling();
            Assert.Equal(0, comparer.Compare(null, null));

            // While passing through non-null values.
            Assert.Equal(dangerousComparer.Compare("test1", "test2"), comparer.Compare("test1", "test2"));
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

            // While passing through Compare calls.
            Assert.Equal(dangerousComparer.Compare(7, 13), comparer.Compare(13, 7));
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

            // While passing through Compare calls.
            Assert.Equal(dangerousComparer.Compare(7, 13), comparer.Compare(13, 7));
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

            // While passing through Compare calls.
            Assert.Equal(dangerousComparer.Compare(7, 13), comparer.Compare(13, 7));
        }

        [Fact]
        public void ToString_StandardNullHandling_DumpsComparer()
        {
            Assert.StartsWith("StandardNullHandling", ComparerBuilder.For<int?>().Null().WithStandardNullHandling().ToString());
        }

        [Fact]
        public void ToString_WithGetHashCode_DumpsComparer()
        {
            Assert.StartsWith("ExplicitGetHashCode", ComparerBuilder.For<int?>().Null().WithGetHashCodeConstant().ToString());
        }
    }
}
