using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace EqualityComparers.Util
{
    /// <summary>
    /// A comparer that performs a lexicographical ordering on a sequence.
    /// </summary>
    /// <typeparam name="T">The type of sequence elements being compared.</typeparam>
    public sealed class SequenceEqualityComparer<T> : SourceEqualityComparerBase<IEnumerable<T>, T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SequenceEqualityComparer&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="source">The source comparer. If this is <c>null</c>, the default comparer is used.</param>
        public SequenceEqualityComparer(IEqualityComparer<T> source)
            : base(source, false)
        {
        }

        /// <summary>
        /// Returns a hash code for the specified object.
        /// </summary>
        /// <param name="obj">The object for which to return a hash code.</param>
        /// <returns>A hash code for the specified object.</returns>
        protected override int DoGetHashCode(IEnumerable<T> obj)
        {
            Contract.Assume(obj != null);
            unchecked
            {
                var ret = (int)2166136261;
                foreach (var item in obj)
                {
                    ret += Source.GetHashCode(item);
                    ret *= 16777619;
                }
                return ret;
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
            Contract.Assume(x != null);
            Contract.Assume(y != null);

            var xCollection = x as ICollection<T>;
            if (xCollection != null)
            {
                var yCollection = y as ICollection<T>;
                if (yCollection != null)
                {
                    if (xCollection.Count != yCollection.Count)
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

                    var ret = this.Source.Equals(xIter.Current, yIter.Current);
                    if (!ret)
                        return false;
                }
            }            
        }

        /// <summary>
        /// Returns a short, human-readable description of the comparer. This is intended for debugging and not for other purposes.
        /// </summary>
        public override string ToString()
        {
            return "Sequence<" + typeof(T).Name + ">(" + this.Source + ")";
        }
    }
}
