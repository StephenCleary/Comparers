using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using EqualityComparers;

namespace Comparers.Linq
{
    /// <summary>
    /// Extension methods for observable sequences.
    /// </summary>
    public static class ObservableExtensions
    {
        /// <summary>
        /// Returns the maximum value in an observable sequence according to the specified comparer.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">An observable sequence to determine the maximum element of.</param>
        /// <param name="comparerFactory">The definition of a comparer to compare elements.</param>
        public static IObservable<TSource> Max<TSource>(this IObservable<TSource> source, Func<CompareSource<TSource>, IFullComparer<TSource>> comparerFactory)
        {
            var comparer = comparerFactory(CompareSource.For<TSource>());
            return source.Max(comparer);
        }

        /// <summary>
        /// Invokes a transform function on each element of a sequence and returns the maximum value according to the specified comparer.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TResult">The type of the objects derived from the elements in the source sequence to determine the maximum of.</typeparam>
        /// <param name="source">An observable sequence to determine the mimimum element of.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <param name="comparerFactory">The definition of a comparer to compare elements.</param>
        public static IObservable<TResult> Max<TSource, TResult>(this IObservable<TSource> source, Func<TSource, TResult> selector, Func<CompareSource<TResult>, IFullComparer<TResult>> comparerFactory)
        {
            var comparer = comparerFactory(CompareSource.For<TResult>());
            return source.Max(selector, comparer);
        }

        /// <summary>
        /// Returns the elements in an observable sequence with the maximum key value according to the specified comparer.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TKey">The type of the key computed for each element in the source sequence.</typeparam>
        /// <param name="source">An observable sequence to get the maximum elements for.</param>
        /// <param name="keySelector">Key selector function.</param>
        /// <param name="comparerFactory">The definition of a comparer to compare keys.</param>
        public static IObservable<IList<TSource>> MaxBy<TSource, TKey>(this IObservable<TSource> source, Func<TSource, TKey> keySelector, Func<CompareSource<TKey>, IFullComparer<TKey>> comparerFactory)
        {
            var comparer = comparerFactory(CompareSource.For<TKey>());
            return source.MaxBy(keySelector, comparer);
        }

        /// <summary>
        /// Returns the minimum element in an observable sequence according to the specified comparer.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">An observable sequence to determine the mimimum element of.</param>
        /// <param name="comparerFactory">The definition of a comparer to compare elements.</param>
        public static IObservable<TSource> Min<TSource>(this IObservable<TSource> source, Func<CompareSource<TSource>, IFullComparer<TSource>> comparerFactory)
        {
            var comparer = comparerFactory(CompareSource.For<TSource>());
            return source.Min(comparer);
        }

        /// <summary>
        /// Invokes a transform function on each element of a sequence and returns the minimum value according to the specified comparer.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TResult">The type of the objects derived from the elements in the source sequence to determine the minimum of.</typeparam>
        /// <param name="source">An observable sequence to determine the mimimum element of.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <param name="comparerFactory">The definition of a comparer to compare elements.</param>
        public static IObservable<TResult> Min<TSource, TResult>(this IObservable<TSource> source, Func<TSource, TResult> selector, Func<CompareSource<TResult>, IFullComparer<TResult>> comparerFactory)
        {
            var comparer = comparerFactory(CompareSource.For<TResult>());
            return source.Min(selector, comparer);
        }

        /// <summary>
        /// Returns the elements in an observable sequence with the minimum key value according to the specified comparer.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TKey">The type of the key computed for each element in the source sequence.</typeparam>
        /// <param name="source">An observable sequence to get the minimum elements for.</param>
        /// <param name="keySelector">Key selector function.</param>
        /// <param name="comparerFactory">The definition of a comparer to compare keys.</param>
        public static IObservable<IList<TSource>> MinBy<TSource, TKey>(this IObservable<TSource> source, Func<TSource, TKey> keySelector, Func<CompareSource<TKey>, IFullComparer<TKey>> comparerFactory)
        {
            var comparer = comparerFactory(CompareSource.For<TKey>());
            return source.MinBy(keySelector, comparer);
        }

        /// <summary>
        /// Determines whether an observable sequence contains a specified element by using a specified equality comparer.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">An observable sequence in which to locate a value.</param>
        /// <param name="value">The value to locate in the source sequence.</param>
        /// <param name="comparerFactory">The definition of a comparer to compare elements.</param>
        public static IObservable<bool> Contains<TSource>(this IObservable<TSource> source, TSource value, Func<EqualityCompareSource<TSource>, IEqualityComparer<TSource>> comparerFactory)
        {
            var comparer = comparerFactory(EqualityCompareSource.For<TSource>());
            return source.Contains(value, comparer);
        }

