using System;
using System.Collections.Generic;
using System.Text;

namespace Nito.Comparers.Fixes
{
    /// <summary>
    /// Provides extension methods for comparers.
    /// </summary>
    public static class FixComparerExtensions
    {
        /// <summary>
        /// Wraps this comparer with one that handles <c>null</c> values in the standard way.
        /// </summary>
        /// <typeparam name="T">The type of objects being compared.</typeparam>
        /// <param name="source">The source comparer. If this is <c>null</c>, the default comparer is used.</param>
        public static IFullComparer<T> WithStandardNullHandling<T>(this IComparer<T>? source) =>
            new StandardNullHandlingComparer<T>(source);

        /// <summary>
        /// Wraps this comparer with one that provides a <c>GetHashCode</c> implementation.
        /// </summary>
        /// <typeparam name="T">The type of objects being compared.</typeparam>
        /// <param name="source">The source comparer. If this is <c>null</c>, the default comparer is used.</param>
        /// <param name="getHashCode">The <c>GetHashCode</c> implementation.</param>
        public static IFullComparer<T> WithGetHashCode<T>(this IComparer<T>? source, Func<T, int> getHashCode) =>
            new ExplicitGetHashCodeComparer<T>(source, getHashCode);

        /// <summary>
        /// Wraps this comparer with one that provides a <c>GetHashCode</c> implementation that throws <see cref="NotImplementedException"/>.
        /// This is only a workaround. The best solution is to provide a proper <c>GetHashCode</c> implementation rather than using this method.
        /// </summary>
        /// <typeparam name="T">The type of objects being compared.</typeparam>
        /// <param name="source">The source comparer. If this is <c>null</c>, the default comparer is used.</param>
        public static IFullComparer<T> WithGetHashCodeThrow<T>(this IComparer<T>? source) =>
            source.WithGetHashCode(_ => throw new NotImplementedException());

        /// <summary>
        /// Wraps this comparer with one that provides a <c>GetHashCode</c> implementation that returns a constant value.
        /// This is only a workaround. The best solution is to provide a proper <c>GetHashCode</c> implementation rather than using this method.
        /// </summary>
        /// <typeparam name="T">The type of objects being compared.</typeparam>
        /// <param name="source">The source comparer. If this is <c>null</c>, the default comparer is used.</param>
        public static IFullComparer<T> WithGetHashCodeConstant<T>(this IComparer<T>? source) =>
            source.WithGetHashCode(_ => 0);
    }
}
