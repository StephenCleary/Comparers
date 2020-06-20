using System;
using System.Collections.Generic;
using System.ComponentModel;
using Nito.Comparers.Util;

namespace Nito.Comparers
{
    /// <summary>
    /// Provides sources for comparers.
    /// </summary>
    /// <typeparam name="T">The type of objects being compared.</typeparam>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public sealed class ComparerBuilderFor<T>
    {
        /// <summary>
        /// Gets a static instance of a comparer builder.
        /// </summary>
        public static ComparerBuilderFor<T> Instance { get; } = new ComparerBuilderFor<T>();

        private ComparerBuilderFor() { }
    }

    /// <summary>
    /// Provides sources for comparers.
    /// </summary>
    public static class ComparerBuilderForExtensions
    {
        /// <summary>
        /// Gets the null comparer for this type, which evaluates all objects as equivalent.
        /// </summary>
        public static IFullComparer<T> Null<T>(this ComparerBuilderFor<T> @this) => NullComparer<T>.Instance;

        /// <summary>
        /// Gets the default comparer for this type.
        /// </summary>
        public static IFullComparer<T> Default<T>(this ComparerBuilderFor<T> @this) => (IFullComparer<T>)ComparerHelpers.NormalizeDefault<T>(null);

        /// <summary>
        /// Creates a key comparer.
        /// </summary>
        /// <typeparam name="T">The type of object being compared.</typeparam>
        /// <typeparam name="TKey">The type of key objects being compared.</typeparam>
        /// <param name="this">The comparer builder.</param>
        /// <param name="selector">The key selector. May not be <c>null</c>.</param>
        /// <param name="comparerFactory">The definition of the key comparer. May not be <c>null</c>.</param>
        /// <param name="specialNullHandling">A value indicating whether <c>null</c> values are passed to <paramref name="selector"/>. If <c>false</c>, then <c>null</c> values are considered less than any non-<c>null</c> values and are not passed to <paramref name="selector"/>. This value is ignored if <typeparamref name="T"/> is a non-nullable type.</param>
        /// <param name="descending">A value indicating whether the sorting is done in descending order. If <c>false</c> (the default), then the sort is in ascending order.</param>
        /// <returns>A key comparer.</returns>
        public static IFullComparer<T> OrderBy<T, TKey>(this ComparerBuilderFor<T> @this, Func<T, TKey> selector, Func<ComparerBuilderFor<TKey>, IComparer<TKey>> comparerFactory, bool specialNullHandling = false, bool descending = false)
        {
            _ = comparerFactory ?? throw new ArgumentNullException(nameof(comparerFactory));
            var comparer = comparerFactory(ComparerBuilder.For<TKey>());
            return @this.OrderBy(selector, comparer, specialNullHandling, descending);
        }

        /// <summary>
        /// Creates a key comparer.
        /// </summary>
        /// <typeparam name="T">The type of object being compared.</typeparam>
        /// <typeparam name="TKey">The type of key objects being compared.</typeparam>
        /// <param name="this">The comparer builder.</param>
        /// <param name="selector">The key selector. May not be <c>null</c>.</param>
        /// <param name="keyComparer">The key comparer. Defaults to <c>null</c>. If this is <c>null</c>, the default comparer is used.</param>
        /// <param name="specialNullHandling">A value indicating whether <c>null</c> values are passed to <paramref name="selector"/>. If <c>false</c>, then <c>null</c> values are considered less than any non-<c>null</c> values and are not passed to <paramref name="selector"/>. This value is ignored if <typeparamref name="T"/> is a non-nullable type.</param>
        /// <param name="descending">A value indicating whether the sorting is done in descending order. If <c>false</c> (the default), then the sort is in ascending order.</param>
        /// <returns>A key comparer.</returns>
        public static IFullComparer<T> OrderBy<T, TKey>(this ComparerBuilderFor<T> @this, Func<T, TKey> selector, IComparer<TKey>? keyComparer = null, bool specialNullHandling = false, bool descending = false)
        {
            var selectComparer = new SelectComparer<T, TKey>(selector, keyComparer, specialNullHandling);
            return descending ? selectComparer.Reverse() : selectComparer;
        }
    }
}