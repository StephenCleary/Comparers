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
        public static StringComparer ToStringComparer(this IFullComparer<string>? source) => new FullStringComparer(source);
    }
}
