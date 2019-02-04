using System;

namespace Nito.Comparers.Util
{
    /// <summary>
    /// Common implementations for equality comparers.
    /// </summary>
    /// <typeparam name="T">The type of objects being compared.</typeparam>
    internal abstract class EqualityComparerBase<T> : IFullEqualityComparer<T>
    {
        /// <summary>
        /// A value indicating whether <c>null</c> values will be passed down to derived implementations.
        /// </summary>
        private readonly bool _specialNullHandling;

        /// <summary>
        /// Gets a value indicating whether <c>null</c> values will be passed down to derived implementations.
        /// </summary>
        protected bool SpecialNullHandling
        {
            get { return _specialNullHandling; }
        }

        /// <summary>
        /// Returns a hash code for the specified object.
        /// </summary>
        /// <param name="obj">The object for which to return a hash code.</param>
        /// <returns>A hash code for the specified object.</returns>
        protected abstract int DoGetHashCode(T obj);

        /// <summary>
        /// Compares two objects and returns <c>true</c> if they are equal and <c>false</c> if they are not equal.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns><c>true</c> if <paramref name="x"/> is equal to <paramref name="y"/>; otherwise, <c>false</c>.</returns>
        protected abstract bool DoEquals(T x, T y);

        /// <summary>
        /// Initializes a new instance of the <see cref="EqualityComparerBase{T}"/> class.
        /// </summary>
        /// <param name="specialNullHandling">A value indicating whether <c>null</c> values are passed to <see cref="DoGetHashCode"/> and <see cref="DoEquals"/>. If <c>false</c>, then <c>null</c> values are considered less than any non-<c>null</c> values and are not passed to <see cref="DoGetHashCode"/> nor <see cref="DoEquals"/>. This value is ignored if <typeparamref name="T"/> is a non-nullable type.</param>
        protected EqualityComparerBase(bool specialNullHandling)
        {
            _specialNullHandling = specialNullHandling;
            if (default(T) != null)
                _specialNullHandling = false;
        }

        /// <summary>
        /// Compares two objects and returns a value indicating whether they are equal.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns><c>true</c> if <paramref name="x"/> is equal to <paramref name="y"/>; otherwise <c>false</c>.</returns>
        bool System.Collections.IEqualityComparer.Equals(object x, object y)
        {
            if (!SpecialNullHandling)
            {
                if (x == null || y == null)
                    return (x == null && y == null);
            }

            // EqualityComparer<T>.IEqualityComparer.Equals will throw in this situation, but int.Equals returns false.
            var xValid = x is T || x == null;
            var yValid = y is T || y == null;
            if (!xValid || !yValid)
                return false;

            return DoEquals((T)x, (T)y);
        }

        /// <summary>
        /// Returns a hash code for the specified object.
        /// </summary>
        /// <param name="obj">The object for which to return a hash code.</param>
        /// <returns>A hash code for the specified object.</returns>
        int System.Collections.IEqualityComparer.GetHashCode(object obj)
        {
            if (!SpecialNullHandling)
            {
                if (obj == null)
                    return 0;
            }

            var objValid = obj is T || obj == null;
            if (!objValid)
                throw new ArgumentException("Invalid type for comparison.");

            return DoGetHashCode((T)obj);
        }

        /// <summary>
        /// Compares two objects and returns a value indicating whether they are equal.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns><c>true</c> if <paramref name="x"/> is equal to <paramref name="y"/>; otherwise <c>false</c>.</returns>
        public bool Equals(T x, T y)
        {
            if (!SpecialNullHandling)
            {
                if (x == null || y == null)
                    return (x == null && y == null);
            }

            return DoEquals(x, y);
        }

        /// <summary>
        /// Returns a hash code for the specified object.
        /// </summary>
        /// <param name="obj">The object for which to return a hash code.</param>
        /// <returns>A hash code for the specified object.</returns>
        public int GetHashCode(T obj)
        {
            if (!SpecialNullHandling)
            {
                if (obj == null)
                    return 0;
            }

            return DoGetHashCode(obj);
        }
    }
}
