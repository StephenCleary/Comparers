using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Nito.Comparers.Util
{
    /// <summary>
    /// Provides helper methods for comparer implementations.
    /// </summary>
    public static class ComparerHelpers
    {
        /// <summary>
        /// Attempts to return a hash code for the specified object, using the specified comparer. If the comparer does not support hash codes, this method will throw an exception.
        /// </summary>
        /// <typeparam name="T">The type of objects being compared.</typeparam>
        /// <param name="comparer">The comparer to use to calculate a hash code. May not be <c>null</c>.</param>
        /// <param name="obj">The object for which to return a hash code. May be <c>null</c>.</param>
        /// <returns>A hash code for the specified object.</returns>
        public static int GetHashCodeFromComparer<T>(IComparer<T> comparer, T obj)
        {
            var equalityComparer = comparer as IEqualityComparer<T>;
            if (equalityComparer != null)
                return equalityComparer.GetHashCode(obj);
            var objectEqualityComparer = comparer as System.Collections.IEqualityComparer;
            if (objectEqualityComparer != null)
                return objectEqualityComparer.GetHashCode(obj);

            throw new NotImplementedException();
        }

        /// <summary>
        /// Converts a <c>null</c> or default comparer into a default comparer that supports hash codes (and sequences, if necessary).
        /// </summary>
        /// <typeparam name="T">The type of objects being compared.</typeparam>
        /// <param name="comparer">The comparer. May be <c>null</c>.</param>
        /// <returns>A default comparer or <paramref name="comparer"/>.</returns>
        public static IComparer<T> NormalizeDefault<T>(IComparer<T> comparer)
        {
            if (comparer != null && comparer != Comparer<T>.Default)
                return comparer;

            if (DefaultComparer<T>.IsImplementedByType || !DefaultComparer<T>.IsImplemented)
                return DefaultComparer<T>.Instance;

            // If T doesn't implement a default comparer but DefaultComparer does, then T must implement IEnumerable<U>.
            // Extract the U and create a SequenceComparer<U>.
            var enumerable = ReflectionHelpers.TryGetEnumeratorType(typeof(T));
            var elementTypes = enumerable.GenericTypeArguments;
            var genericSequenceComparerType = typeof(SequenceComparer<>);
            var sequenceComparerType = genericSequenceComparerType.MakeGenericType(elementTypes);
            var genericComparerType = typeof(IComparer<>);
            var comparerType = genericComparerType.MakeGenericType(elementTypes);
            foreach (var constructor in sequenceComparerType.GetTypeInfo().DeclaredConstructors)
            {
                var instance = constructor.Invoke(new object[] { null });
                return (IComparer<T>)instance;
            }

            // Should never get here.
            return null;
        }
    }
}
