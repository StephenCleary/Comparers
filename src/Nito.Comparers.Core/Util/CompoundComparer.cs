using Nito.Comparers.Internals;
using System;
using System.Collections.Generic;

namespace Nito.Comparers.Util
{
    /// <summary>
    /// A comparer that uses another comparer if the source comparer determines the objects are equal.
    /// </summary>
    /// <typeparam name="T">The type of objects being compared.</typeparam>
    internal sealed class CompoundComparer<T> : SourceComparerBase<T, T>
    {
        /// <summary>
        /// The second comparer.
        /// </summary>
        private readonly IComparer<T> _secondSource;

        /// <summary>
        /// The <c>GetHashCode</c> implementation for the second comparer.
        /// </summary>
        private readonly Func<T?, int> _secondSourceGetHashCode;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompoundComparer&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="source">The source comparer. If this is <c>null</c>, the default comparer is used.</param>
        /// <param name="secondSource">The second comparer. If this is <c>null</c>, the default comparer is used.</param>
        public CompoundComparer(IComparer<T>? source, IComparer<T>? secondSource)
            : base(source, null, true)
        {
            _secondSource = ComparerHelpers.NormalizeDefault(secondSource);
            _secondSourceGetHashCode = ComparerHelpers.ComparerGetHashCode(_secondSource);
        }

        /// <inheritdoc />
        protected override int DoGetHashCode(T? obj)
        {
            unchecked
            {
                var ret = Murmur3Hash.Create(SourceGetHashCode(obj));
                ret.Combine(_secondSourceGetHashCode(obj));
                return ret.HashCode;
            }
        }

        /// <inheritdoc />
        protected override int DoCompare(T? x, T? y)
        {
            var ret = Source.Compare(x!, y!);
            if (ret != 0)
                return ret;
            return _secondSource.Compare(x!, y!);
        }

        /// <summary>
        /// Returns a short, human-readable description of the comparer. This is intended for debugging and not for other purposes.
        /// </summary>
        public override string ToString() => $"Compound({Source}, {_secondSource})";
    }
}
