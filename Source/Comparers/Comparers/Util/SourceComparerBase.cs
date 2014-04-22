using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using EqualityComparers.Util;

namespace Comparers.Util
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
        private readonly IComparer<TSource> source_;

        /// <summary>
        /// Initializes a new instance of the <see cref="SourceComparerBase&lt;T, TSource&gt;"/> class.
        /// </summary>
        /// <param name="source">The source comparer. If this is <c>null</c>, the default comparer is used.</param>
        /// <param name="allowNulls">A value indicating whether <c>null</c> values are passed to <see cref="EqualityComparerBase{T}.DoGetHashCode"/> and <see cref="ComparerBase{T}.DoCompare"/>. If <c>false</c>, then <c>null</c> values are considered less than any non-<c>null</c> values and are not passed to <see cref="EqualityComparerBase{T}.DoGetHashCode"/> nor <see cref="ComparerBase{T}.DoCompare"/>.</param>
        protected SourceComparerBase(IComparer<TSource> source, bool allowNulls)
            : base(allowNulls)
        {
            this.source_ = ComparerHelpers.NormalizeDefault(source);
        }

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(this.source_ != null);
        }

        /// <summary>
        /// Gets the source comparer.
        /// </summary>
        public IComparer<TSource> Source
        {
            get
            {
                Contract.Ensures(Contract.Result<IComparer<TSource>>() != null);
                return this.source_;
            }
        }
    }
}
