using System;
using System.Collections.Generic;
using System.Text;

#if !NET461 && !NETCOREAPP3_0 && !NETSTANDARD1_0 && !NETSTANDARD2_0 && !NETSTANDARD2_1
using static System.Numerics.BitOperations;
#endif

#pragma warning disable CA1815, CA1721

namespace Nito.Comparers.Internals
{
    /// <summary>
    /// A hash combiner that is implemented with the Murmur 3 algorithm. This is a mutable struct for performance reasons.
    /// </summary>
    public struct Murmur3Hash
    {
        private uint _len;
        private uint _hash;

        /// <summary>
        /// Gets the current result of the hash function.
        /// </summary>
        public int HashCode
        {
            get
            {
                unchecked
                {
                    var result = _hash ^ _len;
                    result ^= result >> 16;
                    result *= 0x85ebca6b;
                    result ^= result >> 13;
                    result *= 0xc2b2ae35;
                    result ^= result >> 16;
                    return (int)result;
                }
            }
        }

        /// <summary>
        /// Creates a new hash, starting at <paramref name="seed"/>.
        /// </summary>
        /// <param name="seed">The seed for the hash. Defaults to the FNV hash offset, for no particular reason.</param>
        public static Murmur3Hash Create(int seed = unchecked((int)2166136261))
        {
            var result = new Murmur3Hash();
            result._hash = unchecked((uint)seed);
            return result;
        }

        /// <summary>
        /// Adds the specified integer to this hash.
        /// </summary>
        /// <param name="data">The integer to hash.</param>
        public void Combine(int data)
        {
            unchecked
            {
                _len += 4;
                var k = (uint)data;
                k *= 0xcc9e2d51;
                k = RotateLeft(k, 15);
                k *= 0x1b873593;
                _hash ^= k;
                _hash = RotateLeft(_hash, 13);
                _hash *= 5;
                _hash += 0xe6546b64;
            }
        }

#if NET461 || NETCOREAPP3_0 || NETSTANDARD1_0 || NETSTANDARD2_0 || NETSTANDARD2_1
        private static uint RotateLeft(uint value, int bits) => (value << bits) | (value >> (32 - bits));
#endif
    }
}