        /// <summary>
        /// Returns an observable sequence that contains only distinct elements according to the comparer.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">An observable sequence to retain distinct elements for.</param>
        /// <param name="comparerFactory">The definition of a comparer to compare elements.</param>
        public static IObservable<TSource> Distinct<TSource>(this IObservable<TSource> source, Func<EqualityCompareSource<TSource>, IEqualityComparer<TSource>> comparerFactory)
        {
            var comparer = comparerFactory(EqualityCompareSource.For<TSource>());
            return source.Distinct(comparer);
        }

        /// <summary>
        /// Returns an observable sequence that contains only distinct elements according to the keySelector and the comparer.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TKey">The type of the discriminator key computed for each element in the source sequence.</typeparam>
        /// <param name="source">An observable sequence to retain distinct elements for.</param>
        /// <param name="keySelector">A function to compute the comparison key for each element.</param>
        /// <param name="comparerFactory">The definition of a comparer to compare keys.</param>
        public static IObservable<TSource> Distinct<TSource, TKey>(this IObservable<TSource> source, Func<TSource, TKey> keySelector, Func<EqualityCompareSource<TKey>, IEqualityComparer<TKey>> comparerFactory)
        {
            var comparer = comparerFactory(EqualityCompareSource.For<TKey>());
            return source.Distinct(keySelector, comparer);
        }

        /// <summary>
        /// Returns an observable sequence that contains only distinct contiguous elements according to the comparer.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">An observable sequence to retain distinct contiguous elements for.</param>
        /// <param name="comparerFactory">The definition of a comparer to compare elements.</param>
        public static IObservable<TSource> DistinctUntilChanged<TSource>(this IObservable<TSource> source, Func<EqualityCompareSource<TSource>, IEqualityComparer<TSource>> comparerFactory)
        {
            var comparer = comparerFactory(EqualityCompareSource.For<TSource>());
            return source.DistinctUntilChanged(comparer);
        }

        /// <summary>
        /// Returns an observable sequence that contains only distinct contiguous elements according to the keySelector and the comparer.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TKey">The type of the discriminator key computed for each element in the source sequence.</typeparam>
        /// <param name="source">An observable sequence to retain distinct contiguous elements for, based on a computed key value.</param>
        /// <param name="keySelector">A function to compute the comparison key for each element.</param>
        /// <param name="comparerFactory">The definition of a comparer to compare keys.</param>
        public static IObservable<TSource> DistinctUntilChanged<TSource, TKey>(this IObservable<TSource> source, Func<TSource, TKey> keySelector, Func<EqualityCompareSource<TKey>, IEqualityComparer<TKey>> comparerFactory)
        {
            var comparer = comparerFactory(EqualityCompareSource.For<TKey>());
            return source.DistinctUntilChanged(keySelector, comparer);
        }

        /// <summary>
        /// Groups the elements of an observable sequence according to a specified key selector function and comparer.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TKey">The type of the grouping key computed for each element in the source sequence.</typeparam>
        /// <param name="source">An observable sequence whose elements to group.</param>
        /// <param name="keySelector">A function to extract the key for each element.</param>
        /// <param name="comparerFactory">The definition of a comparer to compare keys.</param>
        public static IObservable<IGroupedObservable<TKey, TSource>> GroupBy<TSource, TKey>(this IObservable<TSource> source, Func<TSource, TKey> keySelector, Func<EqualityCompareSource<TKey>, IEqualityComparer<TKey>> comparerFactory)
        {
            var comparer = comparerFactory(EqualityCompareSource.For<TKey>());
            return source.GroupBy(keySelector, comparer);
        }

        /// <summary>
        /// Groups the elements of an observable sequence according to a specified key selector function and comparer and selects the resulting elements by using a specified function.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TKey">The type of the grouping key computed for each element in the source sequence.</typeparam>
        /// <typeparam name="TElement">The type of the elements within the groups computed for each element in the source sequence.</typeparam>
        /// <param name="source">An observable sequence whose elements to group.</param>
        /// <param name="keySelector">A function to extract the key for each element.</param>
        /// <param name="elementSelector">A function to map each source element to an element in an observable group.</param>
        /// <param name="comparerFactory">The definition of a comparer to compare keys.</param>
        public static IObservable<IGroupedObservable<TKey, TElement>> GroupBy<TSource, TKey, TElement>(this IObservable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, Func<EqualityCompareSource<TKey>, IEqualityComparer<TKey>> comparerFactory)
        {
            var comparer = comparerFactory(EqualityCompareSource.For<TKey>());
            return source.GroupBy(keySelector, elementSelector, comparer);
        }

