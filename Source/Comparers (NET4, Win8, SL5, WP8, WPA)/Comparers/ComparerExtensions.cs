using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using Comparers.Util;

namespace Comparers
{
    /// <summary>
    /// Provides extension methods for comparers.
    /// </summary>
    public static class ComparerExtensions
    {
        /// <summary>
        /// Returns a comparer that reverses the evaluation of the specified source comparer.
        /// </summary>
        /// <typeparam name="T">The type of objects being compared.</typeparam>
        /// <param name="source">The source comparer. If this is <c>null</c>, the default comparer is used.</param>
        /// <returns>A comparer that reverses the evaluation of the specified source comparer.</returns>
        public static IFullComparer<T> Reverse<T>(this IComparer<T> source)
        {
            Contract.Ensures(Contract.Result<IFullComparer<T>>() != null);
            return new ReverseComparer<T>(source);
        }

        /// <summary>
        /// Returns a comparer that uses another comparer if the source comparer determines the objects are equal.
        /// </summary>
        /// <typeparam name="T">The type of objects being compared.</typeparam>
        /// <param name="source">The source comparer. If this is <c>null</c>, the default comparer is used.</param>
        /// <param name="thenBy">The comparer that is used if <paramref name="source"/> determines the objects are equal. If this is <c>null</c>, the default comparer is used.</param>
        /// <returns>A comparer that uses another comparer if the source comparer determines the objects are equal.</returns>
        public static IFullComparer<T> ThenBy<T>(this IComparer<T> source, IComparer<T> thenBy)
        {
            Contract.Ensures(Contract.Result<IFullComparer<T>>() != null);
            return new CompoundComparer<T>(source, thenBy);
        }

        /// <summary>
        /// Returns a comparer that uses a key comparer if the source comparer determines the objects are equal.
        /// </summary>
        /// <typeparam name="T">The type of objects being compared.</typeparam>
        /// <typeparam name="TKey">The type of key objects being compared.</typeparam>
        /// <param name="source">The source comparer. If this is <c>null</c>, the default comparer is used.</param>
        /// <param name="selector">The key selector. May not be <c>null</c>.</param>
        /// <param name="keyComparer">The key comparer. Defaults to <c>null</c>. If this is <c>null</c>, the default comparer is used.</param>
        /// <param name="allowNulls">A value indicating whether <c>null</c> values are passed to <paramref name="selector"/>. If <c>false</c>, then <c>null</c> values are considered less than any non-<c>null</c> values and are not passed to <paramref name="selector"/>.</param>
        /// <returns>A comparer that uses a key comparer if the source comparer determines the objects are equal.</returns>
        public static IFullComparer<T> ThenBy<T, TKey>(this IComparer<T> source, Func<T, TKey> selector, IComparer<TKey> keyComparer = null, bool allowNulls = false)
        {
            Contract.Requires(selector != null);
            Contract.Ensures(Contract.Result<IFullComparer<T>>() != null);
            return source.ThenBy(keyComparer.SelectFrom(selector, allowNulls));
        }

        /// <summary>
        /// Returns a comparer that uses a descending key comparer if the source comparer determines the objects are equal.
        /// </summary>
        /// <typeparam name="T">The type of objects being compared.</typeparam>
        /// <typeparam name="TKey">The type of key objects being compared.</typeparam>
        /// <param name="source">The source comparer. If this is <c>null</c>, the default comparer is used.</param>
        /// <param name="selector">The key selector. May not be <c>null</c>.</param>
        /// <param name="keyComparer">The key comparer. The returned comparer applies this key comparer in reverse. Defaults to <c>null</c>. If this is <c>null</c>, the default comparer is used.</param>
        /// <param name="allowNulls">A value indicating whether <c>null</c> values are passed to <paramref name="selector"/>. If <c>false</c>, then <c>null</c> values are considered less than any non-<c>null</c> values and are not passed to <paramref name="selector"/>.</param>
        /// <returns>A comparer that uses a key comparer if the source comparer determines the objects are equal.</returns>
        public static IFullComparer<T> ThenByDescending<T, TKey>(this IComparer<T> source, Func<T, TKey> selector, IComparer<TKey> keyComparer = null, bool allowNulls = false)
        {
            Contract.Requires(selector != null);
            Contract.Ensures(Contract.Result<IFullComparer<T>>() != null);
            return ThenBy<T, TKey>(source, selector, keyComparer.Reverse(), allowNulls);
        }

        /// <summary>
        /// Returns a comparer that will perform a lexicographical ordering on a sequence of items.
        /// </summary>
        /// <typeparam name="T">The type of sequence elements being compared.</typeparam>
        /// <param name="source">The source comparer. If this is <c>null</c>, the default comparer is used.</param>
        /// <returns>A comparer that will perform a lexicographical ordering on a sequence of items.</returns>
        public static IFullComparer<IEnumerable<T>> Sequence<T>(this IComparer<T> source)
        {
            Contract.Ensures(Contract.Result<IFullComparer<IEnumerable<T>>>() != null);
            return new SequenceComparer<T>(source);
        }
    }
}
