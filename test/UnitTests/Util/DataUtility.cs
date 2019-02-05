using System;
using System.Collections.Generic;
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

        public static string Key<T>(Expression<Func<T>> expression)
        {
            var propertyInfo = FindDebugView(expression.GetType());
            return propertyInfo.GetValue(expression) as string;
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
    }
}
