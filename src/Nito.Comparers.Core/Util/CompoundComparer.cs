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
        /// Initializes a new instance of the <see cref="CompoundComparer&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="source">The source comparer. If this is <c>null</c>, the default comparer is used.</param>
        /// <param name="secondSource">The second comparer. If this is <c>null</c>, the default comparer is used.</param>
        public CompoundComparer(IComparer<T> source, IComparer<T> secondSource)
            : base(source, true)
        {
            _secondSource = ComparerHelpers.NormalizeDefault(secondSource);
        }

        /// <inheritdoc />
        protected override int DoGetHashCode(T obj)
        {
            unchecked
            {
                var ret = (int)2166136261;
                ret += ComparerHelpers.GetHashCodeFromComparer(_source, obj);
                ret *= 16777619;
                ret += ComparerHelpers.GetHashCodeFromComparer(_secondSource, obj);
                ret *= 16777619;
                return ret;
            }
        }

        /// <inheritdoc />
        protected override int DoCompare(T x, T y)
        {
            var ret = _source.Compare(x, y);
            if (ret != 0)
                return ret;
            return _secondSource.Compare(x, y);
        }

        /// <summary>
        /// Returns a short, human-readable description of the comparer. This is intended for debugging and not for other purposes.
        /// </summary>
        public override string ToString()
        {
            return "Compound(" + _source + ", " + _secondSource + ")";
        }
    }
}
