using System;
using System.Collections.Generic;
using System.Text;
using Nito.Comparers.Util;

namespace Nito.Comparers
{
    /// <summary>
    /// Provides sources for comparers.
    /// </summary>
    /// <typeparam name="T">The type of objects being compared.</typeparam>
    public sealed class CompareSource<T>
    {
        /// <summary>
        /// Gets the null comparer for this type, which evaluates all objects as equivalent.
        /// </summary>
        public IFullComparer<T> Null()
        {
            return Compare<T>.Null();
        }

        /// <summary>
        /// Gets the default comparer for this type.
        /// </summary>
        public IFullComparer<T> Default()
        {
            return Compare<T>.Default();
        }

        /// <summary>
        /// Creates a key comparer.
        /// </summary>
        /// <typeparam name="TKey">The type of key objects being compared.</typeparam>
        /// <param name="selector">The key selector. May not be <c>null</c>.</param>
        /// <param name="keyComparer">The key comparer. Defaults to <c>null</c>. If this is <c>null</c>, the default comparer is used.</param>
        /// <param name="allowNulls">A value indicating whether <c>null</c> values are passed to <paramref name="selector"/>. If <c>false</c>, then <c>null</c> values are considered less than any non-<c>null</c> values and are not passed to <paramref name="selector"/>.</param>
        /// <param name="descending">A value indicating whether the sorting is done in descending order. If <c>false</c> (the default), then the sort is in ascending order.</param>
        /// <returns>A key comparer.</returns>
        public IFullComparer<T> OrderBy<TKey>(Func<T, TKey> selector, IComparer<TKey> keyComparer = null, bool allowNulls = false, bool descending = false)
        {
            return Compare<T>.OrderBy(selector, keyComparer, allowNulls, descending);
        }

        /// <summary>
        /// Creates a descending key comparer.
        /// </summary>
        /// <typeparam name="TKey">The type of key objects being compared.</typeparam>
        /// <param name="selector">The key selector. May not be <c>null</c>.</param>
        /// <param name="keyComparer">The key comparer. The returned comparer applies this key comparer in reverse. Defaults to <c>null</c>. If this is <c>null</c>, the default comparer is used.</param>
        /// <param name="allowNulls">A value indicating whether <c>null</c> values are passed to <paramref name="selector"/>. If <c>false</c>, then <c>null</c> values are considered less than any non-<c>null</c> values and are not passed to <paramref name="selector"/>.</param>
        /// <returns>A key comparer.</returns>
        public IFullComparer<T> OrderByDescending<TKey>(Func<T, TKey> selector, IComparer<TKey> keyComparer = null, bool allowNulls = false)
        {
            return Compare<T>.OrderByDescending(selector, keyComparer, allowNulls);
        }
    }

    /// <summary>
    /// Provides sources for comparers, inferring the type being compared.
    /// </summary>
    public static class CompareSource
    {
        /// <summary>
        /// Creates a source for a comparer of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of objects being compared.</typeparam>
        public static CompareSource<T> For<T>()
        {
            return new CompareSource<T>();
        }

        /// <summary>
        /// Creates a source for a comparer of type <typeparamref name="T"/>. <paramref name="expression"/> is only used to infer the type <typeparamref name="T"/>; it is not evaluated.
        /// </summary>
        /// <typeparam name="T">The type of objects being compared.</typeparam>
        /// <param name="expression">An expression of the type of objects being compared. The expression is only used to infer the type <typeparamref name="T"/>; it is not evaluated.</param>
        public static CompareSource<T> For<T>(Func<T> expression)
        {
            return new CompareSource<T>();
        }

        /// <summary>
        /// Creates a source for a comparer of type <typeparamref name="T"/>. <paramref name="expression"/> is only used to infer the type <typeparamref name="T"/>; it is not evaluated.
        /// </summary>
        /// <typeparam name="T">The type of objects being compared.</typeparam>
        /// <param name="expression">An expression whose results are a sequence of objects being compared. The expression is only used to infer the type <typeparamref name="T"/>; it is not evaluated.</param>
        public static CompareSource<T> ForElementsOf<T>(Func<IEnumerable<T>> expression)
        {
            return new CompareSource<T>();
        }

        /// <summary>
        /// Creates a source for a comparer of type <typeparamref name="T"/>. <paramref name="sequence"/> is only used to infer the type <typeparamref name="T"/>; it is not enumerated.
        /// </summary>
        /// <typeparam name="T">The type of objects being compared.</typeparam>
        /// <param name="sequence">A sequence of objects being compared. This argument is only used to infer the type <typeparamref name="T"/>; it is not enumerated.</param>
        public static CompareSource<T> ForElementsOf<T>(IEnumerable<T> sequence)
        {
            return new CompareSource<T>();
        }
    }
}