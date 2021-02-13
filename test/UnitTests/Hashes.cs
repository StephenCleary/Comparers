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

        [Fact]
        public void CommutativeHashCombiner_IsCommutative()
        {
            // This unit test assumes the GetHashCode of these two objects are different.
            int value1 = 5;
            int value2 = 7;
            Assert.NotEqual(value1.GetHashCode(), value2.GetHashCode());

            var hash1 = CommutativeHashCombiner.Create();
            hash1.Combine(value1);
            hash1.Combine(value2);
            var result1 = hash1.HashCode;

            var hash2 = CommutativeHashCombiner.Create();
            hash2.Combine(value2);
            hash2.Combine(value1);
            var result2 = hash2.HashCode;

            Assert.Equal(result1, result2);
        }
    }
}
