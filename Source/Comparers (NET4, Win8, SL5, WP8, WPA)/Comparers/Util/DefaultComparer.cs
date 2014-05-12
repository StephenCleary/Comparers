using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using System.Reflection;
using System.Diagnostics.CodeAnalysis;

namespace Comparers.Util
{
    /// <summary>
    /// The default comparer.
    /// </summary>
    /// <typeparam name="T">The type of objects being compared.</typeparam>
    public sealed class DefaultComparer<T> : ComparerBase<T>, IEqualityComparer<T>, System.Collections.IEqualityComparer
    {
        private DefaultComparer()
            : base(true)
        {
        }

        static DefaultComparer()
        {
            // This type constructor does nothing; it only exists to make static field initialization deterministic.
        }

        /// <summary>
        /// Returns a hash code for the specified object.
        /// </summary>
        /// <param name="obj">The object for which to return a hash code. This object may be <c>null</c>.</param>
        /// <returns>A hash code for the specified object.</returns>
        protected override int DoGetHashCode(T obj)
        {
            return EqualityComparer<T>.Default.GetHashCode(obj);
        }

        /// <summary>
        /// Compares two objects and returns a value less than 0 if <paramref name="x"/> is less than <paramref name="y"/>, 0 if <paramref name="x"/> is equal to <paramref name="y"/>, or greater than 0 if <paramref name="x"/> is greater than <paramref name="y"/>.
        /// </summary>
        /// <param name="x">The first object to compare. This object may be <c>null</c>.</param>
        /// <param name="y">The second object to compare. This object may be <c>null</c>.</param>
        /// <returns>A value less than 0 if <paramref name="x"/> is less than <paramref name="y"/>, 0 if <paramref name="x"/> is equal to <paramref name="y"/>, or greater than 0 if <paramref name="x"/> is greater than <paramref name="y"/>.</returns>
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
                Contract.Ensures(Contract.Result<DefaultComparer<T>>() != null);
                return instance;
            }
        }

        /// <summary>
        /// Compares two objects and returns a value indicating whether they are equal.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns><c>true</c> if <paramref name="x"/> is equal to <paramref name="y"/>; otherwise <c>false</c>.</returns>
        bool IEqualityComparer<T>.Equals(T x, T y)
        {
            return EqualityComparer<T>.Default.Equals(x, y);
        }

        /// <summary>
        /// Returns a hash code for the specified object.
        /// </summary>
        /// <param name="obj">The object for which to return a hash code.</param>
        /// <returns>A hash code for the specified object.</returns>
        [ContractOption(category: "contract", setting: "inheritance", toggle: false)]
        int IEqualityComparer<T>.GetHashCode(T obj)
        {
            return EqualityComparer<T>.Default.GetHashCode(obj);
        }

        /// <summary>
        /// Compares two objects and returns a value indicating whether they are equal.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns><c>true</c> if <paramref name="x"/> is equal to <paramref name="y"/>; otherwise <c>false</c>.</returns>
        bool System.Collections.IEqualityComparer.Equals(object x, object y)
        {
            return object.Equals(x, y);
        }

        /// <summary>
        /// Returns a hash code for the specified object.
        /// </summary>
        /// <param name="obj">The object for which to return a hash code.</param>
        /// <returns>A hash code for the specified object.</returns>
        [ContractOption(category: "contract", setting: "inheritance", toggle: false)]
        [SuppressMessage("Microsoft.Contracts", "Nonnull-22-0")]
        int System.Collections.IEqualityComparer.GetHashCode(object obj)
        {
            Contract.Assume(obj == null || obj is T);
            return (this as IEqualityComparer<T>).GetHashCode((T)obj);
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
                Contract.Assume(comparableBaseGeneric.IsGenericTypeDefinition && comparableBaseGeneric.GetGenericArguments().Length == 1);
                var comparableBase = comparableBaseGeneric.MakeGenericType(typeofT);
                var property = comparableBase.GetProperty("DefaultComparer", BindingFlags.Static | BindingFlags.Public);
                Contract.Assume(property != null);
                var value = property.GetValue(null, null);
                Contract.Assume(value != null);
                comparableBaseString = value.ToString();
            }
            catch
            {
            }

            if (comparableBaseString != null)
                return "Default(" + comparableBaseString + ")";
            if (typeofT.GetInterfaces().FirstOrDefault(x => x.Name == "IComparable`1") != null)
                return "Default(" + typeofT.Name + ": IComparable<T>)";
            if (typeofT.GetInterfaces().FirstOrDefault(x => x.Name == "IComparable") != null)
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
                if (typeofT.GetInterfaces().FirstOrDefault(x => x.Name == "IComparable`1") != null)
                    return true;
                if (typeofT.GetInterfaces().FirstOrDefault(x => x.Name == "IComparable") != null)
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
                if (typeofT.GetInterfaces().FirstOrDefault(x => x.Name == "IEquatable`1") != null)
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
                var enumerable = ComparerHelpers.TryGetEnumeratorType(typeof(T));
                if (enumerable == null)
                    return false;
                var defaultComparerGenericType = typeof(DefaultComparer<>);
                Contract.Assume(defaultComparerGenericType.IsGenericTypeDefinition);
                Contract.Assume(defaultComparerGenericType.GetGenericArguments().Length == enumerable.GetGenericArguments().Length);
                var defaultComparerType = defaultComparerGenericType.MakeGenericType(enumerable.GetGenericArguments());
                var property = defaultComparerType.GetProperty("IsImplemented", BindingFlags.Static | BindingFlags.Public);
                Contract.Assume(property != null);
                var value = property.GetValue(null, null);
                Contract.Assume(value != null);
                return (bool)value;
            }
        }
    }
}
