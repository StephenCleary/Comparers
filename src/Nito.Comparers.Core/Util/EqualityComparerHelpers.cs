using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
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
        public static IEqualityComparer<T> NormalizeDefault<T>(IEqualityComparer<T>? comparer)
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
            var constructor = sequenceComparerType.GetTypeInfo().DeclaredConstructors.First();
            var instance = constructor.Invoke(new object[] { null! });
            return (IEqualityComparer<T>)instance;
        }

        public static bool ImplementEquals<T>(object? x, object? y, bool specialNullHandling, Func<T, T, bool> doEquals)
        {
            // EqualityComparer<T>.IEqualityComparer.Equals will throw in this situation, but int.Equals returns false.
            var xValid = x is T || x == null;
            var yValid = y is T || y == null;
            if (!xValid || !yValid)
            {
                if (!xValid && !yValid)
                    throw new ArgumentException("Invalid types for equality comparison.");
                return false;
            }

            if (!specialNullHandling)
            {
                if (x == null || y == null)
                    return (x == null && y == null);
            }

            return doEquals((T) x!, (T) y!);
        }

        public static bool ImplementEquals<T>([AllowNull] T x, [AllowNull] T y, bool specialNullHandling, Func<T, T, bool> doEquals)
        {
            if (!specialNullHandling)
            {
                if (x == null || y == null)
                    return (x == null && y == null);
            }

            return doEquals(x!, y!);
        }

        public static int ImplementGetHashCode<T>(object? obj, bool specialNullHandling, Func<T, int> doGetHashCode)
        {
            if (!specialNullHandling)
            {
                if (obj == null)
                    return 0;
            }

            var objValid = obj is T || obj == null;
            if (!objValid)
                throw new ArgumentException("Invalid type for comparison.");

            return doGetHashCode((T) obj!);
        }

        public static int ImplementGetHashCode<T>([AllowNull] T obj, bool specialNullHandling, Func<T, int> doGetHashCode)
        {
            if (!specialNullHandling)
            {
                if (obj == null)
                    return 0;
            }

            return doGetHashCode(obj!);
        }
    }
}
