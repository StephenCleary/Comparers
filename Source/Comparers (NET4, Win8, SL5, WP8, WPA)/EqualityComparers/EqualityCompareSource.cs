using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using Comparers;
using EqualityComparers.Util;

namespace EqualityComparers
{
    /// <summary>
    /// Provides sources for equality comparers.
    /// </summary>
    /// <typeparam name="T">The type of objects being compared.</typeparam>
    public sealed class EqualityCompareSource<T>
    {
        /// <summary>
        /// Gets the null equality comparer for this type, which evaluates all objects as equivalent.
        /// </summary>
        public IEqualityComparer<T> Null()
        {
            Contract.Ensures(Contract.Result<IEqualityComparer<T>>() != null);
            return EqualityCompare<T>.Null();
        }

        /// <summary>
        /// Gets the default equality comparer for this type.
        /// </summary>
        public IEqualityComparer<T> Default()
        {
            Contract.Ensures(Contract.Result<IEqualityComparer<T>>() != null);
            return EqualityCompare<T>.Default();
        }

        /// <summary>
        /// Gets the reference equality comparer for this type.
        /// </summary>
        public IEqualityComparer<T> Reference()
        {
            Contract.Ensures(Contract.Result<IEqualityComparer<T>>() != null);
            return EqualityCompare<T>.Reference();
        }

        /// <summary>
        /// Creates a key comparer.
        /// </summary>
        /// <typeparam name="TKey">The type of key objects being compared.</typeparam>
        /// <param name="selector">The key selector. May not be <c>null</c>.</param>
        /// <param name="keyComparer">The key comparer. Defaults to <c>null</c>. If this is <c>null</c>, the default comparer is used.</param>
        /// <param name="allowNulls">A value indicating whether <c>null</c> values are passed to <paramref name="selector"/>. If <c>false</c>, then <c>null</c> values are considered less than any non-<c>null</c> values and are not passed to <paramref name="selector"/>.</param>
        /// <returns>A key comparer.</returns>
        public IEqualityComparer<T> EquateBy<TKey>(Func<T, TKey> selector, IEqualityComparer<TKey> keyComparer = null, bool allowNulls = false)
        {
            Contract.Requires(selector != null);
            Contract.Ensures(Contract.Result<IEqualityComparer<T>>() != null);
            return EqualityCompare<T>.EquateBy(selector, keyComparer, allowNulls);
        }
    }

    /// <summary>
    /// Provides sources for equality comparers, inferring the type being compared.
    /// </summary>
    public static class EqualityCompareSource
    {
        /// <summary>
        /// Creates a source for an equality comparer of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of objects being compared.</typeparam>
        public static EqualityCompareSource<T> For<T>()
        {
            return new EqualityCompareSource<T>();
        }

        /// <summary>
        /// Creates a source for an equality comparer of type <typeparamref name="T"/>. <paramref name="instance"/> is only used to infer the type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of objects being compared.</typeparam>
        /// <param name="instance">An instance of the type of objects being compared.</param>
        public static EqualityCompareSource<T> For<T>(T instance)
        {
            return new EqualityCompareSource<T>();
        }
    }
}