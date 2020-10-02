using Nito.Comparers.Util;

#pragma warning disable CS0660, CS0661, CA2225

namespace Nito.Comparers
{
    /// <summary>
    /// Provides implementations for equality and hash code methods, along with overloaded operators. Note: overloaded operators should only be considered for immutable reference types. These implementations assume that there will only be one derived type that defines equality.
    /// </summary>
    /// <typeparam name="T">The type of objects being compared.</typeparam>
    public abstract class EquatableBaseWithOperators<T> : EquatableBase<T> where T : EquatableBaseWithOperators<T>
    {
        /// <summary>
        /// Returns <c>true</c> if two <typeparamref name="T"/> objects have the same value.
        /// </summary>
        /// <param name="left">A value of type <typeparamref name="T"/> or <c>null</c>.</param>
        /// <param name="right">A value of type <typeparamref name="T"/> or <c>null</c>.</param>
        /// <returns><c>true</c> if two <typeparamref name="T"/> objects have the same value; otherwise, <c>false</c>.</returns>
        public static bool operator ==(EquatableBaseWithOperators<T>? left, EquatableBaseWithOperators<T>? right) =>
            ComparableImplementations.ImplementOpEquality(DefaultComparer, (T)left!, (T)right!);

        /// <summary>
        /// Returns <c>true</c> if two <typeparamref name="T"/> objects have different values.
        /// </summary>
        /// <param name="left">A value of type <typeparamref name="T"/> or <c>null</c>.</param>
        /// <param name="right">A value of type <typeparamref name="T"/> or <c>null</c>.</param>
        /// <returns><c>true</c> if two <typeparamref name="T"/> objects have different values; otherwise, <c>false</c>.</returns>
        public static bool operator !=(EquatableBaseWithOperators<T>? left, EquatableBaseWithOperators<T>? right) =>
            ComparableImplementations.ImplementOpInequality(DefaultComparer, (T)left!, (T)right!);
    }
}
