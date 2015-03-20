using System;
using System.Collections.Generic;
using System.Text;
using Nito.Comparers.Util;

namespace Nito.Comparers
{
    /// <summary>
    /// Provides implementations for comparison, equality, and hash code methods, along with overloaded operators. Note: overloaded operators should only be considered for immutable reference types. These implementations assume that there will only be one derived type that defines comparison/equality.
    /// </summary>
    /// <typeparam name="T">The type of objects being compared.</typeparam>
    public abstract class ComparableBaseWithOperators<T> : ComparableBase<T> where T : ComparableBaseWithOperators<T>
    {
        /// <summary>
        /// Returns a value indicating whether this instance is equal to the specified object.
        /// </summary>
        /// <param name="obj">The object to compare with this instance.</param>
        /// <returns>A value indicating whether this instance is equal to the specified object.</returns>
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        /// <summary>
        /// Gets the hash code for this instance.
        /// </summary>
        /// <returns>The hash code for this instance.</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Returns <c>true</c> if two <typeparamref name="T"/> objects have the same value.
        /// </summary>
        /// <param name="left">A value of type <typeparamref name="T"/> or <c>null</c>.</param>
        /// <param name="right">A value of type <typeparamref name="T"/> or <c>null</c>.</param>
        /// <returns><c>true</c> if two <typeparamref name="T"/> objects have the same value; otherwise, <c>false</c>.</returns>
        public static bool operator ==(ComparableBaseWithOperators<T> left, ComparableBaseWithOperators<T> right)
        {
            return ComparableImplementations.ImplementOpEquality(DefaultComparer, (T)left, (T)right);
        }

        /// <summary>
        /// Returns <c>true</c> if two <typeparamref name="T"/> objects have different values.
        /// </summary>
        /// <param name="left">A value of type <typeparamref name="T"/> or <c>null</c>.</param>
        /// <param name="right">A value of type <typeparamref name="T"/> or <c>null</c>.</param>
        /// <returns><c>true</c> if two <typeparamref name="T"/> objects have different values; otherwise, <c>false</c>.</returns>
        public static bool operator !=(ComparableBaseWithOperators<T> left, ComparableBaseWithOperators<T> right)
        {
            return ComparableImplementations.ImplementOpInequality(DefaultComparer, (T)left, (T)right);
        }

        /// <summary>
        /// Returns <c>true</c> if <paramref name="left"/> has a value that is less than the value of <paramref name="right"/>.
        /// </summary>
        /// <param name="left">A value of type <typeparamref name="T"/> or <c>null</c>.</param>
        /// <param name="right">A value of type <typeparamref name="T"/> or <c>null</c>.</param>
        /// <returns><c>true</c> if <paramref name="left"/> has a value that is less than the value of <paramref name="right"/>; otherwise, <c>false</c>.</returns>
        public static bool operator <(ComparableBaseWithOperators<T> left, ComparableBaseWithOperators<T> right)
        {
            return ComparableImplementations.ImplementOpLessThan(DefaultComparer, (T)left, (T)right);
        }

        /// <summary>
        /// Returns <c>true</c> if <paramref name="left"/> has a value that is greater than the value of <paramref name="right"/>.
        /// </summary>
        /// <param name="left">A value of type <typeparamref name="T"/> or <c>null</c>.</param>
        /// <param name="right">A value of type <typeparamref name="T"/> or <c>null</c>.</param>
        /// <returns><c>true</c> if <paramref name="left"/> has a value that is greater than the value of <paramref name="right"/>; otherwise, <c>false</c>.</returns>
        public static bool operator >(ComparableBaseWithOperators<T> left, ComparableBaseWithOperators<T> right)
        {
            return ComparableImplementations.ImplementOpGreaterThan(DefaultComparer, (T)left, (T)right);
        }

        /// <summary>
        /// Returns <c>true</c> if <paramref name="left"/> has a value that is less than or equal to the value of <paramref name="right"/>.
        /// </summary>
        /// <param name="left">A value of type <typeparamref name="T"/> or <c>null</c>.</param>
        /// <param name="right">A value of type <typeparamref name="T"/> or <c>null</c>.</param>
        /// <returns><c>true</c> if <paramref name="left"/> has a value that is less than or equal to the value of <paramref name="right"/>; otherwise, <c>false</c>.</returns>
        public static bool operator <=(ComparableBaseWithOperators<T> left, ComparableBaseWithOperators<T> right)
        {
            return ComparableImplementations.ImplementOpLessThanOrEqual(DefaultComparer, (T)left, (T)right);
        }

        /// <summary>
        /// Returns <c>true</c> if <paramref name="left"/> has a value that is greater than or equal to the value of <paramref name="right"/>.
        /// </summary>
        /// <param name="left">A value of type <typeparamref name="T"/> or <c>null</c>.</param>
        /// <param name="right">A value of type <typeparamref name="T"/> or <c>null</c>.</param>
        /// <returns><c>true</c> if <paramref name="left"/> has a value that is greater than or equal to the value of <paramref name="right"/>; otherwise, <c>false</c>.</returns>
        public static bool operator >=(ComparableBaseWithOperators<T> left, ComparableBaseWithOperators<T> right)
        {
            return ComparableImplementations.ImplementOpGreaterThanOrEqual(DefaultComparer, (T)left, (T)right);
        }
    }
}
