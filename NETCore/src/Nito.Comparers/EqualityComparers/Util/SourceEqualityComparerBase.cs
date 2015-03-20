using System;
using System.Collections.Generic;
using System.Text;

namespace Nito.EqualityComparers.Util
{
    /// <summary>
    /// Common implementations for equality comparers that are based on a source comparer, possibly for a different type of object.
    /// </summary>
    /// <typeparam name="T">The type of objects compared by this comparer.</typeparam>
    /// <typeparam name="TSource">The type of objects compared by the source comparer.</typeparam>
    public abstract class SourceEqualityComparerBase<T, TSource> : EqualityComparerBase<T>
    {
        /// <summary>
        /// The source comparer.
        /// </summary>
        private readonly IEqualityComparer<TSource> source_;

        /// <summary>
        /// Initializes a new instance of the <see cref="SourceEqualityComparerBase{T,TSource}"/> class.
        /// </summary>
        /// <param name="source">The source comparer. If this is <c>null</c>, the default comparer is used.</param>
        /// <param name="allowNulls">A value indicating whether <c>null</c> values are passed to <see cref="EqualityComparerBase{T}.DoGetHashCode"/> and <see cref="EqualityComparerBase{T}.DoEquals"/>. If <c>false</c>, then <c>null</c> values are considered not equal to any non-<c>null</c> values and are not passed to <see cref="EqualityComparerBase{T}.DoGetHashCode"/> nor <see cref="EqualityComparerBase{T}.DoEquals"/>.</param>
        protected SourceEqualityComparerBase(IEqualityComparer<TSource> source, bool allowNulls)
            : base(allowNulls)
        {
            this.source_ = EqualityComparerHelpers.NormalizeDefault(source);
        }

        /// <summary>
        /// Gets the source comparer.
        /// </summary>
        public IEqualityComparer<TSource> Source
        {
            get
            {
                return this.source_;
            }
        }
    }
}
