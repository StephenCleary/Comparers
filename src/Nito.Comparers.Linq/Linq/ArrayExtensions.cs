using System;
using System.Collections.Generic;
using System.Text;

namespace Nito.Comparers.Linq
{
    /// <summary>
    /// Extension methods for <c>T[]</c>.
    /// </summary>
    public static class ArrayExtensions
    {
        /// <summary>
        /// Searches the sorted array for an element and returns the index of that element.
        /// </summary>
        /// <typeparam name="T">The type of the elements.</typeparam>
        /// <param name="this">The array to search.</param>
        /// <param name="item">The item to locate.</param>
        /// <param name="comparerFactory">The definition of a comparer to compare elements.</param>
        /// <returns>The index of the item if it is found; otherwise, the bitwise complement of the index of (where the element would be, plus one).</returns>
        public static int BinarySearch<T>(this T[] @this, T item, Func<ComparerBuilderFor<T>, IFullComparer<T>> comparerFactory)
        {
            var comparer = comparerFactory(ComparerBuilder.For<T>());
            return Array.BinarySearch(@this, item, comparer);
        }

        /// <summary>
        /// Searches part of a sorted array for an element and returns the index of that element.
        /// </summary>
        /// <typeparam name="T">The type of the elements.</typeparam>
        /// <param name="this">The array to search.</param>
        /// <param name="index">The offset at which to start searching.</param>
        /// <param name="count">The number of elements to search.</param>
        /// <param name="item">The item to locate.</param>
        /// <param name="comparerFactory">The definition of a comparer to compare elements.</param>
        /// <returns>The index of the item if it is found; otherwise, the bitwise complement of the index of (where the element would be, plus one).</returns>
        public static int BinarySearch<T>(this T[] @this, int index, int count, T item, Func<ComparerBuilderFor<T>, IFullComparer<T>> comparerFactory)
        {
            var comparer = comparerFactory(ComparerBuilder.For<T>());
            return Array.BinarySearch(@this, index, count, item, comparer);
        }

        /// <summary>
        /// Sorts the array.
        /// </summary>
        /// <typeparam name="T">The type of the elements.</typeparam>
        /// <param name="this">The array to sort.</param>
        /// <param name="comparerFactory">The definition of a comparer to compare elements.</param>
        public static void Sort<T>(this T[] @this, Func<ComparerBuilderFor<T>, IFullComparer<T>> comparerFactory)
        {
            var comparer = comparerFactory(ComparerBuilder.For<T>());
            Array.Sort(@this, comparer);
        }

        /// <summary>
        /// Sorts part of an array.
        /// </summary>
        /// <typeparam name="T">The type of the elements.</typeparam>
        /// <param name="this">The array to sort.</param>
        /// <param name="index">The offset at which to start sorting.</param>
        /// <param name="count">The number of elements to sort.</param>
        /// <param name="comparerFactory">The definition of a comparer to compare elements.</param>
        public static void Sort<T>(this T[] @this, int index, int count, Func<ComparerBuilderFor<T>, IFullComparer<T>> comparerFactory)
        {
            var comparer = comparerFactory(ComparerBuilder.For<T>());
            Array.Sort(@this, index, count, comparer);
        }

#if !NETSTANDARD1_0
        /// <summary>
        /// Sorts a pair of arrays. This array contains the keys; the first parameter to this method contains the corresponding items.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys.</typeparam>
        /// <typeparam name="TValue">The type of the items.</typeparam>
        /// <param name="this">The array containing the keys.</param>
        /// <param name="items">The array containing the items.</param>
        /// <param name="comparerFactory">The definition of a comparer to compare elements.</param>
        public static void Sort<TKey, TValue>(this TKey[] @this, TValue[] items, Func<ComparerBuilderFor<TKey>, IFullComparer<TKey>> comparerFactory)
        {
            var comparer = comparerFactory(ComparerBuilder.For<TKey>());
            Array.Sort(@this, items, comparer);
        }

        /// <summary>
        /// Sorts part of a pair of arrays. This array contains the keys; the first parameter to this method contains the corresponding items.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys.</typeparam>
        /// <typeparam name="TValue">The type of the items.</typeparam>
        /// <param name="this">The array containing the keys.</param>
        /// <param name="items">The array containing the items.</param>
        /// <param name="index">The offset at which to start sorting.</param>
        /// <param name="length">The number of elements to sort.</param>
        /// <param name="comparerFactory">The definition of a comparer to compare elements.</param>
        public static void Sort<TKey, TValue>(this TKey[] @this, TValue[] items, int index, int length, Func<ComparerBuilderFor<TKey>, IFullComparer<TKey>> comparerFactory)
        {
            var comparer = comparerFactory(ComparerBuilder.For<TKey>());
            Array.Sort(@this, items, index, length, comparer);
        }
#endif
    }
}
