using System;
using System.Collections.Generic;

namespace Nito.Comparers.Util
{
    /// <summary>
    /// An equality comparer that works by comparing the results of the specified key selector.
    /// </summary>
    /// <typeparam name="TSource">The type of key objects being compared.</typeparam>
    /// <typeparam name="T">The type of objects being compared.</typeparam>
    internal sealed class SelectEqualityComparer<T, TSource> : SourceEqualityComparerBase<T, TSource>
    {
        /// <summary>
        /// The key selector.
        /// </summary>
        private readonly Func<T, TSource> _selector;

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectEqualityComparer&lt;T, TSource&gt;"/> class.
        /// </summary>
        /// <param name="selector">The key selector. May not be <c>null</c>.</param>
        /// <param name="source">The source comparer. If this is <c>null</c>, the default comparer is used.</param>
        /// <param name="specialNullHandling">A value indicating whether <c>null</c> values are passed to <paramref name="selector"/>. If <c>false</c>, then <c>null</c> values are considered less than any non-<c>null</c> values and are not passed to <paramref name="selector"/>. This value is ignored if <typeparamref name="T"/> is a non-nullable type.</param>
        public SelectEqualityComparer(Func<T, TSource> selector, IEqualityComparer<TSource>? source, bool specialNullHandling)
            : base(source, specialNullHandling)
        {
            _selector = selector;
        }

        /// <inheritdoc />
        protected override int DoGetHashCode(T obj) => Source.GetHashCode(_selector(obj));

        /// <inheritdoc />
        protected override bool DoEquals(T x, T y) => Source.Equals(_selector(x), _selector(y));

        /// <summary>
        /// Returns a short, human-readable description of the comparer. This is intended for debugging and not for other purposes.
        /// </summary>
        public override string ToString() => $"Select<{typeof(TSource).Name}>({Source})";
    }
}
