using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Nito.Comparers.Util;

namespace Nito.Comparers
{
    /// <summary>
    /// Provides implementations for equality and hash code methods. These implementations assume that there will only be one derived type that defines equality.
    /// </summary>
    /// <typeparam name="T">The type of objects being compared.</typeparam>
    public abstract class EquatableBase<T> : IEquatable<T> where T : EquatableBase<T>
    {
        static EquatableBase() => RuntimeHelpers.RunClassConstructor(typeof(T).TypeHandle);

        /// <summary>
        /// Gets the default comparer for this type.
        /// </summary>
        public static IFullEqualityComparer<T> DefaultComparer { get; protected set; } = null!;

        /// <summary>
        /// Gets the hash code for this instance.
        /// </summary>
        /// <returns>The hash code for this instance.</returns>
        public override int GetHashCode() => ComparableImplementations.ImplementGetHashCode(DefaultComparer, (T)this);

        /// <summary>
        /// Returns a value indicating whether this instance is equal to the specified object.
        /// </summary>
        /// <param name="obj">The object to compare with this instance.</param>
        /// <returns>A value indicating whether this instance is equal to the specified object.</returns>
        public override bool Equals(object? obj) => ComparableImplementations.ImplementEquals(DefaultComparer, (T)this, obj);

        /// <summary>
        /// Returns a value indicating whether this instance is equal to the specified object.
        /// </summary>
        /// <param name="other">The object to compare with this instance. May be <c>null</c>.</param>
        /// <returns>A value indicating whether this instance is equal to the specified object.</returns>
        public bool Equals(T other) => ComparableImplementations.ImplementEquals(DefaultComparer, (T)this, other!);
    }
}
