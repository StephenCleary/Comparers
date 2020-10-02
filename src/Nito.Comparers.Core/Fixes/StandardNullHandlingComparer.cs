using Nito.Comparers.Util;
using System;
using System.Collections.Generic;

namespace Nito.Comparers.Fixes
{
    /// <summary>
    /// A comparer that handles <c>null</c> values in a standard way, and passes non-<c>null</c> values to the source comparer.
    /// </summary>
    /// <typeparam name="T">The type of objects being compared.</typeparam>
    internal sealed class StandardNullHandlingComparer<T> : SourceComparerBase<T, T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StandardNullHandlingComparer{T}"/> class.
        /// </summary>
        /// <param name="source">The source comparer. If this is <c>null</c>, the default comparer is used.</param>
        public StandardNullHandlingComparer(IComparer<T>? source)
            : base(source, null, false)
        {
        }

        /// <inheritdoc />
        protected override int DoCompare(T x, T y) => Source.Compare(x, y);

        /// <inheritdoc />
        protected override int DoGetHashCode(T obj) => SourceGetHashCode(obj);

        /// <summary>
        /// Returns a short, human-readable description of the comparer. This is intended for debugging and not for other purposes.
        /// </summary>
        public override string ToString() => $"StandardNullHandling({Source})";
    }
}
