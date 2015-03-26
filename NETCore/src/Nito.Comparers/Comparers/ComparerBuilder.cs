using System;
using System.Collections.Generic;
using Nito.Comparers.Util;

namespace Nito.Comparers
{
    /// <summary>
    /// Provides sources for comparers.
    /// </summary>
    /// <typeparam name="T">The type of objects being compared.</typeparam>
    public struct ComparerBuilder<T>
    {
        /// <summary>
        /// Gets the null comparer for this type, which evaluates all objects as equivalent.
        /// </summary>
        public IFullComparer<T> Null()
        {
            return NullComparer<T>.Instance;
        }

        /// <summary>
        /// Gets the default comparer for this type.
        /// </summary>
        public IFullComparer<T> Default()
        {
            return (IFullComparer<T>)ComparerHelpers.NormalizeDefault<T>(null);
        }

        /// <summary>
        /// Creates a key comparer.
        /// </summary>
        /// <typeparam name="TKey">The type of key objects being compared.</typeparam>
        /// <param name="selector">The key selector. May not be <c>null</c>.</param>
        /// <param name="keyComparer">The key comparer. Defaults to <c>null</c>. If this is <c>null</c>, the default comparer is used.</param>
        /// <param name="specialNullHandling">A value indicating whether <c>null</c> values are passed to <paramref name="selector"/>. If <c>false</c>, then <c>null</c> values are considered less than any non-<c>null</c> values and are not passed to <paramref name="selector"/>.</param>
        /// <param name="descending">A value indicating whether the sorting is done in descending order. If <c>false</c> (the default), then the sort is in ascending order.</param>
        /// <returns>A key comparer.</returns>
        public IFullComparer<T> OrderBy<TKey>(Func<T, TKey> selector, IComparer<TKey> keyComparer = null, bool specialNullHandling = false, bool descending = false)
        {
            return Null().ThenBy(selector, keyComparer, specialNullHandling, descending);
        }
    }

    /// <summary>
    /// Provides sources for comparers, inferring the type being compared.
    /// </summary>
    public static class ComparerBuilder
    {
        /// <summary>
        /// Creates a source for a comparer of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of objects being compared.</typeparam>
        public static ComparerBuilder<T> For<T>()
        {
            return new ComparerBuilder<T>();
        }

        /// <summary>
        /// Creates a source for a comparer of type <typeparamref name="T"/>. <paramref name="expression"/> is only used to infer the type <typeparamref name="T"/>; it is not evaluated.
        /// </summary>
        /// <typeparam name="T">The type of objects being compared.</typeparam>
        /// <param name="expression">An expression of the type of objects being compared. The expression is only used to infer the type <typeparamref name="T"/>; it is not evaluated.</param>
        public static ComparerBuilder<T> For<T>(Func<T> expression)
        {
            return new ComparerBuilder<T>();
        }

        /// <summary>
        /// Creates a source for a comparer of type <typeparamref name="T"/>. <paramref name="expression"/> is only used to infer the type <typeparamref name="T"/>; it is not evaluated.
        /// </summary>
        /// <typeparam name="T">The type of objects being compared.</typeparam>
        /// <param name="expression">An expression whose results are a sequence of objects being compared. The expression is only used to infer the type <typeparamref name="T"/>; it is not evaluated.</param>
        public static ComparerBuilder<T> ForElementsOf<T>(Func<IEnumerable<T>> expression)
        {
            return new ComparerBuilder<T>();
        }

        /// <summary>
        /// Creates a source for a comparer of type <typeparamref name="T"/>. <paramref name="sequence"/> is only used to infer the type <typeparamref name="T"/>; it is not enumerated.
        /// </summary>
        /// <typeparam name="T">The type of objects being compared.</typeparam>
        /// <param name="sequence">A sequence of objects being compared. This argument is only used to infer the type <typeparamref name="T"/>; it is not enumerated.</param>
        public static ComparerBuilder<T> ForElementsOf<T>(IEnumerable<T> sequence)
        {
            return new ComparerBuilder<T>();
        }
    }
}