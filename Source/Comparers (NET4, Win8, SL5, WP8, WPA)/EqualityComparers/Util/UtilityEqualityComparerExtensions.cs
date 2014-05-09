using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace EqualityComparers.Util
{
    /// <summary>
    /// Provides extension methods for equality comparers.
    /// </summary>
    public static class UtilityEqualityComparerExtensions
    {
        /// <summary>
        /// Returns an equality comparer that works by comparing the results of the specified key selector.
        /// </summary>
        /// <typeparam name="TSource">The type of key objects being compared.</typeparam>
        /// <typeparam name="T">The type of objects being compared.</typeparam>
        /// <param name="source">The source comparer. If this is <c>null</c>, the default comparer is used.</param>
        /// <param name="selector">The key selector. May not be <c>null</c>.</param>
        /// <param name="allowNulls">A value indicating whether <c>null</c> values are passed to <paramref name="selector"/>. If <c>false</c>, then <c>null</c> values are considered less than any non-<c>null</c> values and are not passed to <paramref name="selector"/>.</param>
        /// <returns>A comparer that works by comparing the results of the specified key selector.</returns>
        public static IEqualityComparer<T> SelectEquateFrom<T, TSource>(this IEqualityComparer<TSource> source, Func<T, TSource> selector, bool allowNulls = false)
        {
            Contract.Requires(selector != null);
            Contract.Ensures(Contract.Result<IEqualityComparer<T>>() != null);
            return new SelectEqualityComparer<T, TSource>(selector, source, allowNulls);
        }
    }
}
