using System;
using System.Collections.Generic;

namespace Nito.Comparers.Util
{
    /// <summary>
    /// A comparer that works by comparing the results of the specified key selector.
    /// </summary>
    /// <typeparam name="TSource">The type of key objects being compared.</typeparam>
    /// <typeparam name="T">The type of objects being compared.</typeparam>
    internal sealed class SelectComparer<T, TSource> : SourceComparerBase<T, TSource>
    {
        /// <summary>
        /// The key selector.
        /// </summary>
        private readonly Func<T, TSource> _selector;

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectComparer{T, TSource}"/> class.
        /// </summary>
        /// <param name="selector">The key selector. May not be <c>null</c>.</param>
        /// <param name="source">The source comparer. If this is <c>null</c>, the default comparer is used.</param>
        /// <param name="specialNullHandling">A value indicating whether <c>null</c> values are passed to <paramref name="selector"/>. If <c>false</c>, then <c>null</c> values are considered less than any non-<c>null</c> values and are not passed to <paramref name="selector"/>.</param>
        public SelectComparer(Func<T, TSource> selector, IComparer<TSource> source, bool specialNullHandling)
            : base(source, specialNullHandling)
        {
            _selector = selector;
        }

        /// <summary>
        /// Returns a hash code for the specified object.
        /// </summary>
        /// <param name="obj">The object for which to return a hash code.</param>
        /// <returns>A hash code for the specified object.</returns>
        protected override int DoGetHashCode(T obj)
        {
            return _sourceEqualityComparer.GetHashCode(_selector(obj));
        }

        /// <summary>
        /// Compares two objects and returns a value less than 0 if <paramref name="x"/> is less than <paramref name="y"/>, 0 if <paramref name="x"/> is equal to <paramref name="y"/>, or greater than 0 if <paramref name="x"/> is greater than <paramref name="y"/>.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>A value less than 0 if <paramref name="x"/> is less than <paramref name="y"/>, 0 if <paramref name="x"/> is equal to <paramref name="y"/>, or greater than 0 if <paramref name="x"/> is greater than <paramref name="y"/>.</returns>
        protected override int DoCompare(T x, T y)
        {
            return _source.Compare(_selector(x), _selector(y));
        }

        /// <summary>
        /// Compares two objects and returns <c>true</c> if they are equal and <c>false</c> if they are not equal.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns><c>true</c> if <paramref name="x"/> is equal to <paramref name="y"/>; otherwise, <c>false</c>.</returns>
        protected override bool DoEquals(T x, T y)
        {
            return _sourceEqualityComparer.Equals(_selector(x), _selector(y));
        }

        /// <summary>
        /// Returns a short, human-readable description of the comparer. This is intended for debugging and not for other purposes.
        /// </summary>
        public override string ToString()
        {
            return "Select<" + typeof(TSource).Name + ">(" + _source + ")";
        }
    }
}
