using System;
using System.Collections.Generic;
using Nito.Comparers.Util;

namespace Nito.Comparers
{
    /// <summary>
    /// Provides extension methods for equality comparers.
    /// </summary>
    public static class EqualityComparerExtensions
    {
        /// <summary>
        /// Returns an equality comparer that uses another comparer if the source comparer determines the objects are equal.
        /// </summary>
        /// <typeparam name="T">The type of objects being compared.</typeparam>
        /// <param name="source">The source comparer. If this is <c>null</c>, the default comparer is used.</param>
        /// <param name="thenBy">The comparer that is used if <paramref name="source"/> determines the objects are equal. If this is <c>null</c>, the default comparer is used.</param>
        /// <returns>A comparer that uses another comparer if the source comparer determines the objects are equal.</returns>
        public static IFullEqualityComparer<T> ThenEquateBy<T>(this IEqualityComparer<T>? source, IEqualityComparer<T>? thenBy) =>
            new CompoundEqualityComparer<T>(source, thenBy);

        /// <summary>
        /// Returns an equality comparer that uses a key comparer if the source comparer determines the objects are equal.
        /// </summary>
        /// <typeparam name="T">The type of objects being compared.</typeparam>
        /// <typeparam name="TKey">The type of key objects being compared.</typeparam>
        /// <param name="source">The source comparer. If this is <c>null</c>, the default comparer is used.</param>
        /// <param name="selector">The key selector. May not be <c>null</c>.</param>
        /// <param name="comparerFactory">The definition of the key comparer. May not be <c>null</c>.</param>
        /// <param name="specialNullHandling">A value indicating whether <c>null</c> values are passed to <paramref name="selector"/>. If <c>false</c>, then <c>null</c> values are considered less than any non-<c>null</c> values and are not passed to <paramref name="selector"/>. This value is ignored if <typeparamref name="T"/> is a non-nullable type.</param>
        /// <returns>A comparer that uses a key comparer if the source comparer determines the objects are equal.</returns>
        public static IFullEqualityComparer<T> ThenEquateBy<T, TKey>(this IEqualityComparer<T>? source, Func<T, TKey> selector, Func<EqualityComparerBuilderFor<TKey>, IEqualityComparer<TKey>> comparerFactory, bool specialNullHandling = false)
        {
            _ = comparerFactory ?? throw new ArgumentNullException(nameof(comparerFactory));
            var comparer = comparerFactory(EqualityComparerBuilder.For<TKey>());
            return source.ThenEquateBy(selector, comparer, specialNullHandling);
        }

        /// <summary>
        /// Returns an equality comparer that uses a key comparer if the source comparer determines the objects are equal.
        /// </summary>
        /// <typeparam name="T">The type of objects being compared.</typeparam>
        /// <typeparam name="TKey">The type of key objects being compared.</typeparam>
        /// <param name="source">The source comparer. If this is <c>null</c>, the default comparer is used.</param>
        /// <param name="selector">The key selector. May not be <c>null</c>.</param>
        /// <param name="keyComparer">The key comparer. Defaults to <c>null</c>. If this is <c>null</c>, the default comparer is used.</param>
        /// <param name="specialNullHandling">A value indicating whether <c>null</c> values are passed to <paramref name="selector"/>. If <c>false</c>, then <c>null</c> values are considered less than any non-<c>null</c> values and are not passed to <paramref name="selector"/>. This value is ignored if <typeparamref name="T"/> is a non-nullable type.</param>
        /// <returns>A comparer that uses a key comparer if the source comparer determines the objects are equal.</returns>
        public static IFullEqualityComparer<T> ThenEquateBy<T, TKey>(this IEqualityComparer<T>? source, Func<T, TKey> selector, IEqualityComparer<TKey>? keyComparer = null, bool specialNullHandling = false)
        {
            var selectComparer = new SelectEqualityComparer<T, TKey>(selector, keyComparer, specialNullHandling);
            return source.ThenEquateBy(selectComparer);
        }

        /// <summary>
        /// Returns an equality comparer that will perform a lexicographical equality on a sequence of items.
        /// This is the same as calling <see cref="EquateSequence{T}(System.Collections.Generic.IEqualityComparer{T}?,bool)"/> with <c>ignoreOrder</c> set to <c>false</c>.
        /// </summary>
        /// <typeparam name="T">The type of sequence elements being compared.</typeparam>
        /// <param name="source">The source comparer. If this is <c>null</c>, the default comparer is used.</param>
        /// <returns>A comparer that will perform a lexicographical equality on a sequence of items.</returns>
        public static IFullEqualityComparer<IEnumerable<T>> EquateSequence<T>(this IEqualityComparer<T>? source) => source.EquateSequence(ignoreOrder: false);

        /// <summary>
        /// Returns an equality comparer that will perform a lexicographical or unordered equality on a sequence of items.
        /// </summary>
        /// <typeparam name="T">The type of sequence elements being compared.</typeparam>
        /// <param name="source">The source comparer. If this is <c>null</c>, the default comparer is used.</param>
        /// <param name="ignoreOrder">Whether the sequences will be equated ignoring order.</param>
        /// <returns>A comparer that will perform an equality on a sequence of items.</returns>
        public static IFullEqualityComparer<IEnumerable<T>> EquateSequence<T>(this IEqualityComparer<T>? source, bool ignoreOrder) =>
            ignoreOrder ? new UnorderedSequenceEqualityComparer<T>(source) : new SequenceEqualityComparer<T>(source);
    }
}
