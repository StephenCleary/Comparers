using System;
using Nito.Comparers.Util;

namespace Nito.Comparers
{
    /// <summary>
    /// An object that implements a comparer using delegates.
    /// </summary>
    /// <typeparam name="T">The type of objects being compared.</typeparam>
    public sealed class AnonymousComparer<T> : ComparerBase<T>
    {
        /// <summary>
        /// Gets or sets a delegate which compares two objects and returns a value less than 0 if its first argument is less than its second argument, 0 if its two arguments are equal, or greater than 0 if its first argument is greater than its second argument.
        /// </summary>
        public new Func<T, T, int> Compare { get; set; }

        /// <summary>
        /// Gets or sets a delegate which calculates a hash code for an object.
        /// </summary>
        public new Func<T, int> GetHashCode { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AnonymousComparer{T}"/> class.
        /// </summary>
        /// <param name="specialNullHandling">A value indicating whether <c>null</c> values are passed to <see cref="Compare"/> and <see cref="GetHashCode"/>. If <c>false</c>, then <c>null</c> values are considered less than any non-<c>null</c> values and are not passed to <see cref="Compare"/> nor <see cref="GetHashCode"/>.</param>
        public AnonymousComparer(bool specialNullHandling = false)
            : base(specialNullHandling)
        {
        }

        /// <summary>
        /// Returns a hash code for the specified object.
        /// </summary>
        /// <param name="obj">The object for which to return a hash code.</param>
        /// <returns>A hash code for the specified object.</returns>
        protected override int DoGetHashCode(T obj)
        {
            return GetHashCode(obj);
        }

        /// <summary>
        /// Compares two objects and returns a value less than 0 if <paramref name="x"/> is less than <paramref name="y"/>, 0 if <paramref name="x"/> is equal to <paramref name="y"/>, or greater than 0 if <paramref name="x"/> is greater than <paramref name="y"/>.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>A value less than 0 if <paramref name="x"/> is less than <paramref name="y"/>, 0 if <paramref name="x"/> is equal to <paramref name="y"/>, or greater than 0 if <paramref name="x"/> is greater than <paramref name="y"/>.</returns>
        protected override int DoCompare(T x, T y)
        {
            return Compare(x, y);
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
