using Nito.Comparers.Internals;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nito.Comparers.Util
{
    /// <summary>
    /// A comparer that performs an unordered equality comparison on a sequence.
    /// </summary>
    /// <typeparam name="T">The type of sequence elements being compared.</typeparam>
    internal sealed class UnorderedSequenceEqualityComparer<T> : SourceEqualityComparerBase<IEnumerable<T>, T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnorderedSequenceEqualityComparer&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="source">The source comparer. If this is <c>null</c>, the default comparer is used.</param>
        public UnorderedSequenceEqualityComparer(IEqualityComparer<T>? source)
            : base(source, false)
        {
        }

        /// <inheritdoc />
        protected override int DoGetHashCode(IEnumerable<T> obj)
        {
            var ret = CommutativeHashCombiner.Create();
            foreach (var item in obj)
                ret.Combine(Source.GetHashCode(item!));
            return ret.HashCode;
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
                    if (xCount.Value == 0)
                        return true;
                }
            }

            var equivalenceClassCounts = new Dictionary<Wrapper, int>(EqualityComparerBuilder.For<Wrapper>().EquateBy(w => w.Value, Source));

            using (var xIter = x.GetEnumerator())
            using (var yIter = y.GetEnumerator())
            {
                while (true)
                {
                    if (!xIter.MoveNext())
                    {
                        if (!yIter.MoveNext())
                        {
                            // We have reached the end of both sequences simultaneously.
                            // They are equivalent if all equivalence class counts have canceled each other out.
                            return equivalenceClassCounts.All(x => x.Value == 0);
                        }

                        return false;
                    }

                    if (!yIter.MoveNext())
                        return false;

                    // If both items are equivalent, just skip the equivalence class counts.
                    var ret = Source.Equals(xIter.Current, yIter.Current);
                    if (ret)
                        continue;

                    var xKey = new Wrapper { Value = xIter.Current };
                    var yKey = new Wrapper { Value = yIter.Current };

                    // Treat `x` as adding counts and `y` as subtracting counts; any counts not present are 0.
                    if (equivalenceClassCounts.ContainsKey(xKey))
                        ++equivalenceClassCounts[xKey];
                    else
                        equivalenceClassCounts[xKey] = 1;
                    if (equivalenceClassCounts.ContainsKey(yKey))
                        --equivalenceClassCounts[yKey];
                    else
                        equivalenceClassCounts[yKey] = -1;
                }
            }            
        }

        /// <summary>
        /// Returns a short, human-readable description of the comparer. This is intended for debugging and not for other purposes.
        /// </summary>
        public override string ToString() => $"UnorderedSequence<{typeof(T).Name}>({Source})";

        private struct Wrapper
        {
            public T Value { get; set; }
        }
    }
}
