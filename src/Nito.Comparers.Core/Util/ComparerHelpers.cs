using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Nito.Comparers.Util
{
    /// <summary>
    /// Provides helper methods for comparer implementations.
    /// </summary>
    internal static class ComparerHelpers
    {
        /// <summary>
        /// Determines how to compute hash codes using the specified comparer. If the comparer does not support hash codes, this method will throw an exception.
        /// </summary>
        /// <typeparam name="T">The type of objects being compared.</typeparam>
        /// <param name="comparer">The comparer to use to calculate a hash code. May be <c>null</c>, but this method will throw an exception since <c>null</c> does not support hash codes.</param>
        public static Func<T?, int> ComparerGetHashCode<T>(IComparer<T>? comparer)
        {
            if (comparer is IEqualityComparer<T> equalityComparer)
#pragma warning disable CS8622 // Nullability of reference types in type of parameter doesn't match the target delegate (possibly because of nullability attributes).
	            return equalityComparer.GetHashCode;
#pragma warning restore CS8622 // Nullability of reference types in type of parameter doesn't match the target delegate (possibly because of nullability attributes).
            if (comparer is IEqualityComparer objectEqualityComparer)
                return obj => objectEqualityComparer.GetHashCode(obj!);

            throw new InvalidOperationException("Comparer does not implement IEqualityComparer or IEqualityComparer<T>.");
        }

        /// <summary>
        /// Converts a <c>null</c> or default comparer into a default comparer that supports hash codes (and sequences, if necessary).
        /// </summary>
        /// <typeparam name="T">The type of objects being compared.</typeparam>
        /// <param name="comparer">The comparer. May be <c>null</c>.</param>
        /// <returns>A default comparer or <paramref name="comparer"/>.</returns>
        public static IComparer<T> NormalizeDefault<T>(IComparer<T>? comparer)
        {
            if (comparer != null && comparer != Comparer<T>.Default)
                return comparer;

            if (DefaultComparer<T>.IsImplementedByType || !DefaultComparer<T>.IsImplemented)
                return DefaultComparer<T>.Instance;

            // If T doesn't implement a default comparer but DefaultComparer does, then T must implement IEnumerable<U>.
            // Extract the U and create a SequenceComparer<U>.
            var enumerable = ReflectionHelpers.TryGetEnumeratorType(typeof(T))!;
            var elementTypes = enumerable.GenericTypeArguments;
            var genericSequenceComparerType = typeof(SequenceComparer<>);
            var sequenceComparerType = genericSequenceComparerType.MakeGenericType(elementTypes);
            var constructor = sequenceComparerType.GetTypeInfo().DeclaredConstructors.First();
            var instance = constructor.Invoke(new object[] { null! });
            return (IComparer<T>)instance;
        }
    }
}
