using Nito.Comparers.Internals;
using System.Collections.Generic;

namespace Nito.Comparers.Util
{
    /// <summary>
    /// A equality comparer that uses another comparer if the source comparer determines the objects are equal.
    /// </summary>
    /// <typeparam name="T">The type of objects being compared.</typeparam>
    internal sealed class CompoundEqualityComparer<T> : SourceEqualityComparerBase<T, T>
    {
        /// <summary>
        /// The second comparer.
        /// </summary>
        private readonly IEqualityComparer<T> _secondSource;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompoundEqualityComparer&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="source">The source comparer. If this is <c>null</c>, the default comparer is used.</param>
        /// <param name="secondSource">The second comparer. If this is <c>null</c>, the default comparer is used.</param>
        public CompoundEqualityComparer(IEqualityComparer<T> source, IEqualityComparer<T> secondSource)
            : base(source, true)
        {
            _secondSource = EqualityComparerHelpers.NormalizeDefault(secondSource);
        }

        /// <inheritdoc />
        protected override int DoGetHashCode(T obj)
        {
            unchecked
            {
                var ret = Murmur3Hash.Create(Source.GetHashCode(obj));
                ret.Combine(_secondSource.GetHashCode(obj));
                return ret.HashCode;
            }
        }

        /// <inheritdoc />
        protected override bool DoEquals(T x, T y)
        {
            var ret = Source.Equals(x, y);
            if (!ret)
                return false;
            return _secondSource.Equals(x, y);
        }

        /// <summary>
        /// Returns a short, human-readable description of the comparer. This is intended for debugging and not for other purposes.
        /// </summary>
        public override string ToString() => $"Compound({Source}, {_secondSource})";
    }
}
