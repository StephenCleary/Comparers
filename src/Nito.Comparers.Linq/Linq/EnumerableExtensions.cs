using System;
using System.Collections.Generic;
using System.Linq;

namespace Nito.Comparers.Linq
{
    /// <summary>
    /// Extension methods for enumerable sequences.
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Sorts the elements of a sequence in ascending order by using a specified comparer.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TKey">The type of the key returned by <paramref name="keySelector"/>.</typeparam>
        /// <param name="source">A sequence of values to order.</param>
        /// <param name="keySelector">A function to extract a key from an element.</param>
        /// <param name="comparerFactory">The definition of a comparer to compare keys.</param>
        public static IOrderedEnumerable<T> OrderBy<T, TKey>(this IEnumerable<T> source, Func<T, TKey> keySelector, Func<ComparerBuilderFor<TKey>, IFullComparer<TKey>> comparerFactory)
        {
            var comparer = comparerFactory(ComparerBuilder.For<TKey>());
            return source.OrderBy(keySelector, comparer);
        }

        /// <summary>
        /// Sorts the elements of a sequence in descending order by using a specified comparer.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TKey">The type of the key returned by <paramref name="keySelector"/>.</typeparam>
        /// <param name="source">A sequence of values to order.</param>
        /// <param name="keySelector">A function to extract a key from an element.</param>
        /// <param name="comparerFactory">The definition of a comparer to compare keys.</param>
        public static IOrderedEnumerable<T> OrderByDescending<T, TKey>(this IEnumerable<T> source, Func<T, TKey> keySelector, Func<ComparerBuilderFor<TKey>, IFullComparer<TKey>> comparerFactory)
        {
            var comparer = comparerFactory(ComparerBuilder.For<TKey>());
            return source.OrderByDescending(keySelector, comparer);
        }

        /// <summary>
        /// Performs a subsequent ordering of the elements in a sequence in ascending order by using a specified comparer.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TKey">The type of the key returned by <paramref name="keySelector"/>.</typeparam>
        /// <param name="source">A sequence of values to order.</param>
        /// <param name="keySelector">A function to extract a key from an element.</param>
        /// <param name="comparerFactory">The definition of a comparer to compare keys.</param>
        public static IOrderedEnumerable<T> ThenBy<T, TKey>(this IOrderedEnumerable<T> source, Func<T, TKey> keySelector, Func<ComparerBuilderFor<TKey>, IFullComparer<TKey>> comparerFactory)
        {
            var comparer = comparerFactory(ComparerBuilder.For<TKey>());
            return source.ThenBy(keySelector, comparer);
        }

        /// <summary>
        /// Performs a subsequent ordering of the elements in a sequence in descending order by using a specified comparer.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TKey">The type of the key returned by <paramref name="keySelector"/>.</typeparam>
        /// <param name="source">A sequence of values to order.</param>
        /// <param name="keySelector">A function to extract a key from an element.</param>
        /// <param name="comparerFactory">The definition of a comparer to compare keys.</param>
        public static IOrderedEnumerable<T> ThenByDescending<T, TKey>(this IOrderedEnumerable<T> source, Func<T, TKey> keySelector, Func<ComparerBuilderFor<TKey>, IFullComparer<TKey>> comparerFactory)
        {
            var comparer = comparerFactory(ComparerBuilder.For<TKey>());
            return source.ThenByDescending(keySelector, comparer);
        }

        /// <summary>
        /// Determines whether a sequence contains a specified element by using a specified equality comparer.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">A sequence in which to locate a value.</param>
        /// <param name="value">The value to locate in the sequence.</param>
        /// <param name="comparerFactory">The definition of a comparer to compare values.</param>
        public static bool Contains<TSource>(this IEnumerable<TSource> source, TSource value, Func<EqualityComparerBuilderFor<TSource>, IEqualityComparer<TSource>> comparerFactory)
        {
            var comparer = comparerFactory(EqualityComparerBuilder.For<TSource>());
            return source.Contains(value, comparer);
        }

        /// <summary>
        /// Returns distinct elements from a sequence by using a specified equality comparer to compare values.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">The sequence to remove duplicate elements from.</param>
        /// <param name="comparerFactory">The definition of a comparer to compare values.</param>
        public static IEnumerable<TSource> Distinct<TSource>(this IEnumerable<TSource> source, Func<EqualityComparerBuilderFor<TSource>, IEqualityComparer<TSource>> comparerFactory)
        {
            var comparer = comparerFactory(EqualityComparerBuilder.For<TSource>());
            return source.Distinct(comparer);
        }

