using System;
using System.Collections.Generic;
using System.ComponentModel;
using Nito.Comparers.Util;

#pragma warning disable IDE0079
#pragma warning disable IDE0060

namespace Nito.Comparers
{
    /// <summary>
    /// Provides sources for equality comparers.
    /// </summary>
    /// <typeparam name="T">The type of objects being compared.</typeparam>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public sealed class EqualityComparerBuilderFor<T>
    {
        /// <summary>
        /// Gets a static instance of an equality comparer builder.
        /// </summary>
        public static EqualityComparerBuilderFor<T> Instance { get; } = new EqualityComparerBuilderFor<T>();

        private EqualityComparerBuilderFor() { }
    }

    /// <summary>
    /// Provides sources for equality comparers.
    /// </summary>
    public static class EqualityComparerBuilderForExtensions
    {
        /// <summary>
        /// Gets the null equality comparer for this type, which evaluates all objects as equivalent.
        /// </summary>
        public static IFullEqualityComparer<T> Null<T>(this EqualityComparerBuilderFor<T> @this) => NullComparer<T>.Instance;

        /// <summary>
        /// Gets the default equality comparer for this type.
        /// </summary>
        public static IFullEqualityComparer<T> Default<T>(this EqualityComparerBuilderFor<T> @this) => (IFullEqualityComparer<T>)EqualityComparerHelpers.NormalizeDefault<T>(null);

        /// <summary>
        /// Gets the reference equality comparer for this type.
        /// </summary>
        public static IFullEqualityComparer<T> Reference<T>(this EqualityComparerBuilderFor<T> @this)
            where T : class
        { 
            return ReferenceEqualityComparer<T>.Instance;
        }

        /// <summary>
        /// Creates a key comparer.
        /// </summary>
        /// <typeparam name="T">The type of object being compared.</typeparam>
        /// <typeparam name="TKey">The type of key objects being compared.</typeparam>
        /// <param name="this">The equality comparer builder.</param>
        /// <param name="selector">The key selector. May not be <c>null</c>.</param>
        /// <param name="comparerFactory">The definition of the key comparer. May not be <c>null</c>.</param>
        /// <param name="specialNullHandling">A value indicating whether <c>null</c> values are passed to <paramref name="selector"/>. If <c>false</c>, then <c>null</c> values are considered less than any non-<c>null</c> values and are not passed to <paramref name="selector"/>. This value is ignored if <typeparamref name="T"/> is a non-nullable type.</param>
        /// <returns>A key comparer.</returns>
        public static IFullEqualityComparer<T> EquateBy<T, TKey>(this EqualityComparerBuilderFor<T> @this, Func<T?, TKey?> selector, Func<EqualityComparerBuilderFor<TKey>, IEqualityComparer<TKey>> comparerFactory, bool specialNullHandling = false)
        {
            _ = comparerFactory ?? throw new ArgumentNullException(nameof(comparerFactory));
            var comparer = comparerFactory(EqualityComparerBuilder.For<TKey>());
            return @this.EquateBy(selector, comparer, specialNullHandling);
        }

        /// <summary>
        /// Creates a key comparer.
        /// </summary>
        /// <typeparam name="T">The type of object being compared.</typeparam>
        /// <typeparam name="TKey">The type of key objects being compared.</typeparam>
        /// <param name="this">The equality comparer builder.</param>
        /// <param name="selector">The key selector. May not be <c>null</c>.</param>
        /// <param name="keyComparer">The key comparer. Defaults to <c>null</c>. If this is <c>null</c>, the default comparer is used.</param>
        /// <param name="specialNullHandling">A value indicating whether <c>null</c> values are passed to <paramref name="selector"/>. If <c>false</c>, then <c>null</c> values are considered less than any non-<c>null</c> values and are not passed to <paramref name="selector"/>. This value is ignored if <typeparamref name="T"/> is a non-nullable type.</param>
        /// <returns>A key comparer.</returns>
        public static IFullEqualityComparer<T> EquateBy<T, TKey>(this EqualityComparerBuilderFor<T> @this, Func<T?, TKey?> selector, IEqualityComparer<TKey>? keyComparer = null, bool specialNullHandling = false)
        {
            return new SelectEqualityComparer<T, TKey>(selector, keyComparer, specialNullHandling);
        }
    }
}