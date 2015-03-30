using System.Collections.Generic;

namespace Nito.Comparers.Util
{
    /// <summary>
    /// Common implementations for equality comparers that are based on a source comparer, possibly for a different type of object.
    /// </summary>
    /// <typeparam name="T">The type of objects compared by this comparer.</typeparam>
    /// <typeparam name="TSource">The type of objects compared by the source comparer.</typeparam>
    internal abstract class SourceEqualityComparerBase<T, TSource> : EqualityComparerBase<T>
    {
        /// <summary>
        /// The source comparer.
        /// </summary>
        protected readonly IEqualityComparer<TSource> _source;

        /// <summary>
        /// Initializes a new instance of the <see cref="SourceEqualityComparerBase{T,TSource}"/> class.
        /// </summary>
        /// <param name="source">The source comparer. If this is <c>null</c>, the default comparer is used.</param>
        /// <param name="specialNullHandling">A value indicating whether <c>null</c> values are passed to <see cref="EqualityComparerBase{T}.DoGetHashCode"/> and <see cref="EqualityComparerBase{T}.DoEquals"/>. If <c>false</c>, then <c>null</c> values are considered not equal to any non-<c>null</c> values and are not passed to <see cref="EqualityComparerBase{T}.DoGetHashCode"/> nor <see cref="EqualityComparerBase{T}.DoEquals"/>.</param>
        protected SourceEqualityComparerBase(IEqualityComparer<TSource> source, bool specialNullHandling)
            : base(specialNullHandling)
        {
            _source = EqualityComparerHelpers.NormalizeDefault(source);
        }
    }
}
