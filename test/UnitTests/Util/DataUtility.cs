using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace UnitTests.Util
{
    public static class DataUtility
    {
        /// <summary>
        /// Creates a new string instance with the same characters.
        /// </summary>
        // ReSharper disable once UseStringInterpolation
        public static string Duplicate(string data) => string.Format("{0}", data);

        public static T Fake<T>() => (dynamic)Fake(typeof(T));

        public static object Fake(Type type)
        {
            if (type == typeof(int) || type == typeof(int?))
                return Random.Next();
            if (type == typeof(int[]))
                return new[] { Random.Next() };
            if (type == typeof(string) || type == typeof(object))
                return Guid.NewGuid().ToString("N");
            if (type == typeof(HierarchyBase))
                return new HierarchyBase { Id = Random.Next() };
            if (type == typeof(HierarchyDerived1))
                return new HierarchyDerived1 { Id = Random.Next() };
            if (type == typeof(HierarchyDerived2))
                return new HierarchyDerived2 { Id = Random.Next() };
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>))
            {
                var innerType = type.GetGenericArguments()[0];
                var result = Array.CreateInstance(innerType, 1);
                result.SetValue(Fake(innerType), 0);
                return result;
            }

            throw new InvalidOperationException($"Cannot fake; unknown type {type.Name}");
        }

        /// <summary>
        /// Returns different instances with the same values.
        /// </summary>
        public static (object, object) FakeEquivalent(Type type)
        {
            if (type == typeof(int) || type == typeof(int?))
                return (13, 13);
            if (type == typeof(int[]))
                return (new[] { 13 }, new[] { 13 });
            if (type == typeof(string) || type == typeof(object))
                return ("test", Duplicate("test"));
            if (type == typeof(HierarchyBase))
                return (new HierarchyBase { Id = 13 }, new HierarchyBase { Id = 13 });
            if (type == typeof(HierarchyDerived1))
                return (new HierarchyDerived1 { Id = 13 }, new HierarchyDerived1 { Id = 13 });
            if (type == typeof(HierarchyDerived2))
                return (new HierarchyDerived2 { Id = 13 }, new HierarchyDerived2 { Id = 13 });
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>))
            {
                var innerType = type.GetGenericArguments()[0];
                var value = Fake(innerType);
                var result1 = Array.CreateInstance(innerType, 1);
                result1.SetValue(value, 0);
                var result2 = Array.CreateInstance(innerType, 1);
                result2.SetValue(value, 0);
                return (result1, result2);
            }

            throw new InvalidOperationException($"Cannot fake; unknown type {type.Name}");
        }

        public static (object Smaller, object Larger) FakeDifferent(Type type)
        {
            if (type == typeof(int) || type == typeof(int?))
                return (7, 13);
            if (type == typeof(int[]))
                return (new[] { 7 }, new[] { 13 });
            if (type == typeof(string) || type == typeof(object))
                return ("1test", "2test");
            if (type == typeof(HierarchyBase))
                return (new HierarchyBase { Id = 7 }, new HierarchyBase { Id = 13 });
            if (type == typeof(HierarchyDerived1))
                return (new HierarchyDerived1 { Id = 7 }, new HierarchyDerived1 { Id = 13 });
            if (type == typeof(HierarchyDerived2))
                return (new HierarchyDerived2 { Id = 7 }, new HierarchyDerived2 { Id = 13 });
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>))
            {
                var innerType = type.GetGenericArguments()[0];
                var (smaller, larger) = FakeDifferent(innerType);
                var result1 = Array.CreateInstance(innerType, 1);
                result1.SetValue(smaller, 0);
                var result2 = Array.CreateInstance(innerType, 2);
                result2.SetValue(smaller, 0);
                result2.SetValue(larger, 0);
                return (result1, result2);
            }

            throw new InvalidOperationException($"Cannot fake; unknown type {type.Name}");
        }

        private static readonly Random Random = new Random();

        public static object FakeNot<T>() => FakeNot(typeof(T));

        public static object FakeNot(Type type)
        {
            if (!type.IsValueType)
                return Fake(typeof(int));
            return Fake(typeof(string));
        }

        public static Type ComparedType(System.Collections.IEqualityComparer comparer)
        {
            var genericEqualityComparerInterface = comparer.GetType().GetInterfaces().FirstOrDefault(
                x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IEqualityComparer<>));
            if (genericEqualityComparerInterface == null)
                throw new InvalidOperationException($"Unable to determine comparer type for {comparer.GetType().Name}");
            return genericEqualityComparerInterface.GenericTypeArguments[0];
        }

        public static Type ComparedType(System.Collections.IComparer comparer)
        {
            var genericComparerInterface = comparer.GetType().GetInterfaces().FirstOrDefault(
                x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IComparer<>));
            if (genericComparerInterface == null)
                throw new InvalidOperationException($"Unable to determine comparer type for {comparer.GetType().Name}");
            return genericComparerInterface.GenericTypeArguments[0];
        }

        public static Func<object, object, bool> FindIComparerTEquals(object comparer)
        {
            var genericEqualityComparerInterface = comparer.GetType().GetInterfaces().FirstOrDefault(
                x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IEqualityComparer<>));
            if (genericEqualityComparerInterface == null)
                throw new InvalidOperationException($"Unable to find IEqualityComparer<T> interface for {comparer.GetType().Name}");
            var method = genericEqualityComparerInterface.GetMethod("Equals");
            return (x, y) => (bool)method.Invoke(comparer, new[] { x, y });
        }

        public static Func<object, int> FindIComparerTGetHashCode(object comparer)
        {
            var genericEqualityComparerInterface = comparer.GetType().GetInterfaces().FirstOrDefault(
                x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IEqualityComparer<>));
            if (genericEqualityComparerInterface == null)
                throw new InvalidOperationException($"Unable to find IEqualityComparer<T> interface for {comparer.GetType().Name}");
            var method = genericEqualityComparerInterface.GetMethod("GetHashCode");
            return x => (int)method.Invoke(comparer, new[] { x });
        }

        public static Func<object, object, int> FindIComparerTCompare(object comparer)
        {
            var genericComparerInterface = comparer.GetType().GetInterfaces().FirstOrDefault(
                x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IComparer<>));
            if (genericComparerInterface == null)
                throw new InvalidOperationException($"Unable to determine comparer type for {comparer.GetType().Name}");
            var method = genericComparerInterface.GetMethod("Compare");
            return (x, y) => (int)method.Invoke(comparer, new[] { x, y });
        }

        public static string Key<T>(Expression<Func<T>> expression)
        {
            var propertyInfo = FindDebugView(expression.GetType());
            return $"{expression.Compile()()} {propertyInfo.GetValue(expression)}";
        }

        private static PropertyInfo FindDebugView(Type type)
        {
            while (true)
            {
                var result = type.GetProperty("DebugView", BindingFlags.Instance | BindingFlags.NonPublic);
                if (result != null) return result;
                if (type.BaseType == null) return null;
                type = type.BaseType;
            }
        }

        public static bool IsNullValid(Type type) => !type.IsValueType || Nullable.GetUnderlyingType(type) != null;
    }
}
