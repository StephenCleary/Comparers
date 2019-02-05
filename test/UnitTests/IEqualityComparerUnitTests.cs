using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nito.Comparers;
using Xunit;
using UnitTests.Util;
using static UnitTests.Util.DataUtility;
// ReSharper disable UseStringInterpolation

namespace UnitTests
{
    public class IEqualityComparerUnitTests
    {
        [Theory]
        [MemberData(nameof(ReflexiveData))]
        public void Equals_IsReflexive(string comparerKey, object a)
        {
            var comparer = EqualityComparers[comparerKey];
            Assert.True(comparer.Equals(a, a));
            Assert.Equal(comparer.GetHashCode(a), comparer.GetHashCode(a));
        }
        public static readonly TheoryData<string, object> ReflexiveData = new TheoryData<string, object>
        {
            { Key(() => EqualityComparerBuilder.For<int>().Default()), 13 },
            { Key(() => EqualityComparerBuilder.For<int?>().Default()), 13 },
            { Key(() => EqualityComparerBuilder.For<int[]>().Default()), new[] { 13 } },
            { Key(() => EqualityComparerBuilder.For<Uri>().Default()), new Uri("https://www.example.com/") },
            { Key(() => EqualityComparerBuilder.For<string>().Default()), "test" },
            { Key(() => EqualityComparerBuilder.For<object>().Default()), new object() },
            { Key(() => HierarchyComparers.BaseEqualityComparer), new HierarchyBase { Id = 13 } },
            { Key(() => HierarchyComparers.BaseEqualityComparer), new HierarchyDerived1 { Id = 13 } },
            { Key(() => HierarchyComparers.Derived1EqualityComparer), new HierarchyDerived1 { Id = 13 } },
            { Key(() => EqualityComparerBuilder.For<int>().Null()), 13 },
        };

        [Theory]
        [MemberData(nameof(NullReflexiveData))]
        public void Equals_BothParametersNull_IsReflexive(string comparerKey)
        {
            var comparer = EqualityComparers[comparerKey];
            Assert.True(comparer.Equals(null, null));
            Assert.Equal(comparer.GetHashCode(null), comparer.GetHashCode(null));
        }
        public static readonly TheoryData<string> NullReflexiveData = new TheoryData<string>
        {
            { Key(() => EqualityComparerBuilder.For<int>().Default()) },
            { Key(() => EqualityComparerBuilder.For<int?>().Default()) },
            { Key(() => EqualityComparerBuilder.For<int[]>().Default()) },
            { Key(() => EqualityComparerBuilder.For<Uri>().Default()) },
            { Key(() => EqualityComparerBuilder.For<string>().Default()) },
            { Key(() => EqualityComparerBuilder.For<object>().Default()) },
            { Key(() => HierarchyComparers.BaseEqualityComparer) },
            { Key(() => HierarchyComparers.Derived1EqualityComparer) },
            { Key(() => EqualityComparerBuilder.For<int>().Null()) },
        };

        // TODO: Move this into comparer-specific unit tests.
        [Theory]
        [MemberData(nameof(DifferentInstancesAndEqualData))]
        public void EqualsAndGetHashCode_DifferentEquivalentInstances_AreEqual(string comparerKey, object a, object b)
        {
            if (object.ReferenceEquals(a, b))
                throw new ArgumentException("Unit test error: objects must be different instances.");

            var comparer = EqualityComparers[comparerKey];
            Assert.True(comparer.Equals(a, b));
            Assert.True(comparer.Equals(b, a));
            Assert.Equal(comparer.GetHashCode(a), comparer.GetHashCode(b));
        }
        public static readonly TheoryData<string, object, object> DifferentInstancesAndEqualData = new TheoryData<string, object, object>
        {
            { Key(() => EqualityComparerBuilder.For<int>().Default()), 13, 13 },
            { Key(() => EqualityComparerBuilder.For<int?>().Default()), 13, 13 },
            { Key(() => EqualityComparerBuilder.For<int[]>().Default()), new[] { 13 }, new[] { 13 } },
            { Key(() => EqualityComparerBuilder.For<string>().Default()), "test", Duplicate("test") },
            { Key(() => HierarchyComparers.BaseEqualityComparer), new HierarchyBase { Id = 13 }, new HierarchyBase { Id = 13 } },
            { Key(() => HierarchyComparers.BaseEqualityComparer), new HierarchyDerived1 { Id = 13 }, new HierarchyDerived2 { Id = 13 } },
            { Key(() => HierarchyComparers.Derived1EqualityComparer), new HierarchyDerived1 { Id = 13 }, new HierarchyDerived1 { Id = 13 } },
        };

        // TODO: Move this into comparer-specific unit tests.
        [Theory]
        [MemberData(nameof(NotEqualData))]
        public void Equals_NonequivalentInstances_ReturnsFalse(string comparerKey, object a, object b)
        {
            var comparer = EqualityComparers[comparerKey];
            Assert.False(comparer.Equals(a, b));
            Assert.False(comparer.Equals(b, a));
        }
        public static readonly TheoryData<string, object, object> NotEqualData = new TheoryData<string, object, object>
        {
            { Key(() => EqualityComparerBuilder.For<int>().Default()), 7, 13 },
            { Key(() => EqualityComparerBuilder.For<int?>().Default()), 7, 13 },
            { Key(() => EqualityComparerBuilder.For<int[]>().Default()), new[] { 7 }, new[] { 13 } },
            { Key(() => EqualityComparerBuilder.For<string>().Default()), "test", "other" },
            { Key(() => HierarchyComparers.BaseEqualityComparer), new HierarchyBase { Id = 7 }, new HierarchyBase { Id = 13 } },
            { Key(() => HierarchyComparers.BaseEqualityComparer), new HierarchyDerived1 { Id = 7 }, new HierarchyDerived2 { Id = 13 } },
            { Key(() => HierarchyComparers.Derived1EqualityComparer), new HierarchyDerived1 { Id = 7 }, new HierarchyDerived1 { Id = 13 } },
        };

