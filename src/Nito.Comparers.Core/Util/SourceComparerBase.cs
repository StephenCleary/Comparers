using System;
using System.Collections.Generic;

namespace Nito.Comparers.Util
{
    /// <summary>
    /// Common implementations for comparers that are based on a source comparer, possibly for a different type of object.
    /// </summary>
    /// <typeparam name="T">The type of objects compared by this comparer.</typeparam>
    /// <typeparam name="TSource">The type of objects compared by the source comparer.</typeparam>
    internal abstract class SourceComparerBase<T, TSource> : ComparerBase<T>
    {
        /// <summary>
        /// The source comparer.
        /// </summary>
        protected readonly IComparer<TSource> Source;

        /// <summary>
        /// The <c>GetHashCode</c> implementation for the source comparer.
        /// </summary>
        protected readonly Func<TSource?, int> SourceGetHashCode;

        /// <summary>
        /// Initializes a new instance of the <see cref="SourceComparerBase&lt;T, TSource&gt;"/> class.
        /// </summary>
        /// <param name="source">The source comparer. If this is <c>null</c>, the default comparer is used.</param>
        /// <param name="getHashCode">The <c>GetHashCode</c> implementation to use. If this is <c>null</c>, this type will attempt to find <c>GetHashCode</c> on <paramref name="source"/>; if none is found, throws an exception.</param>
        /// <param name="specialNullHandling">A value indicating whether <c>null</c> values are passed to <see cref="EqualityComparerBase{T}.DoGetHashCode"/> and <see cref="ComparerBase{T}.DoCompare"/>. If <c>false</c>, then <c>null</c> values are considered less than any non-<c>null</c> values and are not passed to <see cref="EqualityComparerBase{T}.DoGetHashCode"/> nor <see cref="ComparerBase{T}.DoCompare"/>. This value is ignored if <typeparamref name="T"/> is a non-nullable type.</param>
        protected SourceComparerBase(IComparer<TSource>? source, Func<TSource, int>? getHashCode, bool specialNullHandling)
            : base(specialNullHandling)
        {
            Source = ComparerHelpers.NormalizeDefault(source);
#pragma warning disable CS8619 // Nullability of reference types in value doesn't match target type.
			SourceGetHashCode = getHashCode ?? ComparerHelpers.ComparerGetHashCode(Source);
#pragma warning restore CS8619 // Nullability of reference types in value doesn't match target type.
		}
    }
}
