using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using EqualityComparers.Util;

namespace EqualityComparers
{
    /// <summary>
    /// Provides extension methods for equality comparers.
    /// </summary>
    public static class EqualityComparerExtensions
    {
        /// <summary>
        /// Returns an equality comparer that uses another comparer if the source comparer determines the objects are equal.
        /// </summary>
        /// <typeparam name="T">The type of objects being compared.</typeparam>
        /// <param name="source">The source comparer. If this is <c>null</c>, the default comparer is used.</param>
        /// <param name="thenBy">The comparer that is used if <paramref name="source"/> determines the objects are equal. If this is <c>null</c>, the default comparer is used.</param>
        /// <returns>A comparer that uses another comparer if the source comparer determines the objects are equal.</returns>
        public static IEqualityComparer<T> ThenEquateBy<T>(this IEqualityComparer<T> source, IEqualityComparer<T> thenBy)
        {
            Contract.Ensures(Contract.Result<IEqualityComparer<T>>() != null);
            return new CompoundEqualityComparer<T>(source, thenBy);
        }

        /// <summary>
        /// Returns an equality comparer that uses a key comparer if the source comparer determines the objects are equal.
        /// </summary>
        /// <typeparam name="T">The type of objects being compared.</typeparam>
        /// <typeparam name="TKey">The type of key objects being compared.</typeparam>
        /// <param name="source">The source comparer. If this is <c>null</c>, the default comparer is used.</param>
        /// <param name="selector">The key selector. May not be <c>null</c>.</param>
        /// <param name="keyComparer">The key comparer. Defaults to <c>null</c>. If this is <c>null</c>, the default comparer is used.</param>
        /// <param name="allowNulls">A value indicating whether <c>null</c> values are passed to <paramref name="selector"/>. If <c>false</c>, then <c>null</c> values are considered less than any non-<c>null</c> values and are not passed to <paramref name="selector"/>.</param>
        /// <returns>A comparer that uses a key comparer if the source comparer determines the objects are equal.</returns>
        public static IEqualityComparer<T> ThenEquateBy<T, TKey>(this IEqualityComparer<T> source, Func<T, TKey> selector, IEqualityComparer<TKey> keyComparer = null, bool allowNulls = false)
        {
            Contract.Requires(selector != null);
            Contract.Ensures(Contract.Result<IEqualityComparer<T>>() != null);
            return source.ThenEquateBy(keyComparer.SelectEquateFrom(selector, allowNulls));
        }

        /// <summary>
        /// Returns an equality comparer that will perform a lexicographical ordering on a sequence of items.
        /// </summary>
        /// <typeparam name="T">The type of sequence elements being compared.</typeparam>
        /// <param name="source">The source comparer. If this is <c>null</c>, the default comparer is used.</param>
        /// <returns>A comparer that will perform a lexicographical ordering on a sequence of items.</returns>
        public static IEqualityComparer<IEnumerable<T>> EquateSequence<T>(this IEqualityComparer<T> source)
        {
            Contract.Ensures(Contract.Result<IEqualityComparer<IEnumerable<T>>>() != null);
            return new SequenceEqualityComparer<T>(source);
        }
    }
}
