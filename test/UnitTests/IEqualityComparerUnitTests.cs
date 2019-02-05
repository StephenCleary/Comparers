using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
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
        public void Equals_IsReflexive(string comparerKey, Type comparedTypeOverride)
        {
            var comparer = EqualityComparers[comparerKey];
            var instance = Fake(comparedTypeOverride ?? ComparedType(comparer));
            Assert.True(comparer.Equals(instance, instance));
            Assert.Equal(comparer.GetHashCode(instance), comparer.GetHashCode(instance));
        }
        public static readonly TheoryData<string, Type> ReflexiveData = new TheoryData<string, Type>
        {
            { Key(() => EqualityComparerBuilder.For<int>().Default()), null },
            { Key(() => EqualityComparerBuilder.For<int?>().Default()), null },
            { Key(() => EqualityComparerBuilder.For<int[]>().Default()), null },
            { Key(() => EqualityComparerBuilder.For<HierarchyBase>().Default()), null },
            { Key(() => EqualityComparerBuilder.For<string>().Default()), null },
            { Key(() => EqualityComparerBuilder.For<object>().Default()), null },
            { Key(() => HierarchyComparers.BaseEqualityComparer), null },
            { Key(() => HierarchyComparers.BaseEqualityComparer), typeof(HierarchyDerived1) },
            { Key(() => HierarchyComparers.Derived1EqualityComparer), null },
            { Key(() => EqualityComparerBuilder.For<int>().Null()), null },
            { Key(() => EqualityComparerBuilder.For<int?>().Null()), null },
            { Key(() => EqualityComparerBuilder.For<int[]>().Null()), null },
            { Key(() => EqualityComparerBuilder.For<HierarchyBase>().Null()), null },
            { Key(() => EqualityComparerBuilder.For<string>().Null()), null },
            { Key(() => EqualityComparerBuilder.For<object>().Null()), null },
        };

        [Theory]
        [MemberData(nameof(All))]
        public void Equals_BothParametersNull_IsReflexive(string comparerKey)
        {
            var comparer = EqualityComparers[comparerKey];
            Assert.True(comparer.Equals(null, null));
            Assert.Equal(comparer.GetHashCode(null), comparer.GetHashCode(null));
        }

        [Theory]
        [MemberData(nameof(DifferentTypesData))]
        public void Equals_OneInstanceIsIncompatible_ReturnsFalse(string comparerKey, Type comparedTypeOverride)
        {
            var comparer = EqualityComparers[comparerKey];
            var comparedType = comparedTypeOverride ?? ComparedType(comparer);
            var a = Fake(comparedType);
            var b = FakeNot(comparedType);
            Assert.False(comparer.Equals(a, b));
            Assert.False(comparer.Equals(b, a));
        }
        public static readonly TheoryData<string, Type> DifferentTypesData = new TheoryData<string, Type>
        {
            { Key(() => EqualityComparerBuilder.For<int>().Default()), null },
            { Key(() => EqualityComparerBuilder.For<int?>().Default()), null },
            { Key(() => EqualityComparerBuilder.For<int[]>().Default()), null },
            { Key(() => EqualityComparerBuilder.For<HierarchyBase>().Default()), null },
            { Key(() => EqualityComparerBuilder.For<string>().Default()), null },
            { Key(() => EqualityComparerBuilder.For<object>().Default()), null },
            { Key(() => HierarchyComparers.BaseEqualityComparer), null },
            { Key(() => HierarchyComparers.BaseEqualityComparer), typeof(HierarchyDerived1) },
            { Key(() => HierarchyComparers.Derived1EqualityComparer), null },
            { Key(() => EqualityComparerBuilder.For<int>().Null()), null },
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
            { Key(() => EqualityComparerBuilder.For<int>().Default()), FakeNot<int>(), FakeNot<int>() },
            { Key(() => EqualityComparerBuilder.For<int?>().Default()), FakeNot<int?>(), FakeNot<int?>() },
            { Key(() => EqualityComparerBuilder.For<int[]>().Default()), FakeNot<int[]>(), FakeNot<int[]>() },
            { Key(() => EqualityComparerBuilder.For<HierarchyBase>().Default()), FakeNot<HierarchyBase>(), FakeNot<HierarchyBase>() },
            { Key(() => EqualityComparerBuilder.For<string>().Default()), FakeNot<string>(), FakeNot<string>() },
            { Key(() => HierarchyComparers.BaseEqualityComparer), FakeNot<HierarchyBase>(), FakeNot<HierarchyBase>() },
            { Key(() => HierarchyComparers.Derived1EqualityComparer), FakeNot<HierarchyDerived1>(), FakeNot<HierarchyDerived1>() },
            { Key(() => EqualityComparerBuilder.For<int>().Null()), FakeNot<int>(), FakeNot<int>() },
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
            { Key(() => EqualityComparerBuilder.For<HierarchyBase>().Default()), 13 },
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
        //  Non-sequence reference type (HierarchyBase)
        //  string
        //  object

        private static readonly ExpressionDictionary<IEqualityComparer> EqualityComparers = new ExpressionDictionary<IEqualityComparer>
        {
            // Default
            () => EqualityComparerBuilder.For<int>().Default(),
            () => EqualityComparerBuilder.For<int?>().Default(),
            () => EqualityComparerBuilder.For<int[]>().Default(),
            () => EqualityComparerBuilder.For<HierarchyBase>().Default(),
            () => EqualityComparerBuilder.For<string>().Default(),
            () => EqualityComparerBuilder.For<object>().Default(),

            // Null
            () => EqualityComparerBuilder.For<int>().Null(),
            () => EqualityComparerBuilder.For<int?>().Null(),
            () => EqualityComparerBuilder.For<int[]>().Null(),
            () => EqualityComparerBuilder.For<HierarchyBase>().Null(),
            () => EqualityComparerBuilder.For<string>().Null(),
            () => EqualityComparerBuilder.For<object>().Null(),

            // Hierarchy
            () => HierarchyComparers.BaseEqualityComparer,
            () => HierarchyComparers.Derived1EqualityComparer,
        };

        public static readonly TheoryData<string> All = EqualityComparers.AllKeys();


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
    }
}
