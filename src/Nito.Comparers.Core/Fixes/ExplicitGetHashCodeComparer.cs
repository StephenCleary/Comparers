using Nito.Comparers.Util;
using System;
using System.Collections.Generic;

namespace Nito.Comparers.Fixes
{
    /// <summary>
    /// A comparer that handles <c>null</c> values in a standard way, and passes non-<c>null</c> values to the source comparer.
    /// </summary>
    /// <typeparam name="T">The type of objects being compared.</typeparam>
    internal sealed class ExplicitGetHashCodeComparer<T> : SourceComparerBase<T, T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExplicitGetHashCodeComparer{T}"/> class.
        /// </summary>
        /// <param name="source">The source comparer. If this is <c>null</c>, the default comparer is used.</param>
        /// <param name="getHashCode">The <c>GetHashCode</c> implementation to use. If this is <c>null</c>, this type will attempt to find <c>GetHashCode</c> on <paramref name="source"/>; if none is found, throws an exception.</param>
        public ExplicitGetHashCodeComparer(IComparer<T>? source, Func<T, int>? getHashCode)
            : base(source, getHashCode, false)
        {
        }

        /// <inheritdoc />
        protected override int DoGetHashCode(T? obj) => SourceGetHashCode(obj);

		/// <inheritdoc />
		protected override int DoCompare(T? x, T? y) => Source.Compare(x!, y!);

		/// <summary>
		/// Returns a short, human-readable description of the comparer. This is intended for debugging and not for other purposes.
		/// </summary>
		public override string ToString() => $"ExplicitGetHashCode({Source})";
    }
}
