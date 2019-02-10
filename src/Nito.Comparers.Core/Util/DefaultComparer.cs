using System.Collections.Generic;

namespace Nito.Comparers.Util
{
    /// <summary>
    /// The default comparer.
    /// </summary>
    /// <typeparam name="T">The type of objects being compared.</typeparam>
    internal sealed class DefaultComparer<T> : ComparerBase<T>, IEqualityComparer<T>, System.Collections.IEqualityComparer
    {
        private DefaultComparer()
            : base(true)
        {
        }

        static DefaultComparer()
        {
            // This type constructor does nothing; it only exists to make static field initialization deterministic.
        }

        /// <inheritdoc />
        protected override int DoGetHashCode(T obj)
        {
            return EqualityComparer<T>.Default.GetHashCode(obj);
        }

        /// <inheritdoc />
        protected override bool DoEquals(T x, T y)
        {
            return EqualityComparer<T>.Default.Equals(x, y);
        }

        /// <inheritdoc />
        protected override int DoCompare(T x, T y)
        {
            return Comparer<T>.Default.Compare(x, y);
        }

        private static readonly DefaultComparer<T> instance = new DefaultComparer<T>();

        /// <summary>
        /// Gets the default comparer for this type.
        /// </summary>
        public static DefaultComparer<T> Instance
        {
            get
            {
                return instance;
            }
        }

        /// <summary>
        /// Returns a short, human-readable description of the comparer. This is intended for debugging and not for other purposes.
        /// </summary>
        public override string ToString()
        {
            var typeofT = typeof(T);
            string comparableBaseString = null;
            try
            {
                var comparableBaseGeneric = typeof(ComparableBase<>);
                var comparableBase = comparableBaseGeneric.MakeGenericType(typeofT);
                var property = ReflectionHelpers.TryFindDeclaredProperty(comparableBase, "DefaultComparer");
                var value = property.GetValue(null, null);
                comparableBaseString = value.ToString();
            }
            catch
            {
            }

            if (comparableBaseString != null)
                return "Default(" + comparableBaseString + ")";
            if (ReflectionHelpers.TryFindInterfaceType(typeofT, "IComparable`1") != null)
                return "Default(" + typeofT.Name + ": IComparable<T>)";
            if (ReflectionHelpers.TryFindInterfaceType(typeofT, "IComparable") != null)
                return "Default(" + typeofT.Name + ": IComparable)";
            return "Default(" + typeofT.Name + ": undefined)";
        }

        /// <summary>
        /// Gets a value indicating whether a default comparer is implemented by the compared type.
        /// </summary>
        public static bool IsImplementedByType
        {
            get
            {
                var typeofT = typeof(T);
                if (ReflectionHelpers.TryFindInterfaceType(typeofT, "IComparable`1") != null)
                    return true;
                if (ReflectionHelpers.TryFindInterfaceType(typeofT, "IComparable") != null)
                    return true;
                return false;
            }
        }

        /// <summary>
        /// Gets a value indicating whether a default equality comparer is implemented by the compared type.
        /// </summary>
        public static bool IsEqualityComparerImplementedByType
        {
            get
            {
                var typeofT = typeof(T);
                if (ReflectionHelpers.TryFindInterfaceType(typeofT, "IEquatable`1") != null)
                    return true;
                return false;
            }
        }

        /// <summary>
        /// Gets a value indicating whether a default comparer is implemented for this type.
        /// </summary>
        public static bool IsImplemented
        {
            get
            {
                if (IsImplementedByType)
                    return true;
                var enumerable = ReflectionHelpers.TryGetEnumeratorType(typeof(T));
                if (enumerable == null)
                    return false;
                var defaultComparerGenericType = typeof(DefaultComparer<>);
                var defaultComparerType = defaultComparerGenericType.MakeGenericType(enumerable.GenericTypeArguments);
                var property = ReflectionHelpers.TryFindDeclaredProperty(defaultComparerType, "IsImplemented");
                var value = property.GetValue(null, null);
                return (bool)value;
            }
        }
    }
}
