using System;
using System.Collections.Generic;
using System.Text;
using Nito.Comparers.Util;

namespace Nito.Comparers.Fixes
{
    /// <summary>
    /// An equality comparer that handles <c>null</c> values in a standard way, and passes non-<c>null</c> values to the source comparer.
    /// </summary>
    /// <typeparam name="T">The type of objects being compared.</typeparam>
    internal sealed class StandardNullHandlingEqualityComparer<T> : SourceEqualityComparerBase<T, T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StandardNullHandlingEqualityComparer{T}"/> class.
        /// </summary>
        /// <param name="source">The source comparer. If this is <c>null</c>, the default comparer is used.</param>
        public StandardNullHandlingEqualityComparer(IEqualityComparer<T>? source)
            : base(source, false)
        {
        }

        /// <inheritdoc />
        protected override int DoGetHashCode(T obj) => Source.GetHashCode(obj!);

        /// <inheritdoc />
        protected override bool DoEquals(T x, T y) => Source.Equals(x, y);

        /// <summary>
        /// Returns a short, human-readable description of the comparer. This is intended for debugging and not for other purposes.
        /// </summary>
        public override string ToString() => $"StandardNullHandling({Source})";
    }
}
