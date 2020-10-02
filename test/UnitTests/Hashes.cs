using Nito.Comparers;
using Nito.Comparers.Internals;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace UnitTests
{
    public class Hashes
    {
        [Fact]
        public void Hash_MatchesWellKnownValue()
        {
            var input = 0x64636261; // "abcd" in UTF-8
            var hash = Murmur3Hash.Create(0);
            hash.Combine(input);
            Assert.Equal(1139631978, hash.HashCode);
        }

        [Fact]
        public void NullComparerHash_EqualsDefaultMurmer3Hash()
        {
            var comparer = ComparerBuilder.For<int>().Null();
            var objectHash = comparer.GetHashCode(0);
            Assert.Equal(Murmur3Hash.Create().HashCode, objectHash);
        }
    }
}
