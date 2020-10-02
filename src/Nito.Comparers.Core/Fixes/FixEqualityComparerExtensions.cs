using System;
using System.Collections.Generic;
using System.Text;

namespace Nito.Comparers.Fixes
{
    /// <summary>
    /// Provides extension methods for equality comparers.
    /// </summary>
    public static class FixEqualityComparerExtensions
    {
        /// <summary>
        /// Wraps this equality comparer with one that handles <c>null</c> values in the standard way.
        /// </summary>
        /// <typeparam name="T">The type of objects being compared.</typeparam>
        /// <param name="source">The source comparer. If this is <c>null</c>, the default comparer is used.</param>
        public static IFullEqualityComparer<T> WithStandardNullHandlingForEquality<T>(this IEqualityComparer<T>? source) =>
            new StandardNullHandlingEqualityComparer<T>(source);
    }
}
