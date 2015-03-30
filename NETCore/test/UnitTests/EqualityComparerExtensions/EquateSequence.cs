using System;
using System.Collections.Generic;
using System.Linq;
using Nito.Comparers;
using Xunit;
using System.Globalization;

namespace EqualityComparerExtensions_
{
    public class _EquateSequence
    {
        [Fact]
        public void SubstitutesCompareDefaultForComparerDefault()
        {
            var comparer = EqualityComparer<int>.Default.EquateSequence();
            Assert.Equal(EqualityComparerBuilder.For<int>().Default().EquateSequence().ToString(), comparer.ToString());
        }

        [Fact]
        public void SubstitutesCompareDefaultForNull()
        {
            IEqualityComparer<int> source = null;
            var comparer = source.EquateSequence();
            Assert.Equal(EqualityComparerBuilder.For<int>().Default().EquateSequence().ToString(), comparer.ToString());
        }

        [Fact]
        public void ShorterSequenceIsNotEqualToLongerSequenceIfElementsAreEqual()
        {
            Assert.False(EqualityComparerBuilder.For<int>().Default().EquateSequence().Equals(new[] { 3, 4 }, new[] { 3, 4, 5 }));
            Assert.False(EqualityComparerBuilder.For<int>().Default().EquateSequence().Equals(new[] { 3, 4, 5 }, new[] { 3, 4 }));
        }

        [Fact]
        public void SequencesAreEqualIfElementsAreEqual()
        {
            Assert.True(EqualityComparerBuilder.For<int>().Default().EquateSequence().Equals(new[] { 3, 4 }, new[] { 3, 4 }));
            Assert.True(EqualityComparerBuilder.For<int>().Default().EquateSequence().Equals(new[] { 3, 4, 5 }, new[] { 3, 4, 5 }));
            Assert.Equal(EqualityComparerBuilder.For<int>().Default().EquateSequence().GetHashCode(new[] { 3, 4 }), EqualityComparerBuilder.For<int>().Default().EquateSequence().GetHashCode(new[] { 3, 4 }));
        }

        [Fact]
        public void EqualLengthSequencesWithUnequalElementsAreNotEqual()
        {
            Assert.False(EqualityComparerBuilder.For<int>().Default().EquateSequence().Equals(new[] { 3, 4 }, new[] { 3, 5 }));
            Assert.False(EqualityComparerBuilder.For<int>().Default().EquateSequence().Equals(new[] { 3, 4, 5 }, new[] { 3, 3, 5 }));
        }

        [Fact]
        public void SequenceUsesSourceComparerForElementComparisons()
        {
#if ASPNETCORE50
            StringComparer invariantCultureComparerIgnoreCase = CultureInfo.InvariantCulture.CompareInfo.GetStringComparer(CompareOptions.IgnoreCase);
#else
            StringComparer invariantCultureComparerIgnoreCase = StringComparer.InvariantCultureIgnoreCase;
#endif
            var comparer = invariantCultureComparerIgnoreCase.EquateSequence();
            Assert.True(comparer.Equals(new[] { "a" }, new[] { "A" }));
            Assert.Equal(comparer.GetHashCode(new[] { "a" }), comparer.GetHashCode(new[] { "A" }));
        }

        [Fact]
        public void NullIsNotEqualToEmpty()
        {
            Assert.False(EqualityComparerBuilder.For<int>().Default().EquateSequence().Equals(null, Enumerable.Empty<int>()));
            Assert.False(EqualityComparerBuilder.For<int>().Default().EquateSequence().Equals(Enumerable.Empty<int>(), null));
        }
    }
}
