using System.Collections.Generic;

namespace Nito.Comparers.Util
{
    /// <summary>
    /// A comparer that performs a lexicographical ordering on a sequence.
    /// </summary>
    /// <typeparam name="T">The type of sequence elements being compared.</typeparam>
    internal sealed class SequenceComparer<T> : ComparerBase<IEnumerable<T>>
    {
        /// <summary>
        /// The source comparer.
        /// </summary>
        private readonly IComparer<T> _source;

        /// <summary>
        /// Initializes a new instance of the <see cref="SequenceComparer{T}"/> class.
        /// </summary>
        /// <param name="source">The source comparer. If this is <c>null</c>, the default comparer is used.</param>
        public SequenceComparer(IComparer<T> source)
            : base(false)
        {
            _source = ComparerHelpers.NormalizeDefault(source);
            _sourceSequenceEqualityComparer = ComparerHelpers.GetEqualityComparerFromComparer(_source).EquateSequence();
        }

        /// <summary>
        /// The source equality comparer.
        /// </summary>
        private readonly IEqualityComparer<IEnumerable<T>> _sourceSequenceEqualityComparer;

        /// <summary>
        /// Returns a hash code for the specified object.
        /// </summary>
        /// <param name="obj">The object for which to return a hash code.</param>
        /// <returns>A hash code for the specified object.</returns>
        protected override int DoGetHashCode(IEnumerable<T> obj)
        {
            return _sourceSequenceEqualityComparer.GetHashCode(obj);
        }

        /// <summary>
        /// Compares two objects and returns a value less than 0 if <paramref name="x"/> is less than <paramref name="y"/>, 0 if <paramref name="x"/> is equal to <paramref name="y"/>, or greater than 0 if <paramref name="x"/> is greater than <paramref name="y"/>.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>A value less than 0 if <paramref name="x"/> is less than <paramref name="y"/>, 0 if <paramref name="x"/> is equal to <paramref name="y"/>, or greater than 0 if <paramref name="x"/> is greater than <paramref name="y"/>.</returns>
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
        /// Compares two objects and returns <c>true</c> if they are equal and <c>false</c> if they are not equal.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns><c>true</c> if <paramref name="x"/> is equal to <paramref name="y"/>; otherwise, <c>false</c>.</returns>
        protected override bool DoEquals(IEnumerable<T> x, IEnumerable<T> y)
        {
            return _sourceSequenceEqualityComparer.Equals(x, y);
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
