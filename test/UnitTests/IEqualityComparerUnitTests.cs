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
            //{ EqualityComparer<int>.Default, new HierarchyBase { Id = 13 } }, // .Equals returns true due to reference equality check, but .GetHashCode throws: https://github.com/dotnet/corefx/blob/53a33cf2662ac8c9a45d13067012d80cf0ba6956/src/Common/src/CoreLib/System/Collections/Generic/EqualityComparer.cs#L29

            { Key(() => EqualityComparerBuilder.For<int>().Default()), 13 },
            { Key(() => EqualityComparerBuilder.For<int>().Default()), null },
            { Key(() => EqualityComparerBuilder.For<int?>().Default()), 13 },
            { Key(() => EqualityComparerBuilder.For<int?>().Default()), null },
            { Key(() => EqualityComparerBuilder.For<int[]>().Default()), new[] { 13 } },
            { Key(() => EqualityComparerBuilder.For<int[]>().Default()), null },
            { Key(() => EqualityComparerBuilder.For<string>().Default()), "test" },
            { Key(() => EqualityComparerBuilder.For<string>().Default()), null },
            { Key(() => HierarchyComparers.BaseEqualityComparer), new HierarchyBase { Id = 13 } },
            { Key(() => HierarchyComparers.BaseEqualityComparer), new HierarchyDerived1 { Id = 13 } },
            { Key(() => HierarchyComparers.BaseEqualityComparer), null },
            { Key(() => HierarchyComparers.Derived1EqualityComparer), new HierarchyDerived1 { Id = 13 } },
        };

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
            { Key(() => EqualityComparerBuilder.For<int>().Default()), 7, "test" },
            { Key(() => EqualityComparerBuilder.For<int>().Default()), "test", 7 },
            { Key(() => EqualityComparerBuilder.For<int?>().Default()), 7, 13 },
            { Key(() => EqualityComparerBuilder.For<int?>().Default()), 7, "test" },
            { Key(() => EqualityComparerBuilder.For<int?>().Default()), "test", 7 },
            { Key(() => EqualityComparerBuilder.For<int[]>().Default()), new[] { 7 }, new[] { 13 } },
            { Key(() => EqualityComparerBuilder.For<int[]>().Default()), new[] { 7 }, "test" },
            { Key(() => EqualityComparerBuilder.For<int[]>().Default()), "test", new[] { 7 } },
            { Key(() => EqualityComparerBuilder.For<string>().Default()), "test", "other" },
            { Key(() => EqualityComparerBuilder.For<string>().Default()), "test", 13 },
            { Key(() => EqualityComparerBuilder.For<string>().Default()), 13, "test" },
            { Key(() => HierarchyComparers.BaseEqualityComparer), new HierarchyBase { Id = 7 }, new HierarchyBase { Id = 13 } },
            { Key(() => HierarchyComparers.BaseEqualityComparer), new HierarchyBase { Id = 7 }, "test" },
            { Key(() => HierarchyComparers.BaseEqualityComparer), "test", new HierarchyBase { Id = 7 } },
            { Key(() => HierarchyComparers.BaseEqualityComparer), new HierarchyDerived1 { Id = 7 }, new HierarchyDerived2 { Id = 13 } },
            { Key(() => HierarchyComparers.BaseEqualityComparer), new HierarchyDerived1 { Id = 7 }, "test" },
            { Key(() => HierarchyComparers.BaseEqualityComparer), "test", new HierarchyDerived1 { Id = 7 } },
            { Key(() => HierarchyComparers.Derived1EqualityComparer), new HierarchyDerived1 { Id = 7 }, new HierarchyDerived1 { Id = 13 } },
            { Key(() => HierarchyComparers.Derived1EqualityComparer), new HierarchyDerived1 { Id = 7 }, "test" },
            { Key(() => HierarchyComparers.Derived1EqualityComparer), "test", new HierarchyDerived1 { Id = 7 } },
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
            { Key(() => EqualityComparer<int>.Default), "test", Duplicate("test") },

            { Key(() => EqualityComparerBuilder.For<int>().Default()), "test", Duplicate("test") },
            { Key(() => EqualityComparerBuilder.For<int?>().Default()), "test", Duplicate("test") },
            { Key(() => EqualityComparerBuilder.For<int[]>().Default()), "test", Duplicate("test") },
            { Key(() => EqualityComparerBuilder.For<string>().Default()), 13, 13 },
            { Key(() => HierarchyComparers.BaseEqualityComparer), "test", Duplicate("test") },
            { Key(() => HierarchyComparers.Derived1EqualityComparer), new HierarchyBase { Id = 13 }, new HierarchyBase { Id = 13 } },
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
            { Key(() => EqualityComparer<int>.Default), "test" },

            { Key(() => EqualityComparerBuilder.For<int>().Default()), "test" },
            { Key(() => EqualityComparerBuilder.For<int?>().Default()), "test" },
            { Key(() => EqualityComparerBuilder.For<int[]>().Default()), "test" },
            { Key(() => EqualityComparerBuilder.For<string>().Default()), 13 },
            { Key(() => HierarchyComparers.BaseEqualityComparer), "test" },
            { Key(() => HierarchyComparers.Derived1EqualityComparer), "test" },
            { Key(() => HierarchyComparers.Derived1EqualityComparer), new HierarchyBase { Id = 13 } },
        };

        private static readonly Dictionary<string, IEqualityComparer> EqualityComparers = new ExpressionDictionary<IEqualityComparer>
        {
            () => EqualityComparer<int>.Default,
            () => EqualityComparerBuilder.For<int>().Default(),
            () => EqualityComparerBuilder.For<int?>().Default(),
            () => EqualityComparerBuilder.For<int[]>().Default(),
            () => EqualityComparerBuilder.For<string>().Default(),
            () => HierarchyComparers.BaseEqualityComparer,
            () => HierarchyComparers.Derived1EqualityComparer,
        };
    }
}
