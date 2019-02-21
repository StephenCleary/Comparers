using Nito.Comparers;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using Xunit;

namespace UnitTests.Examples
{
    public class AllMembers
    {
        [Fact]
        public void BasicTest()
        {
            var c1 = new Component
            {
                Weight = 0.7,
                AccumulatedWeight = 0.2,
                NestedRef = new Nested
                {
                    NestedProp = 223
                }
            };

            var c2 = new Component
            {
                Weight = 0.7,
                AccumulatedWeight = 0.2,
                NestedRef = new Nested
                {
                    NestedProp = 223
                }
            };

            var comparer = AllMembersEqualityComparer<Component>();
            Assert.True(comparer.Equals(c1, c2));
            Assert.Equal(comparer.GetHashCode(c1), comparer.GetHashCode(c2));
        }

        private class Component
        {
            public double AccumulatedWeight { get; set; }

            public double Weight { get; set; }

            public Nested NestedRef { get; set; }
        }

        private class Nested
        {
            public double NestedProp { get; set; }
        }

        private static IFullEqualityComparer<T> AllMembersEqualityComparer<T>()
        {
            var result = EqualityComparerBuilder.For<T>().Null();
            var type = typeof(T);
            var parameter = Expression.Parameter(type);
            foreach (var prop in type.GetProperties())
            {
                var expression = Expression.Property(parameter, prop.Name);
                dynamic selector = Expression.Lambda(expression, parameter).Compile();
                dynamic childComparer = null;
                if (prop.PropertyType.IsClass)
                    childComparer = ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(prop.PropertyType).Invoke(null, null);
                result = EqualityComparerExtensions.ThenEquateBy(result, selector, childComparer);
            }
            return result;
        }
    }
}
