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
