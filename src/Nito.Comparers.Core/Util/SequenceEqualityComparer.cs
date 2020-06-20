using Nito.Comparers.Internals;
using System.Collections.Generic;

namespace Nito.Comparers.Util
{
    /// <summary>
    /// A comparer that performs a lexicographical ordering on a sequence.
    /// </summary>
    /// <typeparam name="T">The type of sequence elements being compared.</typeparam>
    internal sealed class SequenceEqualityComparer<T> : SourceEqualityComparerBase<IEnumerable<T>, T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SequenceEqualityComparer&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="source">The source comparer. If this is <c>null</c>, the default comparer is used.</param>
        public SequenceEqualityComparer(IEqualityComparer<T>? source)
            : base(source, false)
        {
        }

        /// <inheritdoc />
        protected override int DoGetHashCode(IEnumerable<T> obj)
        {
            unchecked
            {
                var ret = Murmur3Hash.Create();
                foreach (var item in obj)
                    ret.Combine(Source.GetHashCode(item));
                return ret.HashCode;
            }
        }

        /// <inheritdoc />
        protected override bool DoEquals(IEnumerable<T> x, IEnumerable<T> y)
        {
            var xCount = x.TryGetCount();
            if (xCount != null)
            {
                var yCount = y.TryGetCount();
                if (yCount != null)
                {
                    if (xCount.Value != yCount.Value)
                        return false;
                }
            }

            using (var xIter = x.GetEnumerator())
            using (var yIter = y.GetEnumerator())
            {
                while (true)
                {
                    if (!xIter.MoveNext())
                    {
                        if (!yIter.MoveNext())
                            return true;
                        return false;
                    }

                    if (!yIter.MoveNext())
                        return false;

                    var ret = Source.Equals(xIter.Current, yIter.Current);
                    if (!ret)
                        return false;
                }
            }            
        }

        /// <summary>
        /// Returns a short, human-readable description of the comparer. This is intended for debugging and not for other purposes.
        /// </summary>
        public override string ToString() => $"Sequence<{typeof(T).Name}>({Source})";
    }
}
