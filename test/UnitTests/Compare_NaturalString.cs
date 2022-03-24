using System;
using System.Collections.Generic;
using System.Linq;
using Nito.Comparers;
using Xunit;
using System.Globalization;

#pragma warning disable CS0162

namespace UnitTests
{
    public class Compare_NaturalString
    {
        [Theory]
        [InlineData("a10", "a10", StringComparison.Ordinal)]
        [InlineData("a10", "A10", StringComparison.OrdinalIgnoreCase)]
        [InlineData("a010", "a10", StringComparison.Ordinal)]
        [InlineData("a000010", "a10", StringComparison.Ordinal)]
        [InlineData("a000b", "a0b", StringComparison.Ordinal)]
        [InlineData("0000", "0", StringComparison.Ordinal)]
        [InlineData("1.01", "1.1", StringComparison.Ordinal)] // Possibly surprising
        public void ExpectedEqual(string x, string y, StringComparison stringComparison)
        {
            var comparer = ComparerBuilder.For<string>().Natural(stringComparison);
            Assert.Equal(0, comparer.Compare(x, y));
        }

        [Theory]
        [InlineData("a09", "a10", StringComparison.Ordinal)]
        [InlineData("a9", "a10", StringComparison.Ordinal)]
        [InlineData("a9b", "a10b", StringComparison.Ordinal)]
        [InlineData("a10a", "a10b", StringComparison.Ordinal)]
        [InlineData("a", "aa", StringComparison.Ordinal)]
        [InlineData("1", "11", StringComparison.Ordinal)]
        [InlineData("", "a", StringComparison.Ordinal)]
        [InlineData("", "0", StringComparison.Ordinal)]
        [InlineData("", "3", StringComparison.Ordinal)]
        [InlineData("0", "a", StringComparison.Ordinal)]
        [InlineData("1.1", "1.10", StringComparison.Ordinal)] // Possibly surprising
        [InlineData("a a", "ab", StringComparison.Ordinal)] // Possibly surprising
        public void ExpectedLessThan(string x, string y, StringComparison stringComparison)
        {
            var comparer = ComparerBuilder.For<string>().Natural(stringComparison);
            Assert.True(comparer.Compare(x, y) < 0);
            Assert.True(comparer.Compare(y, x) > 0);
        }

        [Fact]
        public void ToString_DumpsComparer()
        {
            Assert.Equal("NaturalString", ComparerBuilder.For<string>().Natural(StringComparison.Ordinal).ToString());
        }
    }
}
