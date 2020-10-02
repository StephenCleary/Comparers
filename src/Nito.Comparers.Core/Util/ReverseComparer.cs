using System.Collections.Generic;

namespace Nito.Comparers.Util
{
    /// <summary>
    /// A comparer that reverses the evaluation of the specified source comparer.
    /// </summary>
    /// <typeparam name="T">The type of objects being compared.</typeparam>
    internal sealed class ReverseComparer<T> : SourceComparerBase<T, T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReverseComparer&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="source">The source comparer. If this is <c>null</c>, the default comparer is used.</param>
        public ReverseComparer(IComparer<T>? source)
            : base(source, null, true)
        {
        }

        /// <inheritdoc />
        protected override int DoGetHashCode(T obj) => SourceGetHashCode(obj);

        /// <inheritdoc />
        protected override int DoCompare(T x, T y) => Source.Compare(y, x);

        /// <summary>
        /// Returns a short, human-readable description of the comparer. This is intended for debugging and not for other purposes.
        /// </summary>
        public override string ToString() => $"Reverse({Source})";
    }
}
