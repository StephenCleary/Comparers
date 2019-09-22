using System;
using System.Collections.Generic;
using Nito.Comparers.Util;

namespace Nito.Comparers
{
    /// <summary>
    /// Provides extension methods for string comparers.
    /// </summary>
    public static class StringComparerExtensions
    {
        /// <summary>
        /// Creates a <see cref="StringComparer"/> that wraps the provided comparer.
        /// </summary>
        /// <param name="source">The source comparer. If this is <c>null</c>, the default comparer is used.</param>
        /// <returns>A <see cref="StringComparer"/> instance.</returns>
        public static StringComparer ToStringComparer(this IFullComparer<string> source) => new FullStringComparer(source);

        /// <summary>
        /// Creates a <see cref="IFullComparer{T}"/> that wraps the provided <see cref="StringComparer"/>.
        /// Note that the returned <see cref="IFullComparer{T}"/> will pass <c>null</c> values through to the <see cref="StringComparer"/>; if this is not the desired behavior, call <see cref="O:Nito.Comparers.Fixes.FixComparerExtensions.WithStandardNullHandling"/> instead.
        /// </summary>
        /// <param name="source">The source comparer. If this is <c>null</c>, the default string comparer is used.</param>
        /// <returns>An <see cref="IFullComparer{T}"/> instance.</returns>
        public static IFullComparer<string> ToFullComparer(this StringComparer source) => new SourceComparer<string>(source, null);
    }
}
