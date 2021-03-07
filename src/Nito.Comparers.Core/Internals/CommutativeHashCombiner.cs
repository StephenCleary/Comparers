using System;
using System.Collections.Generic;
using System.Text;

#pragma warning disable CA1815, CA1721

namespace Nito.Comparers.Internals
{
    /// <summary>
    /// A hash combiner that is implemented with a simple commutative algorithm. This is a mutable struct for performance reasons.
    /// </summary>
    public struct CommutativeHashCombiner
    {
        private uint _hash;

        /// <summary>
        /// Gets the current result of the hash function.
        /// </summary>
        public int HashCode => unchecked((int) _hash);

        /// <summary>
        /// Creates a new hash, starting at <paramref name="seed"/>.
        /// </summary>
        /// <param name="seed">The seed for the hash. Defaults to the FNV hash offset, for no particular reason.</param>
        public static CommutativeHashCombiner Create(int seed = unchecked((int)2166136261)) => new() { _hash = unchecked((uint) seed) };

        /// <summary>
        /// Adds the specified integer to this hash. This operation is commutative.
        /// </summary>
        /// <param name="data">The integer to hash.</param>
        public void Combine(int data)
        {
            unchecked
            {
                // Simple addition is pretty much the best we can do since this operation must be commutative.
                // We also add a constant value to act as a kind of "length counter" in the higher 16 bits.
                // The hash combination is free to overflow into the "length counter".
                // The higher 16 bits were chosen because that gives a decent distinction for sequences of <64k items while also distinguishing between small integer values (commonly used as ids).
                _hash += (uint)data + 65536;
            }
        }
    }
}
