using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Nito.Comparers;
using Xunit;

namespace UnitTests.Util
{
    public static class EqualityInvariantTests
    {
        /// <summary>
        /// Assertions for <see cref="object.Equals(object)"/> and <see cref="object.Equals(object,object)"/>.
        /// </summary>
        public static void AssertObjectEquals(object instance)
        {
            AssertEquals(instance, new object(), object.Equals, allowNullAsFirstArgument: true);
            AssertEquals(instance, new object(), (a, b) => a.Equals(b), allowNullAsFirstArgument: false);
        }

        /// <summary>
        /// Assertions for <see cref="IEquatable{T}.Equals(T)"/>.
        /// </summary>
        public static void AssertIEquatableTEquals<T>(T instance, T different)
            where T : class, IEquatable<T>
        {
            AssertEquals(instance, different, (a, b) => a.Equals(b), allowNullAsFirstArgument: false);
        }

        /// <summary>
        /// Assertions for <see cref="IEqualityComparer.Equals(object,object)"/> and <see cref="IEqualityComparer{T}.Equals(T,T)"/>.
        /// </summary>
        public static void AssertIFullEqualityComparerT<T>(IFullEqualityComparer<T> comparer, T instance, T different)
            where T : class
        {
            AssertIEqualityComparer(comparer, instance);
            AssertIEqualityComparerT(comparer, instance, different);
        }

        /// <summary>
        /// Assertions for <see cref="IEqualityComparer.Equals(object,object)"/>.
        /// </summary>
        public static void AssertIEqualityComparer(IEqualityComparer comparer, object instance) =>
            AssertEquals(instance, new object(), comparer.Equals, allowNullAsFirstArgument: true);

        /// <summary>
        /// Assertions for <see cref="IEqualityComparer{T}.Equals(T,T)"/>.
        /// </summary>
        public static void AssertIEqualityComparerT<T>(IEqualityComparer<T> comparer, T instance, T different)
            where T : class =>
            AssertEquals(instance, different, comparer.Equals, allowNullAsFirstArgument: true);

        /// <summary>
        /// Given two non-null, non-equal instances, verifies the equality implementation.
        /// </summary>
        /// <param name="a">An instance not equal to <paramref name="b"/>. May not be <c>null</c>.</param>
        /// <param name="b">An instance not equal to <paramref name="a"/>. May not be <c>null</c>.</param>
        /// <param name="equals">The equality implementation to test. The second parameter to this method must allow <c>null</c> values.</param>
        /// <param name="allowNullAsFirstArgument">Whether <paramref name="equals"/> may take <c>null</c> as its first parameter.</param>
        public static void AssertEquals<T>(T a, T b, Func<T, T, bool> equals, bool allowNullAsFirstArgument)
            where T : class
        {
            Assert.NotNull(a);
            Assert.NotNull(b);

            Assert.True(equals(a, a));
            Assert.True(equals(b, b));
            Assert.False(equals(a, null));
            if (allowNullAsFirstArgument)
                Assert.False(equals(null, a));
            Assert.False(equals(b, null));
            if (allowNullAsFirstArgument)
                Assert.False(equals(null, b));
            Assert.False(equals(a, b));
            Assert.False(equals(b, a));
            if (allowNullAsFirstArgument)
                Assert.True(equals(null, null));
        }
    }
}
