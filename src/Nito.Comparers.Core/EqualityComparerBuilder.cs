using System;
using System.Collections.Generic;
using Nito.Comparers.Util;

namespace Nito.Comparers
{
    /// <summary>
    /// Provides sources for equality comparers, inferring the type being compared.
    /// </summary>
    public static class EqualityComparerBuilder
    {
        /// <summary>
        /// Creates a source for an equality comparer of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of objects being compared.</typeparam>
        public static EqualityComparerBuilderFor<T> For<T>() => EqualityComparerBuilderFor<T>.Instance;

        /// <summary>
        /// Creates a source for a comparer of type <typeparamref name="T"/>. <paramref name="expression"/> is only used to infer the type <typeparamref name="T"/>; it is not evaluated.
        /// </summary>
        /// <typeparam name="T">The type of objects being compared.</typeparam>
        /// <param name="expression">An expression of the type of objects being compared. The expression is only used to infer the type <typeparamref name="T"/>; it is not evaluated.</param>
        public static EqualityComparerBuilderFor<T> For<T>(Func<T> expression) => EqualityComparerBuilderFor<T>.Instance;

        /// <summary>
        /// Creates a source for a comparer of type <typeparamref name="T"/>. <paramref name="expression"/> is only used to infer the type <typeparamref name="T"/>; it is not evaluated.
        /// </summary>
        /// <typeparam name="T">The type of objects being compared.</typeparam>
        /// <param name="expression">An expression whose results are a sequence of objects being compared. The expression is only used to infer the type <typeparamref name="T"/>; it is not evaluated.</param>
        public static EqualityComparerBuilderFor<T> ForElementsOf<T>(Func<IEnumerable<T>> expression) => EqualityComparerBuilderFor<T>.Instance;

        /// <summary>
        /// Creates a source for a comparer of type <typeparamref name="T"/>. <paramref name="sequence"/> is only used to infer the type <typeparamref name="T"/>; it is not enumerated.
        /// </summary>
        /// <typeparam name="T">The type of objects being compared.</typeparam>
        /// <param name="sequence">A sequence of objects being compared. This argument is only used to infer the type <typeparamref name="T"/>; it is not enumerated.</param>
        public static EqualityComparerBuilderFor<T> ForElementsOf<T>(IEnumerable<T> sequence) => EqualityComparerBuilderFor<T>.Instance;
    }
}