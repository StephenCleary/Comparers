using System.Collections.Generic;

namespace Nito.Comparers.Util
{
    /// <summary>
    /// A comparer that performs a lexicographical ordering on a sequence.
    /// </summary>
    /// <typeparam name="T">The type of sequence elements being compared.</typeparam>
    internal sealed class SequenceComparer<T> : SourceComparerBase<IEnumerable<T>, T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SequenceComparer&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="source">The source comparer. If this is <c>null</c>, the default comparer is used.</param>
        public SequenceComparer(IComparer<T> source)
            : base(source, false)
        {
        }

        /// <inheritdoc />
        protected override int DoGetHashCode(IEnumerable<T> obj)
        {
            unchecked
            {
                var ret = (int)2166136261;
                foreach (var item in obj)
                {
                    ret += ComparerHelpers.GetHashCodeFromComparer(_source, item);
                    ret *= 16777619;
                }
                return ret;
            }
        }

        /// <inheritdoc />
        protected override int DoCompare(IEnumerable<T> x, IEnumerable<T> y)
        {
            using (var xIter = x.GetEnumerator())
            using (var yIter = y.GetEnumerator())
            {
                while (true)
                {
                    if (!xIter.MoveNext())
                    {
                        if (!yIter.MoveNext())
                            return 0;
                        return -1;
                    }

                    if (!yIter.MoveNext())
                        return 1;

                    var ret = _source.Compare(xIter.Current, yIter.Current);
                    if (ret != 0)
                        return ret;
                }
            }
        }

        /// <summary>
        /// Returns a short, human-readable description of the comparer. This is intended for debugging and not for other purposes.
        /// </summary>
        public override string ToString()
        {
            return "Sequence<" + typeof(T).Name + ">(" + _source + ")";
        }
    }
}
