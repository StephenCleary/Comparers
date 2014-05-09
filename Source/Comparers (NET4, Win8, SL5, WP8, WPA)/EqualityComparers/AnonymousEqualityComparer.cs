using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using EqualityComparers.Util;

namespace EqualityComparers
{
    /// <summary>
    /// An object that implements an equality comparer using delegates.
    /// </summary>
    /// <typeparam name="T">The type of objects being compared.</typeparam>
    public sealed class AnonymousEqualityComparer<T> : EqualityComparerBase<T>
    {
        /// <summary>
        /// Gets or sets a delegate which compares two objects and returns <c>true</c> if they are equal and <c>false</c> if they are not equal.
        /// </summary>
        public new Func<T, T, bool> Equals { get; set; }

        /// <summary>
        /// Gets or sets a delegate which calculates a hash code for an object.
        /// </summary>
        public new Func<T, int> GetHashCode { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AnonymousEqualityComparer{T}"/> class.
        /// </summary>
        /// <param name="allowNulls">A value indicating whether <c>null</c> values are passed to <see cref="Equals"/> and <see cref="GetHashCode"/>. If <c>false</c>, then <c>null</c> values are considered less than any non-<c>null</c> values and are not passed to <see cref="Equals"/> nor <see cref="GetHashCode"/>.</param>
        public AnonymousEqualityComparer(bool allowNulls = false)
            : base(allowNulls)
        {
        }

        /// <summary>
        /// Returns a hash code for the specified object.
        /// </summary>
        /// <param name="obj">The object for which to return a hash code.</param>
        /// <returns>A hash code for the specified object.</returns>
        protected override int DoGetHashCode(T obj)
        {
            Contract.Assume(this.GetHashCode != null);
            return this.GetHashCode(obj);
        }

        /// <summary>
        /// Compares two objects and returns <c>true</c> if they are equal and <c>false</c> if they are not equal.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns><c>true</c> if <paramref name="x"/> is equal to <paramref name="y"/>; otherwise, <c>false</c>.</returns>
        protected override bool DoEquals(T x, T y)
        {
            Contract.Assume(this.Equals != null);
            return this.Equals(x, y);
        }

        /// <summary>
        /// Returns a short, human-readable description of the comparer. This is intended for debugging and not for other purposes.
        /// </summary>
        public override string ToString()
        {
            return "Anonymous";
        }
    }
}
