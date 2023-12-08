using System;
using System.Diagnostics.CodeAnalysis;

namespace Nito.Comparers.Util
{
    /// <summary>
    /// Common implementations for comparers.
    /// </summary>
    /// <typeparam name="T">The type of objects being compared.</typeparam>
    internal abstract class ComparerBase<T> : EqualityComparerBase<T>, IFullComparer<T>
    {
        /// <summary>
        /// Compares two objects and returns a value less than 0 if <paramref name="x"/> is less than <paramref name="y"/>, 0 if <paramref name="x"/> is equal to <paramref name="y"/>, or greater than 0 if <paramref name="x"/> is greater than <paramref name="y"/>.
        /// </summary>
        /// <param name="x">The first object to compare. May be <c>null</c> if <see cref="EqualityComparerBase{T}.SpecialNullHandling"/> is <c>true</c>.</param>
        /// <param name="y">The second object to compare. May be <c>null</c> if <see cref="EqualityComparerBase{T}.SpecialNullHandling"/> is <c>true</c>.</param>
        /// <returns>A value less than 0 if <paramref name="x"/> is less than <paramref name="y"/>, 0 if <paramref name="x"/> is equal to <paramref name="y"/>, or greater than 0 if <paramref name="x"/> is greater than <paramref name="y"/>.</returns>
        protected abstract int DoCompare(T? x, T? y);

        /// <inheritdoc />
        protected override bool DoEquals(T? x, T? y) => Compare(x, y) == 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="ComparerBase{T}"/> class.
        /// </summary>
        /// <param name="specialNullHandling">A value indicating whether <c>null</c> values are passed to <see cref="EqualityComparerBase{T}.DoGetHashCode"/> and <see cref="DoCompare"/>. If <c>false</c>, then <c>null</c> values are considered less than any non-<c>null</c> values and are not passed to <see cref="EqualityComparerBase{T}.DoGetHashCode"/> nor <see cref="DoCompare"/>. This value is ignored if <typeparamref name="T"/> is a non-nullable type.</param>
        protected ComparerBase(bool specialNullHandling)
            : base(specialNullHandling)
        {
        }

        /// <inheritdoc />
        int System.Collections.IComparer.Compare(object? x, object? y)
        {
            var xValid = x is T || x == null;
            var yValid = y is T || y == null;
            if (!xValid || !yValid)
            {
                // System.Collections.Comparer.Compare forwards to the IComparable implementation of either argument, throwing if neither of them implement IComparable.
                // However, System.Collections.Generic.Comparer<T>.IComparer.Compare throws if either argument is not T.
                // To avoid the possibility of infinite recursion, we take the latter approach.
                throw new ArgumentException("Objects cannot be compared.");
            }

            if (!SpecialNullHandling)
            {
                if (x == null)
                {
                    if (y == null)
                        return 0;
                    return -1;
                }
                else if (y == null)
                {
                    return 1;
                }
            }

            return DoCompare((T)x!, (T)y!);
        }

        /// <inheritdoc />
        public int Compare(T? x, T? y)
        {
            if (!SpecialNullHandling)
            {
                if (x == null)
                {
                    if (y == null)
                        return 0;
                    return -1;
                }
                else if (y == null)
                {
                    return 1;
                }
            }

            return DoCompare(x!, y!);
        }
    }
}
