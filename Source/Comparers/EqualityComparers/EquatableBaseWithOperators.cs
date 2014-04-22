using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using Comparers.Util;

namespace EqualityComparers
{
    /// <summary>
    /// Provides implementations for equality and hash code methods, along with overloaded operators. Note: overloaded operators should only be considered for immutable reference types. These implementations assume that there will only be one derived type that defines equality.
    /// </summary>
    /// <typeparam name="T">The type of objects being compared.</typeparam>
    public abstract class EquatableBaseWithOperators<T> : EquatableBase<T> where T : EquatableBaseWithOperators<T>
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
        public static bool operator ==(EquatableBaseWithOperators<T> left, EquatableBaseWithOperators<T> right)
        {
            Contract.Assume(DefaultComparer != null);
            return ComparableImplementations.ImplementOpEquality(DefaultComparer, (T)left, (T)right);
        }

        /// <summary>
        /// Returns <c>true</c> if two <typeparamref name="T"/> objects have different values.
        /// </summary>
        /// <param name="left">A value of type <typeparamref name="T"/> or <c>null</c>.</param>
        /// <param name="right">A value of type <typeparamref name="T"/> or <c>null</c>.</param>
        /// <returns><c>true</c> if two <typeparamref name="T"/> objects have different values; otherwise, <c>false</c>.</returns>
        public static bool operator !=(EquatableBaseWithOperators<T> left, EquatableBaseWithOperators<T> right)
        {
            Contract.Assume(DefaultComparer != null);
            return ComparableImplementations.ImplementOpInequality(DefaultComparer, (T)left, (T)right);
        }
    }
}