        /// <summary>
        /// Groups the elements of an observable sequence with the specified initial capacity according to a specified key selector function and comparer.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TKey">The type of the grouping key computed for each element in the source sequence.</typeparam>
        /// <param name="source">An observable sequence whose elements to group.</param>
        /// <param name="keySelector">A function to extract the key for each element.</param>
        /// <param name="capacity">The initial number of elements that the underlying dictionary can contain.</param>
        /// <param name="comparerFactory">The definition of a comparer to compare keys.</param>
        public static IObservable<IGroupedObservable<TKey, TSource>> GroupBy<TSource, TKey>(this IObservable<TSource> source, Func<TSource, TKey> keySelector, int capacity, Func<EqualityCompareSource<TKey>, IEqualityComparer<TKey>> comparerFactory)
        {
            var comparer = comparerFactory(EqualityCompareSource.For<TKey>());
            return source.GroupBy(keySelector, capacity, comparer);
        }

        /// <summary>
        /// Groups the elements of an observable sequence with the specified initial capacity according to a specified key selector function and comparer and selects the resulting elements by using a specified function.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TKey">The type of the grouping key computed for each element in the source sequence.</typeparam>
        /// <typeparam name="TElement">The type of the elements within the groups computed for each element in the source sequence.</typeparam>
        /// <param name="source">An observable sequence whose elements to group.</param>
        /// <param name="keySelector">A function to extract the key for each element.</param>
        /// <param name="elementSelector">A function to map each source element to an element in an observable group.</param>
        /// <param name="capacity">The initial number of elements that the underlying dictionary can contain.</param>
        /// <param name="comparerFactory">The definition of a comparer to compare keys.</param>
        public static IObservable<IGroupedObservable<TKey, TElement>> GroupBy<TSource, TKey, TElement>(this IObservable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, int capacity, Func<EqualityCompareSource<TKey>, IEqualityComparer<TKey>> comparerFactory)
        {
            var comparer = comparerFactory(EqualityCompareSource.For<TKey>());
            return source.GroupBy(keySelector, elementSelector, capacity, comparer);
        }

        /// <summary>
        /// Groups the elements of an observable sequence according to a specified key selector function and comparer and selects the resulting elements by using a specified function.
        /// A duration selector function is used to control the lifetime of groups. When a group expires, it receives an OnCompleted notification. When a new element with the same
        /// key value as a reclaimed group occurs, the group will be reborn with a new lifetime request.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TKey">The type of the grouping key computed for each element in the source sequence.</typeparam>
        /// <typeparam name="TElement">The type of the elements within the groups computed for each element in the source sequence.</typeparam>
        /// <typeparam name="TDuration">The type of the elements in the duration sequences obtained for each group to denote its lifetime.</typeparam>
        /// <param name="source">An observable sequence whose elements to group.</param>
        /// <param name="keySelector">A function to extract the key for each element.</param>
        /// <param name="elementSelector">A function to map each source element to an element in an observable group.</param>
        /// <param name="durationSelector">A function to signal the expiration of a group.</param>
        /// <param name="comparerFactory">The definition of a comparer to compare keys.</param>
        public static IObservable<IGroupedObservable<TKey, TElement>> GroupByUntil<TSource, TKey, TElement, TDuration>(this IObservable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, Func<IGroupedObservable<TKey, TElement>, IObservable<TDuration>> durationSelector, Func<EqualityCompareSource<TKey>, IEqualityComparer<TKey>> comparerFactory)
        {
            var comparer = comparerFactory(EqualityCompareSource.For<TKey>());
            return source.GroupByUntil(keySelector, elementSelector, durationSelector, comparer);
        }

