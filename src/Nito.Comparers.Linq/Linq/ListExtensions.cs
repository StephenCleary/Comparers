using System;
using System.Collections.Generic;
using System.Text;

namespace Nito.Comparers.Linq
{
    /// <summary>
    /// Extension methods for <see cref="List{T}"/>.
    /// </summary>
    public static class ListExtensions
    {
        /// <summary>
        /// Searches the sorted list for an element and returns the index of that element.
        /// </summary>
        /// <typeparam name="T">The type of the elements.</typeparam>
        /// <param name="this">The list to search.</param>
        /// <param name="item">The item to locate.</param>
        /// <param name="comparerFactory">The definition of a comparer to compare elements.</param>
        /// <returns>The index of the item if it is found; otherwise, the bitwise complement of the index of (where the element would be, plus one).</returns>
        public static int BinarySearch<T>(this List<T> @this, T item, Func<ComparerBuilderFor<T>, IFullComparer<T>> comparerFactory)
        {
            var comparer = comparerFactory(ComparerBuilder.For<T>());
            return @this.BinarySearch(item, comparer);
        }

        /// <summary>
        /// Searches a portion of the sorted list for an element and returns the index of that element.
        /// </summary>
        /// <typeparam name="T">The type of the elements.</typeparam>
        /// <param name="this">The list to search.</param>
        /// <param name="index">The offset at which to start searching.</param>
        /// <param name="count">The number of elements to search.</param>
        /// <param name="item">The item to locate.</param>
        /// <param name="comparerFactory">The definition of a comparer to compare elements.</param>
        /// <returns>The index of the item if it is found; otherwise, the bitwise complement of the index of (where the element would be, plus one).</returns>
        public static int BinarySearch<T>(this List<T> @this, int index, int count, T item, Func<ComparerBuilderFor<T>, IFullComparer<T>> comparerFactory)
        {
            var comparer = comparerFactory(ComparerBuilder.For<T>());
            return @this.BinarySearch(index, count, item, comparer);
        }

        /// <summary>
        /// Sorts the list.
        /// </summary>
        /// <typeparam name="T">The type of the elements.</typeparam>
        /// <param name="this">The list to sort.</param>
        /// <param name="comparerFactory">The definition of a comparer to compare elements.</param>
        public static void Sort<T>(this List<T> @this, Func<ComparerBuilderFor<T>, IFullComparer<T>> comparerFactory)
        {
            var comparer = comparerFactory(ComparerBuilder.For<T>());
            @this.Sort(comparer);
        }

        /// <summary>
        /// Sorts part of a list.
        /// </summary>
        /// <typeparam name="T">The type of the elements.</typeparam>
        /// <param name="this">The list to sort.</param>
        /// <param name="index">The offset at which to start sorting.</param>
        /// <param name="count">The number of elements to sort.</param>
        /// <param name="comparerFactory">The definition of a comparer to compare elements.</param>
        public static void Sort<T>(this List<T> @this, int index, int count, Func<ComparerBuilderFor<T>, IFullComparer<T>> comparerFactory)
        {
            var comparer = comparerFactory(ComparerBuilder.For<T>());
            @this.Sort(index, count, comparer);
        }
    }
}
