using System;
using System.Collections.Generic;

namespace Nito.Comparers.Util
{
    /// <summary>
    /// A comparer that just forwards to another comparer.
    /// Note that this wrapper forces the wrapped comparer to implement <c>GetHashCode</c>.
    /// </summary>
    /// <typeparam name="T">The type of objects compared by this comparer.</typeparam>
    internal sealed class SourceComparer<T> : SourceComparerBase<T, T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SourceComparer&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="source">The source comparer. If this is <c>null</c>, the default comparer is used.</param>
        /// <param name="getHashCode">The <c>GetHashCode</c> implementation to use. If this is <c>null</c>, this type will attempt to find <c>GetHashCode</c> on <paramref name="source"/>; if none is found, throws an exception.</param>
        public SourceComparer(IComparer<T> source, Func<T, int> getHashCode)
            : base(source, getHashCode, true)
        {
        }

        protected override int DoGetHashCode(T obj) => SourceGetHashCode(obj);

        protected override int DoCompare(T x, T y) => Source.Compare(x, y);

        public override string ToString() => $"Source({Source})";
    }
}