        /// <summary>
        /// Groups the elements of an observable sequence according to a specified key selector function and comparer.
        /// A duration selector function is used to control the lifetime of groups. When a group expires, it receives an OnCompleted notification. When a new element with the same
        /// key value as a reclaimed group occurs, the group will be reborn with a new lifetime request.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TKey">The type of the grouping key computed for each element in the source sequence.</typeparam>
        /// <typeparam name="TDuration">The type of the elements in the duration sequences obtained for each group to denote its lifetime.</typeparam>
        /// <param name="source">An observable sequence whose elements to group.</param>
        /// <param name="keySelector">A function to extract the key for each element.</param>
        /// <param name="durationSelector">A function to signal the expiration of a group.</param>
        /// <param name="comparerFactory">The definition of a comparer to compare keys.</param>
        public static IObservable<IGroupedObservable<TKey, TSource>> GroupByUntil<TSource, TKey, TDuration>(this IObservable<TSource> source, Func<TSource, TKey> keySelector, Func<IGroupedObservable<TKey, TSource>, IObservable<TDuration>> durationSelector, Func<EqualityCompareSource<TKey>, IEqualityComparer<TKey>> comparerFactory)
        {
            var comparer = comparerFactory(EqualityCompareSource.For<TKey>());
            return source.GroupByUntil(keySelector, durationSelector, comparer);
        }

        /// <summary>
        /// Groups the elements of an observable sequence with the specified initial capacity according to a specified key selector function and comparer and selects the resulting elements by using a specified function.
        /// A duration selector function is used to control the lifetime of groups. When a group expires, it receives an OnCompleted notification. When a new element with the same
        /// key value as a reclaimed group occurs, the group will be reborn with a new lifetime request.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TKey">The type of the grouping key computed for each element in the source sequence.</typeparam>
        /// <typeparam name="TElement">The type of the elements within the groups computed for each element in the source sequence.</typeparam>
        /// <typeparam name="TDuration">The type of the elements in the duration sequences obtained for each group to denote its lifetime.</typeparam>
        /// <param name="source">An observable sequence whose elements to group.</param>
        /// <param name="keySelector">A function to extract the key for each element.</param>
        /// <param name="elementSelector">A function to map each source element to an element in an observable group.</param>
        /// <param name="durationSelector">A function to signal the expiration of a group.</param>
        /// <param name="capacity">The initial number of elements that the underlying dictionary can contain.</param>
        /// <param name="comparerFactory">The definition of a comparer to compare keys.</param>
        public static IObservable<IGroupedObservable<TKey, TElement>> GroupByUntil<TSource, TKey, TElement, TDuration>(this IObservable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, Func<IGroupedObservable<TKey, TElement>, IObservable<TDuration>> durationSelector, int capacity, Func<EqualityCompareSource<TKey>, IEqualityComparer<TKey>> comparerFactory)
        {
            var comparer = comparerFactory(EqualityCompareSource.For<TKey>());
            return source.GroupByUntil(keySelector, elementSelector, durationSelector, capacity, comparer);
        }

        /// <summary>
        /// Groups the elements of an observable sequence with the specified initial capacity according to a specified key selector function and comparer.
        /// A duration selector function is used to control the lifetime of groups. When a group expires, it receives an OnCompleted notification. When a new element with the same
        /// key value as a reclaimed group occurs, the group will be reborn with a new lifetime request.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TKey">The type of the grouping key computed for each element in the source sequence.</typeparam>
        /// <typeparam name="TDuration">The type of the elements in the duration sequences obtained for each group to denote its lifetime.</typeparam>
        /// <param name="source">An observable sequence whose elements to group.</param>
        /// <param name="keySelector">A function to extract the key for each element.</param>
        /// <param name="durationSelector">A function to signal the expiration of a group.</param>
        /// <param name="capacity">The initial number of elements that the underlying dictionary can contain.</param>
        /// <param name="comparerFactory">The definition of a comparer to compare keys.</param>
        public static IObservable<IGroupedObservable<TKey, TSource>> GroupByUntil<TSource, TKey, TDuration>(this IObservable<TSource> source, Func<TSource, TKey> keySelector, Func<IGroupedObservable<TKey, TSource>, IObservable<TDuration>> durationSelector, int capacity, Func<EqualityCompareSource<TKey>, IEqualityComparer<TKey>> comparerFactory)
        {
            var comparer = comparerFactory(EqualityCompareSource.For<TKey>());
            return source.GroupByUntil(keySelector, durationSelector, capacity, comparer);
        }

        /// <summary>
        /// Determines whether two sequences are equal by comparing the elements pairwise using a specified equality comparer.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="first">First observable sequence to compare.</param>
        /// <param name="second">Second observable sequence to compare.</param>
        /// <param name="comparerFactory">The definition of a comparer to compare elements.</param>
        public static IObservable<bool> SequenceEqual<TSource>(this IObservable<TSource> first, IObservable<TSource> second, Func<EqualityCompareSource<TSource>, IEqualityComparer<TSource>> comparerFactory)
        {
            var comparer = comparerFactory(EqualityCompareSource.For<TSource>());
            return first.SequenceEqual(second, comparer);
        }