        /// <summary>
        /// Produces the set difference of two sequences by using the specified equality comparer to compare values.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of the sequences.</typeparam>
        /// <param name="first">A sequence whose elements that are not also in <paramref name="second"/> will be returned.</param>
        /// <param name="second">A sequence whose elements that also occur in the first sequence will cause those elements to be removed from the returned sequence.</param>
        /// <param name="comparerFactory">The definition of a comparer to compare values.</param>
        public static IEnumerable<TSource> Except<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second, Func<EqualityComparerBuilderFor<TSource>, IEqualityComparer<TSource>> comparerFactory)
        {
            var comparer = comparerFactory(EqualityComparerBuilder.For<TSource>());
            return first.Except(second, comparer);
        }

        /// <summary>
        /// Groups the elements of a sequence according to a specified key selector function and compares the keys by using a specified equality comparer.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of the sequence.</typeparam>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <param name="source">A sequence whose elements are grouped.</param>
        /// <param name="keySelector">A function to extract the key for each element.</param>
        /// <param name="comparerFactory">The definition of a comparer to compare keys.</param>
        public static IEnumerable<IGrouping<TKey, TSource>> GroupBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<EqualityComparerBuilderFor<TKey>, IEqualityComparer<TKey>> comparerFactory)
        {
            var comparer = comparerFactory(EqualityComparerBuilder.For<TKey>());
            return source.GroupBy(keySelector, comparer);
        }

        /// <summary>
        /// Groups the elements of a sequence according to a key selector function. The keys are compared by using a comparer and each group's elements are projected by using a specified function.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of the sequence.</typeparam>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TElement">The type of the elements in the grouping.</typeparam>
        /// <param name="source">A sequence whose elements are grouped.</param>
        /// <param name="keySelector">A function to extract the key for each element.</param>
        /// <param name="elementSelector">A function to map each source element to an element in the grouping.</param>
        /// <param name="comparerFactory">The definition of a comparer to compare keys.</param>
        public static IEnumerable<IGrouping<TKey, TElement>> GroupBy<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, Func<EqualityComparerBuilderFor<TKey>, IEqualityComparer<TKey>> comparerFactory)
        {
            var comparer = comparerFactory(EqualityComparerBuilder.For<TKey>());
            return source.GroupBy(keySelector, elementSelector, comparer);
        }

        /// <summary>
        /// Groups the elements of a sequence according to a specified key selector function and creates a result value from each group and its key. The keys are compared by using a specified comparer.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of the sequence.</typeparam>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TResult">The type of the result values.</typeparam>
        /// <param name="source">A sequence whose elements are grouped.</param>
        /// <param name="keySelector">A function to extract the key for each element.</param>
        /// <param name="resultSelector">A function to create a result value from each group.</param>
        /// <param name="comparerFactory">The definition of a comparer to compare keys.</param>
        public static IEnumerable<TResult> GroupBy<TSource, TKey, TResult>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TKey, IEnumerable<TSource>, TResult> resultSelector, Func<EqualityComparerBuilderFor<TKey>, IEqualityComparer<TKey>> comparerFactory)
        {
            var comparer = comparerFactory(EqualityComparerBuilder.For<TKey>());
            return source.GroupBy(keySelector, resultSelector, comparer);
        }

        /// <summary>
        /// Groups the elements of a sequence according to a specified key selector function and creates a result value from each group and its key. Key values are compared by using a specified comparer, and the elements of each group are projected by using a specified function.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of the sequence.</typeparam>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TElement">The type of the elements in the grouping.</typeparam>
        /// <typeparam name="TResult">The type of the result values.</typeparam>
        /// <param name="source">A sequence whose elements are grouped.</param>
        /// <param name="keySelector">A function to extract the key for each element.</param>
        /// <param name="elementSelector">A function to map each source element to an element in the grouping.</param>
        /// <param name="resultSelector">A function to create a result value from each group.</param>
        /// <param name="comparerFactory">The definition of a comparer to compare keys.</param>
        public static IEnumerable<TResult> GroupBy<TSource, TKey, TElement, TResult>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, Func<TKey, IEnumerable<TElement>, TResult> resultSelector, Func<EqualityComparerBuilderFor<TKey>, IEqualityComparer<TKey>> comparerFactory)
        {
            var comparer = comparerFactory(EqualityComparerBuilder.For<TKey>());
            return source.GroupBy(keySelector, elementSelector, resultSelector, comparer);
        }

        /// <summary>
        /// Correlates the elements of two sequences based on key equality and groups the results. A specified equality comparer is used to compare keys.
        /// </summary>
        /// <typeparam name="TOuter">The type of the elements of the first sequence.</typeparam>
        /// <typeparam name="TInner">The type of the elements of the second sequence.</typeparam>
        /// <typeparam name="TKey">The type of the keys returned by the key selector functions.</typeparam>
        /// <typeparam name="TResult">The type of the result elements.</typeparam>
        /// <param name="outer">The first sequence to join.</param>
        /// <param name="inner">The sequence to join to the first sequence.</param>
        /// <param name="outerKeySelector">A function to extract the join key from each element of the first sequence.</param>
        /// <param name="innerKeySelector">A function to extract the join key from each element of the second sequence.</param>
        /// <param name="resultSelector">A function to create a result element from an element from the first sequence and a collection of matching elements from the second sequence.</param>
        /// <param name="comparerFactory">The definition of a comparer to compare keys.</param>
        public static IEnumerable<TResult> GroupJoin<TOuter, TInner, TKey, TResult>(this IEnumerable<TOuter> outer, IEnumerable<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, IEnumerable<TInner>, TResult> resultSelector, Func<EqualityComparerBuilderFor<TKey>, IEqualityComparer<TKey>> comparerFactory)
        {
            var comparer = comparerFactory(EqualityComparerBuilder.For<TKey>());
            return outer.GroupJoin(inner, outerKeySelector, innerKeySelector, resultSelector, comparer);
        }

        /// <summary>
        /// Produces the set intersection of two sequences by using the specified equality comparer to compare values.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of the input sequences.</typeparam>
        /// <param name="first">A sequence whose distinct elements that also appear in <paramref name="second"/> will be returned.</param>
        /// <param name="second">A sequence whose distinct elements that also appear in the first sequence will be returned.</param>
        /// <param name="comparerFactory">The definition of a comparer to compare elements.</param>
        public static IEnumerable<TSource> Intersect<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second, Func<EqualityComparerBuilderFor<TSource>, IEqualityComparer<TSource>> comparerFactory)
        {
            var comparer = comparerFactory(EqualityComparerBuilder.For<TSource>());
            return first.Intersect(second, comparer);
        }

        /// <summary>
        /// Correlates the elements of two sequences based on matching keys. A specified equality comparer is used to compare keys.
        /// </summary>
        /// <typeparam name="TOuter">The type of the elements of the first sequence.</typeparam>
        /// <typeparam name="TInner">The type of the elements of the second sequence.</typeparam>
        /// <typeparam name="TKey">The type of the keys returned by the key selector functions.</typeparam>
        /// <typeparam name="TResult">The type of the result elements.</typeparam>
        /// <param name="outer">The first sequence to join.</param>
        /// <param name="inner">The sequence to join to the first sequence.</param>
        /// <param name="outerKeySelector">A function to extract the join key from each element of the first sequence.</param>
        /// <param name="innerKeySelector">A function to extract the join key from each element of the second sequence.</param>
        /// <param name="resultSelector">A function to create a result element from an element from the first sequence and a collection of matching elements from the second sequence.</param>
        /// <param name="comparerFactory">The definition of a comparer to compare keys.</param>
        public static IEnumerable<TResult> Join<TOuter, TInner, TKey, TResult>(this IEnumerable<TOuter> outer, IEnumerable<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, TInner, TResult> resultSelector, Func<EqualityComparerBuilderFor<TKey>, IEqualityComparer<TKey>> comparerFactory)
        {
            var comparer = comparerFactory(EqualityComparerBuilder.For<TKey>());
            return outer.Join(inner, outerKeySelector, innerKeySelector, resultSelector, comparer);
        }

        /// <summary>
        /// Determines whether two sequences are equal by comparing their elements by using a specified equality comparer.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of the input sequences.</typeparam>
        /// <param name="first">A sequence to compare to <paramref name="second"/>.</param>
        /// <param name="second">A sequence to compare to the first sequence.</param>
        /// <param name="comparerFactory">The definition of a comparer to compare elements.</param>
        public static bool SequenceEqual<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second, Func<EqualityComparerBuilderFor<TSource>, IEqualityComparer<TSource>> comparerFactory)
        {
            var comparer = comparerFactory(EqualityComparerBuilder.For<TSource>());
            return first.SequenceEqual(second, comparer);
        }

        /// <summary>
        /// Creates a dictionary from a sequence according to a specified key selector function and key comparer.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of the input sequence.</typeparam>
        /// <typeparam name="TKey">The type of the keys returned by the key selector function.</typeparam>
        /// <param name="source">A sequence to create a dictionary from.</param>
        /// <param name="keySelector">A function to extract the key for each element.</param>
        /// <param name="comparerFactory">The definition of a comparer to compare keys.</param>
        public static Dictionary<TKey, TSource> ToDictionary<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<EqualityComparerBuilderFor<TKey>, IEqualityComparer<TKey>> comparerFactory)
        {
            var comparer = comparerFactory(EqualityComparerBuilder.For<TKey>());
            return source.ToDictionary(keySelector, comparer);
        }

        /// <summary>
        /// Creates a dictionary from a sequence according to a specified key selector function, a comparer, and an element selector function.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of the input sequence.</typeparam>
        /// <typeparam name="TKey">The type of the keys returned by the key selector function.</typeparam>
        /// <typeparam name="TElement">The type of the elements returned by the element selector function.</typeparam>
        /// <param name="source">A sequence to create a dictionary from.</param>
        /// <param name="keySelector">A function to extract the key for each element.</param>
        /// <param name="elementSelector">A transform function to produce a result element value from each element.</param>
        /// <param name="comparerFactory">The definition of a comparer to compare keys.</param>
        public static Dictionary<TKey, TElement> ToDictionary<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, Func<EqualityComparerBuilderFor<TKey>, IEqualityComparer<TKey>> comparerFactory)
        {
            var comparer = comparerFactory(EqualityComparerBuilder.For<TKey>());
            return source.ToDictionary(keySelector, elementSelector, comparer);
        }

        /// <summary>
        /// Creates a lookup from a sequence according to a specified key selector function and key comparer.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of the input sequence.</typeparam>
        /// <typeparam name="TKey">The type of the keys returned by the key selector function.</typeparam>
        /// <param name="source">A sequence to create a lookup from.</param>
        /// <param name="keySelector">A function to extract the key for each element.</param>
        /// <param name="comparerFactory">The definition of a comparer to compare keys.</param>
        public static ILookup<TKey, TSource> ToLookup<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<EqualityComparerBuilderFor<TKey>, IEqualityComparer<TKey>> comparerFactory)
        {
            var comparer = comparerFactory(EqualityComparerBuilder.For<TKey>());
            return source.ToLookup(keySelector, comparer);
        }

        /// <summary>
        /// Creates a lookup from a sequence according to a specified key selector function, a comparer, and an element selector function.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of the input sequence.</typeparam>
        /// <typeparam name="TKey">The type of the keys returned by the key selector function.</typeparam>
        /// <typeparam name="TElement">The type of the elements returned by the element selector function.</typeparam>
        /// <param name="source">A sequence to create a lookup from.</param>
        /// <param name="keySelector">A function to extract the key for each element.</param>
        /// <param name="elementSelector">A transform function to produce a result element value from each element.</param>
        /// <param name="comparerFactory">The definition of a comparer to compare keys.</param>
        public static ILookup<TKey, TElement> ToLookup<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, Func<EqualityComparerBuilderFor<TKey>, IEqualityComparer<TKey>> comparerFactory)
        {
            var comparer = comparerFactory(EqualityComparerBuilder.For<TKey>());
            return source.ToLookup(keySelector, elementSelector, comparer);
        }

        /// <summary>
        /// Produces the set union of two sequences by using a specified equality comparer.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of the input sequences.</typeparam>
        /// <param name="first">A sequence whose distinct elements form the first set for the union.</param>
        /// <param name="second">A sequence whose distinct elements form the second set for the union.</param>
        /// <param name="comparerFactory">The definition of a comparer to compare elements.</param>
        public static IEnumerable<TSource> Union<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second, Func<EqualityComparerBuilderFor<TSource>, IEqualityComparer<TSource>> comparerFactory)
        {
            var comparer = comparerFactory(EqualityComparerBuilder.For<TSource>());
            return first.Union(second, comparer);
        }
    }
}
