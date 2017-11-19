﻿using System.Collections.Generic;

namespace Nito.Comparers.Util
{
    /// <summary>
    /// A comparer that reverses the evaluation of the specified source comparer.
    /// </summary>
    /// <typeparam name="T">The type of objects being compared.</typeparam>
    internal sealed class ReverseComparer<T> : ComparerBase<T>
    {
        /// <summary>
        /// The source comparer.
        /// </summary>
        private readonly IComparer<T> _source;

        /// <summary>
        /// The source equality comparer.
        /// </summary>
        private readonly IEqualityComparer<T> _sourceEqualityComparer;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReverseComparer{T}"/> class.
        /// </summary>
        /// <param name="source">The source comparer. If this is <c>null</c>, the default comparer is used.</param>
        public ReverseComparer(IComparer<T> source)
            : base(true)
        {
            _source = ComparerHelpers.NormalizeDefault(source);
            _sourceEqualityComparer = ComparerHelpers.GetEqualityComparerFromComparer(_source);
        }

        /// <summary>
        /// Returns a hash code for the specified object.
        /// </summary>
        /// <param name="obj">The object for which to return a hash code. This object may be <c>null</c>.</param>
        /// <returns>A hash code for the specified object.</returns>
        protected override int DoGetHashCode(T obj)
        {
            return _sourceEqualityComparer.GetHashCode(obj);
        }

        /// <summary>
        /// Compares two objects and returns a value less than 0 if <paramref name="x"/> is less than <paramref name="y"/>, 0 if <paramref name="x"/> is equal to <paramref name="y"/>, or greater than 0 if <paramref name="x"/> is greater than <paramref name="y"/>.
        /// </summary>
        /// <param name="x">The first object to compare. This object may be <c>null</c>.</param>
        /// <param name="y">The second object to compare. This object may be <c>null</c>.</param>
        /// <returns>A value less than 0 if <paramref name="x"/> is less than <paramref name="y"/>, 0 if <paramref name="x"/> is equal to <paramref name="y"/>, or greater than 0 if <paramref name="x"/> is greater than <paramref name="y"/>.</returns>
        protected override int DoCompare(T x, T y)
        {
            return _source.Compare(y, x);
        }

        /// <summary>
        /// Compares two objects and returns <c>true</c> if they are equal and <c>false</c> if they are not equal.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns><c>true</c> if <paramref name="x"/> is equal to <paramref name="y"/>; otherwise, <c>false</c>.</returns>
        protected override bool DoEquals(T x, T y)
        {
            return _sourceEqualityComparer.Equals(x, y);
        }

        /// <summary>
        /// Returns a short, human-readable description of the comparer. This is intended for debugging and not for other purposes.
        /// </summary>
        public override string ToString()
        {
            return "Reverse(" + _source + ")";
        }
    }
}
