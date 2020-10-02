using Nito.Comparers.Util;

#pragma warning disable CS0660, CS0661, CA2225

namespace Nito.Comparers
{
    /// <summary>
    /// Provides implementations for comparison, equality, and hash code methods, along with overloaded operators. Note: overloaded operators should only be considered for immutable reference types. These implementations assume that there will only be one derived type that defines comparison/equality.
    /// </summary>
    /// <typeparam name="T">The type of objects being compared.</typeparam>
    public abstract class ComparableBaseWithOperators<T> : ComparableBase<T> where T : ComparableBaseWithOperators<T>
    {
        /// <summary>
        /// Returns <c>true</c> if two <typeparamref name="T"/> objects have the same value.
        /// </summary>
        /// <param name="left">A value of type <typeparamref name="T"/> or <c>null</c>.</param>
        /// <param name="right">A value of type <typeparamref name="T"/> or <c>null</c>.</param>
        /// <returns><c>true</c> if two <typeparamref name="T"/> objects have the same value; otherwise, <c>false</c>.</returns>
        public static bool operator ==(ComparableBaseWithOperators<T>? left, ComparableBaseWithOperators<T>? right) =>
            ComparableImplementations.ImplementOpEquality(DefaultComparer, (T)left!, (T)right!);

        /// <summary>
        /// Returns <c>true</c> if two <typeparamref name="T"/> objects have different values.
        /// </summary>
        /// <param name="left">A value of type <typeparamref name="T"/> or <c>null</c>.</param>
        /// <param name="right">A value of type <typeparamref name="T"/> or <c>null</c>.</param>
        /// <returns><c>true</c> if two <typeparamref name="T"/> objects have different values; otherwise, <c>false</c>.</returns>
        public static bool operator !=(ComparableBaseWithOperators<T>? left, ComparableBaseWithOperators<T>? right) =>
            ComparableImplementations.ImplementOpInequality(DefaultComparer, (T)left!, (T)right!);

        /// <summary>
        /// Returns <c>true</c> if <paramref name="left"/> has a value that is less than the value of <paramref name="right"/>.
        /// </summary>
        /// <param name="left">A value of type <typeparamref name="T"/> or <c>null</c>.</param>
        /// <param name="right">A value of type <typeparamref name="T"/> or <c>null</c>.</param>
        /// <returns><c>true</c> if <paramref name="left"/> has a value that is less than the value of <paramref name="right"/>; otherwise, <c>false</c>.</returns>
        public static bool operator <(ComparableBaseWithOperators<T>? left, ComparableBaseWithOperators<T>? right) =>
            ComparableImplementations.ImplementOpLessThan(DefaultComparer, (T)left!, (T)right!);

        /// <summary>
        /// Returns <c>true</c> if <paramref name="left"/> has a value that is greater than the value of <paramref name="right"/>.
        /// </summary>
        /// <param name="left">A value of type <typeparamref name="T"/> or <c>null</c>.</param>
        /// <param name="right">A value of type <typeparamref name="T"/> or <c>null</c>.</param>
        /// <returns><c>true</c> if <paramref name="left"/> has a value that is greater than the value of <paramref name="right"/>; otherwise, <c>false</c>.</returns>
        public static bool operator >(ComparableBaseWithOperators<T>? left, ComparableBaseWithOperators<T>? right) =>
            ComparableImplementations.ImplementOpGreaterThan(DefaultComparer, (T)left!, (T)right!);

        /// <summary>
        /// Returns <c>true</c> if <paramref name="left"/> has a value that is less than or equal to the value of <paramref name="right"/>.
        /// </summary>
        /// <param name="left">A value of type <typeparamref name="T"/> or <c>null</c>.</param>
        /// <param name="right">A value of type <typeparamref name="T"/> or <c>null</c>.</param>
        /// <returns><c>true</c> if <paramref name="left"/> has a value that is less than or equal to the value of <paramref name="right"/>; otherwise, <c>false</c>.</returns>
        public static bool operator <=(ComparableBaseWithOperators<T>? left, ComparableBaseWithOperators<T>? right) =>
            ComparableImplementations.ImplementOpLessThanOrEqual(DefaultComparer, (T)left!, (T)right!);

        /// <summary>
        /// Returns <c>true</c> if <paramref name="left"/> has a value that is greater than or equal to the value of <paramref name="right"/>.
        /// </summary>
        /// <param name="left">A value of type <typeparamref name="T"/> or <c>null</c>.</param>
        /// <param name="right">A value of type <typeparamref name="T"/> or <c>null</c>.</param>
        /// <returns><c>true</c> if <paramref name="left"/> has a value that is greater than or equal to the value of <paramref name="right"/>; otherwise, <c>false</c>.</returns>
        public static bool operator >=(ComparableBaseWithOperators<T>? left, ComparableBaseWithOperators<T>? right) =>
            ComparableImplementations.ImplementOpGreaterThanOrEqual(DefaultComparer, (T)left!, (T)right!);
    }
}
