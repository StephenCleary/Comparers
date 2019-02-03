using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Nito.Comparers;
using Xunit;

namespace UnitTests.Util
{
    public static class ComparisonInvariantTests
    {
        /// <summary>
        /// Assertions for <see cref="object.Equals(object)"/> and <see cref="object.Equals(object,object)"/>.
        /// </summary>
        public static void AssertIComparableCompareTo<T>(T smallest, T middle, T largest)
            where T : class, IComparable
        {
            AssertCompare(smallest, middle, largest, (a, b) => a.CompareTo(b), allowNullAsFirstArgument: false);
            Assert.Throws<ArgumentException>(() => smallest.CompareTo(new object()));
            Assert.Throws<ArgumentException>(() => smallest.CompareTo("test"));
        }

        /// <summary>
        /// Assertions for <see cref="IComparable{T}.CompareTo(T)"/>.
        /// </summary>
        public static void AssertIComparableTCompareTo<T>(T smallest, T middle, T largest)
            where T : class, IComparable<T> =>
            AssertCompare(smallest, middle, largest, (a, b) => a.CompareTo(b), allowNullAsFirstArgument: false);

        /// <summary>
        /// Assertions for all compare and equality comparisons.
        /// </summary>
        public static void AssertIFullComparerT<T>(IFullComparer<T> comparer, T smallest, T middle, T largest, T largest2)
            where T : class
        {
            EqualityInvariantTests.AssertIFullEqualityComparerT(comparer, largest, middle, largest2);
            AssertIComparerT(comparer, smallest, middle, largest);
            AssertIComparer(comparer, smallest, middle, largest);
        }

        /// <summary>
        /// Assertions for <see cref="IComparer.Compare(object,object)"/>.
        /// </summary>
        public static void AssertIComparer(IComparer comparer, object smallest, object middle, object largest)
        {
            AssertCompare(smallest, middle, largest, comparer.Compare, allowNullAsFirstArgument: true);
            Assert.Throws<ArgumentException>(() => comparer.Compare(smallest, new object()));
            Assert.Throws<ArgumentException>(() => comparer.Compare(smallest, "test"));
        }

        /// <summary>
        /// Assertions for <see cref="IComparer{T}.Compare(T,T)"/>.
        /// </summary>
        public static void AssertIComparerT<T>(IComparer<T> comparer, T smallest, T middle, T largest)
            where T : class =>
            AssertCompare(smallest, middle, largest, comparer.Compare, allowNullAsFirstArgument: true);

        /// <summary>
        /// Given three non-null, totally ordered instances, verifies the comparer implementation.
        /// </summary>
        /// <param name="a">An instance less than <paramref name="b"/>. May not be <c>null</c>.</param>
        /// <param name="b">An instance greater than <paramref name="a"/> and less than <paramref name="c"/>. May not be <c>null</c>.</param>
        /// <param name="c">An instance greater than <paramref name="b"/>. May not be <c>null</c>.</param>
        /// <param name="compare">The comparison implementation to test. The second parameter to this method must allow <c>null</c> values.</param>
        /// <param name="allowNullAsFirstArgument">Whether <paramref name="compare"/> may take <c>null</c> as its first parameter.</param>
        public static void AssertCompare<T>(T a, T b, T c, Func<T, T, int> compare, bool allowNullAsFirstArgument)
            where T : class
        {
            Assert.NotNull(a);
            Assert.NotNull(b);
            Assert.NotNull(c);

            Assert.Equal(0, compare(a, a));
            Assert.Equal(0, compare(b, b));
            Assert.Equal(0, compare(c, c));
            if (allowNullAsFirstArgument)
                Assert.Equal(0, compare(null, null));

            Assert.True(compare(a, b) < 0);
            Assert.True(compare(b, a) > 0);
            Assert.True(compare(b, c) < 0);
            Assert.True(compare(c, b) > 0);
            Assert.True(compare(a, c) < 0);
            Assert.True(compare(c, a) > 0);

            // TODO: This may not be true for some specialized comparers
            Assert.True(compare(a, null) > 0);
            if (allowNullAsFirstArgument)
                Assert.True(compare(null, a) < 0);
            Assert.True(compare(b, null) > 0);
            if (allowNullAsFirstArgument)
                Assert.True(compare(null, b) < 0);
            Assert.True(compare(c, null) > 0);
            if (allowNullAsFirstArgument)
                Assert.True(compare(null, c) < 0);
        }
    }
}