        [Theory]
        [MemberData(nameof(DifferentTypesData))]
        public void Equals_OneInstanceIsIncompatible_ReturnsFalse(string comparerKey, object a, object b)
        {
            var comparer = EqualityComparers[comparerKey];
            Assert.False(comparer.Equals(a, b));
            Assert.False(comparer.Equals(b, a));
        }
        public static readonly TheoryData<string, object, object> DifferentTypesData = new TheoryData<string, object, object>
        {
            { Key(() => EqualityComparerBuilder.For<int>().Default()), 7, "test" },
            { Key(() => EqualityComparerBuilder.For<int?>().Default()), 7, "test" },
            { Key(() => EqualityComparerBuilder.For<int[]>().Default()), new[] { 7 }, "test" },
            { Key(() => EqualityComparerBuilder.For<Uri>().Default()), new Uri("https://www.example.com/"), 13 },
            { Key(() => EqualityComparerBuilder.For<string>().Default()), "test", 13 },
            { Key(() => EqualityComparerBuilder.For<object>().Default()), new object(), 13 },
            { Key(() => HierarchyComparers.BaseEqualityComparer), new HierarchyBase { Id = 7 }, "test" },
            { Key(() => HierarchyComparers.BaseEqualityComparer), new HierarchyDerived1 { Id = 7 }, "test" },
            { Key(() => HierarchyComparers.Derived1EqualityComparer), new HierarchyDerived1 { Id = 7 }, "test" },
            { Key(() => EqualityComparerBuilder.For<int>().Null()), 7, "test" },
        };

        [Theory]
        [MemberData(nameof(EqualsThrowsData))]
        public void Equals_BothInstancesAreIncompatible_Throws(string comparerKey, object a, object b)
        {
            var comparer = EqualityComparers[comparerKey];
            Assert.ThrowsAny<ArgumentException>(() => comparer.Equals(a, b));
        }
        public static readonly TheoryData<string, object, object> EqualsThrowsData = new TheoryData<string, object, object>
        {
            // Note: Test is not meaningful for <object> comparers.
            { Key(() => EqualityComparerBuilder.For<int>().Default()), "test", Duplicate("test") },
            { Key(() => EqualityComparerBuilder.For<int?>().Default()), "test", Duplicate("test") },
            { Key(() => EqualityComparerBuilder.For<int[]>().Default()), "test", Duplicate("test") },
            { Key(() => EqualityComparerBuilder.For<Uri>().Default()), 13, 13 },
            { Key(() => EqualityComparerBuilder.For<string>().Default()), 13, 13 },
            { Key(() => HierarchyComparers.BaseEqualityComparer), "test", Duplicate("test") },
            { Key(() => HierarchyComparers.Derived1EqualityComparer), new HierarchyBase { Id = 13 }, new HierarchyBase { Id = 13 } },
            { Key(() => EqualityComparerBuilder.For<int>().Null()), "test", Duplicate("test") },
        };

        [Theory]
        [MemberData(nameof(GetHashCodeThrowsData))]
        public void GetHashCode_IncompatibleInstance_Throws(string comparerKey, object a)
        {
            var comparer = EqualityComparers[comparerKey];
            Assert.ThrowsAny<ArgumentException>(() => comparer.GetHashCode(a));
        }
        public static readonly TheoryData<string, object> GetHashCodeThrowsData = new TheoryData<string, object>
        {
            // Note: Test is not meaningful for <object> comparers.
            { Key(() => EqualityComparerBuilder.For<int>().Default()), "test" },
            { Key(() => EqualityComparerBuilder.For<int?>().Default()), "test" },
            { Key(() => EqualityComparerBuilder.For<int[]>().Default()), "test" },
            { Key(() => EqualityComparerBuilder.For<Uri>().Default()), 13 },
            { Key(() => EqualityComparerBuilder.For<string>().Default()), 13 },
            { Key(() => HierarchyComparers.BaseEqualityComparer), "test" },
            { Key(() => HierarchyComparers.Derived1EqualityComparer), "test" },
            { Key(() => HierarchyComparers.Derived1EqualityComparer), new HierarchyBase { Id = 13 } },
            { Key(() => EqualityComparerBuilder.For<int>().Null()), "test" },
        };

        // For each general class of comparers, include tests for:
        //  Value type (int)
        //  Nullable value type (int?)
        //  Sequence type (int[])
        //  Non-sequence reference type (Uri)
        //  string
        //  object

        private static readonly Dictionary<string, IEqualityComparer> EqualityComparers = new ExpressionDictionary<IEqualityComparer>
        {
            // Default
            () => EqualityComparerBuilder.For<int>().Default(),
            () => EqualityComparerBuilder.For<int?>().Default(),
            () => EqualityComparerBuilder.For<int[]>().Default(),
            () => EqualityComparerBuilder.For<Uri>().Default(),
            () => EqualityComparerBuilder.For<string>().Default(),
            () => EqualityComparerBuilder.For<object>().Default(),

            // Null
            () => EqualityComparerBuilder.For<int>().Null(),
            () => EqualityComparerBuilder.For<int?>().Null(),
            () => EqualityComparerBuilder.For<int[]>().Null(),
            () => EqualityComparerBuilder.For<Uri>().Null(),
            () => EqualityComparerBuilder.For<string>().Null(),
            () => EqualityComparerBuilder.For<object>().Null(),

            // Hierarchy
            () => HierarchyComparers.BaseEqualityComparer,
            () => HierarchyComparers.Derived1EqualityComparer,
        };
    }
}