        /// <summary>
        /// Determines whether an observable and enumerable sequence are equal by comparing the elements pairwise using a specified equality comparer.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="first">First observable sequence to compare.</param>
        /// <param name="second">Second observable sequence to compare.</param>
        /// <param name="comparerFactory">The definition of a comparer to compare elements.</param>
        public static IObservable<bool> SequenceEqual<TSource>(this IObservable<TSource> first, IEnumerable<TSource> second, Func<EqualityCompareSource<TSource>, IEqualityComparer<TSource>> comparerFactory)
        {
            var comparer = comparerFactory(EqualityCompareSource.For<TSource>());
            return first.SequenceEqual(second, comparer);
        }

        /// <summary>
        /// Creates a dictionary from an observable sequence according to a specified key selector function, and a comparer.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TKey">The type of the dictionary key computed for each element in the source sequence.</typeparam>
        /// <param name="source">An observable sequence to create a dictionary for.</param>
        /// <param name="keySelector">A function to extract a key from each element.</param>
        /// <param name="comparerFactory">The definition of a comparer to compare keys.</param>
        public static IObservable<IDictionary<TKey, TSource>> ToDictionary<TSource, TKey>(this IObservable<TSource> source, Func<TSource, TKey> keySelector, Func<EqualityCompareSource<TKey>, IEqualityComparer<TKey>> comparerFactory)
        {
            var comparer = comparerFactory(EqualityCompareSource.For<TKey>());
            return source.ToDictionary(keySelector, comparer);
        }

        /// <summary>
        /// Creates a dictionary from an observable sequence according to a specified key selector function, a comparer, and an element selector function.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TKey">The type of the dictionary key computed for each element in the source sequence.</typeparam>
        /// <typeparam name="TElement">The type of the dictionary value computed for each element in the source sequence.</typeparam>
        /// <param name="source">An observable sequence to create a dictionary for.</param>
        /// <param name="keySelector">A function to extract a key from each element.</param>
        /// <param name="elementSelector">A transform function to produce a result element value from each element.</param>
        /// <param name="comparerFactory">The definition of a comparer to compare keys.</param>
        public static IObservable<IDictionary<TKey, TElement>> ToDictionary<TSource, TKey, TElement>(this IObservable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, Func<EqualityCompareSource<TKey>, IEqualityComparer<TKey>> comparerFactory)
        {
            var comparer = comparerFactory(EqualityCompareSource.For<TKey>());
            return source.ToDictionary(keySelector, elementSelector, comparer);
        }

        /// <summary>
        /// Creates a lookup from an observable sequence according to a specified key selector function, and a comparer.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TKey">The type of the lookup key computed for each element in the source sequence.</typeparam>
        /// <param name="source">An observable sequence to create a lookup for.</param>
        /// <param name="keySelector">A function to extract a key from each element.</param>
        /// <param name="comparerFactory">The definition of a comparer to compare keys.</param>
        public static IObservable<ILookup<TKey, TSource>> ToLookup<TSource, TKey>(this IObservable<TSource> source, Func<TSource, TKey> keySelector, Func<EqualityCompareSource<TKey>, IEqualityComparer<TKey>> comparerFactory)
        {
            var comparer = comparerFactory(EqualityCompareSource.For<TKey>());
            return source.ToLookup(keySelector, comparer);
        }

        /// <summary>
        /// Creates a lookup from an observable sequence according to a specified key selector function, a comparer, and an element selector function.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TKey">The type of the lookup key computed for each element in the source sequence.</typeparam>
        /// <typeparam name="TElement">The type of the lookup value computed for each element in the source sequence.</typeparam>
        /// <param name="source">An observable sequence to create a lookup for.</param>
        /// <param name="keySelector">A function to extract a key from each element.</param>
        /// <param name="elementSelector">A transform function to produce a result element value from each element.</param>
        /// <param name="comparerFactory">The definition of a comparer to compare keys.</param>
        public static IObservable<ILookup<TKey, TElement>> ToLookup<TSource, TKey, TElement>(this IObservable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, Func<EqualityCompareSource<TKey>, IEqualityComparer<TKey>> comparerFactory)
        {
            var comparer = comparerFactory(EqualityCompareSource.For<TKey>());
            return source.ToLookup(keySelector, elementSelector, comparer);
        }
    }
}
