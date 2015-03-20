using System;
using System.Collections.Generic;
using System.Text;
using Nito.EqualityComparers.Util;

namespace Nito.Comparers.Util
{
    /// <summary>
    /// Common implementations for comparers that are based on a source comparer, possibly for a different type of object.
    /// </summary>
    /// <typeparam name="T">The type of objects compared by this comparer.</typeparam>
    /// <typeparam name="TSource">The type of objects compared by the source comparer.</typeparam>
    public abstract class SourceComparerBase<T, TSource> : ComparerBase<T>
    {
        /// <summary>
        /// The source comparer.
        /// </summary>
        private readonly IComparer<TSource> _source;

        /// <summary>
        /// Initializes a new instance of the <see cref="SourceComparerBase&lt;T, TSource&gt;"/> class.
        /// </summary>
        /// <param name="source">The source comparer. If this is <c>null</c>, the default comparer is used.</param>
        /// <param name="specialNullHandling">A value indicating whether <c>null</c> values are passed to <see cref="EqualityComparerBase{T}.DoGetHashCode"/> and <see cref="ComparerBase{T}.DoCompare"/>. If <c>false</c>, then <c>null</c> values are considered less than any non-<c>null</c> values and are not passed to <see cref="EqualityComparerBase{T}.DoGetHashCode"/> nor <see cref="ComparerBase{T}.DoCompare"/>.</param>
        protected SourceComparerBase(IComparer<TSource> source, bool specialNullHandling)
            : base(specialNullHandling)
        {
            _source = ComparerHelpers.NormalizeDefault(source);
        }

        /// <summary>
        /// Gets the source comparer.
        /// </summary>
        protected IComparer<TSource> Source
        {
            get
            {
                return _source;
            }
        }
    }
}
