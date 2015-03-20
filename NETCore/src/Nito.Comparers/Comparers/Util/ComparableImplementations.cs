using System;
using System.Collections.Generic;
using System.Text;

namespace Nito.Comparers.Util
{
    /// <summary>
    /// Provides implementations for comparison, equality, and hash code methods.
    /// </summary>
    public static class ComparableImplementations
    {
        /// <summary>
        /// Implements <see cref="IComparable{T}.CompareTo"/>. Types implementing <see cref="IComparable{T}"/> should also implement <see cref="IComparable"/> and <see cref="IEquatable{T}"/>, and override <see cref="Object.Equals(Object)"/> and <see cref="Object.GetHashCode"/>.
        /// </summary>
        /// <typeparam name="T">The type of objects being compared.</typeparam>
        /// <param name="comparer">The comparer.</param>
        /// <param name="this">The object doing the implementing.</param>
        /// <param name="other">The other object.</param>
        public static int ImplementCompareTo<T>(IComparer<T> comparer, T @this, T other) where T : IComparable<T>
        {
            return comparer.Compare(@this, other);
        }

        /// <summary>
        /// Implements <see cref="IComparable.CompareTo"/>. Types implementing <see cref="IComparable"/> should also override <see cref="Object.Equals(Object)"/> and <see cref="Object.GetHashCode"/>.
        /// </summary>
        /// <param name="comparer">The comparer.</param>
        /// <param name="this">The object doing the implementing.</param>
        /// <param name="obj">The other object.</param>
        public static int ImplementCompareTo(System.Collections.IComparer comparer, IComparable @this, object obj)
        {
            return comparer.Compare(@this, obj);
        }

        /// <summary>
        /// Implements <see cref="Object.GetHashCode"/>. Types overriding <see cref="Object.GetHashCode"/> should also override <see cref="Object.Equals(Object)"/>.
        /// </summary>
        /// <typeparam name="T">The type of objects being compared.</typeparam>
        /// <param name="equalityComparer">The comparer.</param>
        /// <param name="this">The object doing the implementing.</param>
        public static int ImplementGetHashCode<T>(IEqualityComparer<T> equalityComparer, T @this)
        {
            return equalityComparer.GetHashCode(@this);
        }

        /// <summary>
        /// Implements <see cref="IEquatable{T}.Equals"/>. Types implementing <see cref="IEquatable{T}"/> should also override <see cref="Object.Equals(Object)"/> and <see cref="Object.GetHashCode"/>.
        /// </summary>
        /// <typeparam name="T">The type of objects being compared.</typeparam>
        /// <param name="equalityComparer">The comparer.</param>
        /// <param name="this">The object doing the implementing.</param>
        /// <param name="other">The other object.</param>
        public static bool ImplementEquals<T>(IEqualityComparer<T> equalityComparer, T @this, T other) where T : IEquatable<T>
        {
            return equalityComparer.Equals(@this, other);
        }

        /// <summary>
        /// Implements <see cref="Object.Equals(Object)"/>. Types overriding <see cref="Object.Equals(Object)"/> should also override <see cref="Object.GetHashCode"/>.
        /// </summary>
        /// <param name="equalityComparer">The comparer.</param>
        /// <param name="this">The object doing the implementing.</param>
        /// <param name="obj">The other object.</param>
        public static bool ImplementEquals(System.Collections.IEqualityComparer equalityComparer, object @this, object obj)
        {
            return equalityComparer.Equals(@this, obj);
        }

        /// <summary>
        /// Implements <see cref="Object.Equals(Object)"/>. Types overriding <see cref="Object.Equals(Object)"/> should also override <see cref="Object.GetHashCode"/>.
        /// </summary>
        /// <param name="equalityComparer">The comparer.</param>
        /// <param name="this">The object doing the implementing.</param>
        /// <param name="obj">The other object.</param>
        public static bool ImplementEquals<T>(IEqualityComparer<T> equalityComparer, T @this, object obj)
        {
            return equalityComparer.Equals(@this, (T)obj);
        }

        /// <summary>
        /// Implements <c>op_Eqality</c>. Types overloading <c>op_Equality</c> should also overload <c>op_Inequality</c> and override <see cref="Object.Equals(Object)"/> and <see cref="Object.GetHashCode"/>.
        /// </summary>
        /// <typeparam name="T">The type of objects being compared.</typeparam>
        /// <param name="equalityComparer">The comparer.</param>
        /// <param name="left">A value of type <typeparamref name="T"/> or <c>null</c>.</param>
        /// <param name="right">A value of type <typeparamref name="T"/> or <c>null</c>.</param>
        public static bool ImplementOpEquality<T>(IEqualityComparer<T> equalityComparer, T left, T right)
        {
            return equalityComparer.Equals(left, right);
        }

