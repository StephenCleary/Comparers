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
        /// Assertions for <see cref="object.Equals(object)"/>, <see cref="object.Equals(object,object)"/>, and <see cref="object.GetHashCode"/>.
        /// </summary>
        public static void AssertObjectEquals(object instance, object different, object equivalent)
        {
            AssertEquals(instance, different, equivalent, object.Equals, a => a.GetHashCode(),
                allowNullAsFirstArgumentForEquals: true, allowNullArgumentForGetHashCode: false);
            AssertEquals(instance, different, equivalent, (a, b) => a.Equals(b), a => a.GetHashCode(),
                allowNullAsFirstArgumentForEquals: false, allowNullArgumentForGetHashCode: false);
        }

        /// <summary>
        /// Assertions for <see cref="IEquatable{T}.Equals(T)"/> and <see cref="object.GetHashCode"/>.
        /// </summary>
        public static void AssertIEquatableTEquals<T>(T instance, T different, T equivalent)
            where T: IEquatable<T>
        {
            AssertEquals(instance, different, equivalent, (a, b) => a.Equals(b), a => a.GetHashCode(),
                allowNullAsFirstArgumentForEquals: false, allowNullArgumentForGetHashCode: false);
        }

        /// <summary>
        /// Assertions for <see cref="IEqualityComparer"/> and <see cref="IEqualityComparer{T}"/>.
        /// </summary>
        public static void AssertIFullEqualityComparerT<T>(IFullEqualityComparer<T> comparer, T instance, T different, T equivalent)
        {
            AssertIEqualityComparer(comparer, instance, different, equivalent);
            AssertIEqualityComparerT(comparer, instance, different, equivalent);
        }

        /// <summary>
        /// Assertions for <see cref="IEqualityComparer.Equals(object,object)"/> and <see cref="IEqualityComparer.GetHashCode(object)"/>.
        /// </summary>
        public static void AssertIEqualityComparer(IEqualityComparer comparer, object instance, object different, object equivalent)
        {
            AssertEquals(instance, different, equivalent, comparer.Equals, comparer.GetHashCode,
                allowNullAsFirstArgumentForEquals: true, allowNullArgumentForGetHashCode: true);
        }

        /// <summary>
        /// Assertions for <see cref="IEqualityComparer{T}.Equals(T,T)"/> and <see cref="IEqualityComparer{T}.GetHashCode(T)"/>.
        /// </summary>
        public static void AssertIEqualityComparerT<T>(IEqualityComparer<T> comparer, T instance, T different, T equivalent)
        {
            AssertEquals(instance, different, equivalent, comparer.Equals, comparer.GetHashCode,
                allowNullAsFirstArgumentForEquals: true, allowNullArgumentForGetHashCode: true);
        }

        /// <summary>
        /// Given example instances, verifies the equality and hash code implementations.
        /// </summary>
        /// <param name="a">An instance not equal to <paramref name="b"/> but equal to <paramref name="c"/>. May not be <c>null</c>.</param>
        /// <param name="b">An instance not equal to <paramref name="a"/>. May not be <c>null</c>.</param>
        /// <param name="c">An instance not equal to <paramref name="b"/> but equal to <paramref name="a"/>. May not be <c>null</c>.</param>
        /// <param name="equals">The equality implementation to test. The second parameter to this method must allow <c>null</c> values.</param>
        /// <param name="getHashCode">The hash code implementation to test.</param>
        /// <param name="allowNullAsFirstArgumentForEquals">Whether <paramref name="equals"/> may take <c>null</c> as its first argument.</param>
        /// <param name="allowNullArgumentForGetHashCode">Whether <paramref name="getHashCode"/> may take <c>null</c> as its argument.</param>
        public static void AssertEquals<T>(T a, T b, T c, Func<T, T, bool> equals, Func<T, int> getHashCode,
            bool allowNullAsFirstArgumentForEquals, bool allowNullArgumentForGetHashCode)
        {
            // Usage errors.
            Assert.NotNull(a);
            Assert.NotNull(b);
            Assert.NotNull(c);
            Assert.False(object.ReferenceEquals(a, c));

            // Identity
            Assert.True(equals(a, a));
            if (a.GetType() == b.GetType())
                Assert.True(equals(b, b));
            Assert.True(equals(c, c));
            if (allowNullAsFirstArgumentForEquals)
                Assert.True(equals(default(T), default(T)));

            // Inequality
            Assert.False(equals(a, b));
            Assert.False(equals(b, a));
            Assert.False(equals(b, c));
            Assert.False(equals(c, b));

            // Inequality with null
            Assert.False(equals(a, default(T)));
            if (allowNullAsFirstArgumentForEquals)
                Assert.False(equals(default(T), a));
            Assert.False(equals(b, default(T)));
            if (allowNullAsFirstArgumentForEquals)
                Assert.False(equals(default(T), b));
            Assert.False(equals(c, default(T)));
            if (allowNullAsFirstArgumentForEquals)
                Assert.False(equals(default(T), c));

            // Equality
            Assert.True(equals(a, c));
            Assert.True(equals(c, a));

            // GetHashCode
            Assert.Equal(getHashCode(a), getHashCode(c));
            if (allowNullArgumentForGetHashCode)
                getHashCode(default(T)); // assert does not throw
        }
    }
}
