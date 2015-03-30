using System.Collections.Generic;
using System.Reflection;

namespace Nito.Comparers.Util
{
    /// <summary>
    /// Provides helper methods for comparer implementations.
    /// </summary>
    internal static class EqualityComparerHelpers
    {
        /// <summary>
        /// Converts a <c>null</c> or default equality comparer into a default comparer that supports sequences, if necessary.
        /// </summary>
        /// <typeparam name="T">The type of objects being compared.</typeparam>
        /// <param name="comparer">The comparer. May be <c>null</c>.</param>
        /// <returns>A default comparer or <paramref name="comparer"/>.</returns>
        public static IEqualityComparer<T> NormalizeDefault<T>(IEqualityComparer<T> comparer)
        {
            if (comparer != null && comparer != EqualityComparer<T>.Default)
                return comparer;

            if (DefaultComparer<T>.IsEqualityComparerImplementedByType)
                return DefaultComparer<T>.Instance;

            var enumerable = ReflectionHelpers.TryGetEnumeratorType(typeof(T));
            if (enumerable == null)
                return DefaultComparer<T>.Instance;

            // T implements IEnumerable<U>. Extract the U and create a SequenceEqualityComparer<U>.
            var elementTypes = enumerable.GenericTypeArguments;
            var genericSequenceComparerType = typeof(SequenceEqualityComparer<>);
            var sequenceComparerType = genericSequenceComparerType.MakeGenericType(elementTypes);
            var genericComparerType = typeof(IEqualityComparer<>);
            var comparerType = genericComparerType.MakeGenericType(elementTypes);
            foreach (var constructor in sequenceComparerType.GetTypeInfo().DeclaredConstructors)
            {
                var instance = constructor.Invoke(new object[] { null });
                return (IEqualityComparer<T>)instance;
            }

            // Should never get here.
            return null;
        }
    }
}