        /// <summary>
        /// Implements <c>op_Ineqality</c>. Types overloading <c>op_Inequality</c> should also overload <c>op_Equality</c> and override <see cref="Object.Equals(Object)"/> and <see cref="Object.GetHashCode"/>.
        /// </summary>
        /// <typeparam name="T">The type of objects being compared.</typeparam>
        /// <param name="equalityComparer">The comparer.</param>
        /// <param name="left">A value of type <typeparamref name="T"/> or <c>null</c>.</param>
        /// <param name="right">A value of type <typeparamref name="T"/> or <c>null</c>.</param>
        public static bool ImplementOpInequality<T>(IEqualityComparer<T> equalityComparer, T left, T right)
        {
            return !equalityComparer.Equals(left, right);
        }

        /// <summary>
        /// Implements <c>op_LessThan</c>. Types overloading <c>op_LessThan</c> should also overload <c>op_Equality</c>, <c>op_Inequality</c>, <c>op_LessThanOrEqual</c>, <c>op_GreaterThan</c>, and <c>op_GreaterThanOrEqual</c>; and override <see cref="Object.Equals(Object)"/> and <see cref="Object.GetHashCode"/>.
        /// </summary>
        /// <typeparam name="T">The type of objects being compared.</typeparam>
        /// <param name="comparer">The comparer.</param>
        /// <param name="left">A value of type <typeparamref name="T"/> or <c>null</c>.</param>
        /// <param name="right">A value of type <typeparamref name="T"/> or <c>null</c>.</param>
        public static bool ImplementOpLessThan<T>(IComparer<T> comparer, T left, T right)
        {
            return comparer.Compare(left, right) < 0;
        }

        /// <summary>
        /// Implements <c>op_GreaterThan</c>. Types overloading <c>op_LessThan</c> should also overload <c>op_Equality</c>, <c>op_Inequality</c>, <c>op_LessThanOrEqual</c>, <c>op_LessThan</c>, and <c>op_GreaterThanOrEqual</c>; and override <see cref="Object.Equals(Object)"/> and <see cref="Object.GetHashCode"/>.
        /// </summary>
        /// <typeparam name="T">The type of objects being compared.</typeparam>
        /// <param name="comparer">The comparer.</param>
        /// <param name="left">A value of type <typeparamref name="T"/> or <c>null</c>.</param>
        /// <param name="right">A value of type <typeparamref name="T"/> or <c>null</c>.</param>
        public static bool ImplementOpGreaterThan<T>(IComparer<T> comparer, T left, T right)
        {
            return comparer.Compare(left, right) > 0;
        }

        /// <summary>
        /// Implements <c>op_LessThanOrEqual</c>. Types overloading <c>op_LessThan</c> should also overload <c>op_Equality</c>, <c>op_Inequality</c>, <c>op_LessThan</c>, <c>op_GreaterThan</c>, and <c>op_GreaterThanOrEqual</c>; and override <see cref="Object.Equals(Object)"/> and <see cref="Object.GetHashCode"/>.
        /// </summary>
        /// <typeparam name="T">The type of objects being compared.</typeparam>
        /// <param name="comparer">The comparer.</param>
        /// <param name="left">A value of type <typeparamref name="T"/> or <c>null</c>.</param>
        /// <param name="right">A value of type <typeparamref name="T"/> or <c>null</c>.</param>
        public static bool ImplementOpLessThanOrEqual<T>(IComparer<T> comparer, T left, T right)
        {
            return comparer.Compare(left, right) <= 0;
        }

        /// <summary>
        /// Implements <c>op_GreaterThanOrEqual</c>. Types overloading <c>op_LessThan</c> should also overload <c>op_Equality</c>, <c>op_Inequality</c>, <c>op_LessThan</c>, <c>op_GreaterThan</c>, and <c>op_LessThanOrEqual</c>; and override <see cref="Object.Equals(Object)"/> and <see cref="Object.GetHashCode"/>.
        /// </summary>
        /// <typeparam name="T">The type of objects being compared.</typeparam>
        /// <param name="comparer">The comparer.</param>
        /// <param name="left">A value of type <typeparamref name="T"/> or <c>null</c>.</param>
        /// <param name="right">A value of type <typeparamref name="T"/> or <c>null</c>.</param>
        public static bool ImplementOpGreaterThanOrEqual<T>(IComparer<T> comparer, T left, T right)
        {
            return comparer.Compare(left, right) >= 0;
        }
    }
}
