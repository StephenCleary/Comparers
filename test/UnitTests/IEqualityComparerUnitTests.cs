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

            // Reference
            () => EqualityComparerBuilder.For<int>().Reference(),
            () => EqualityComparerBuilder.For<int?>().Reference(),
            () => EqualityComparerBuilder.For<int[]>().Reference(),
            () => EqualityComparerBuilder.For<HierarchyBase>().Reference(),
            () => EqualityComparerBuilder.For<string>().Reference(),
            () => EqualityComparerBuilder.For<object>().Reference(),

            // Key
            () => EqualityComparerBuilder.For<int>().EquateBy(x => x % 13, null, false),
            () => EqualityComparerBuilder.For<int?>().EquateBy(x => x % 13, null, false),
            () => EqualityComparerBuilder.For<int[]>().EquateBy(x => x.Length, null, false),
            () => EqualityComparerBuilder.For<HierarchyBase>().EquateBy(x => x.Id % 13, null, false),
            () => EqualityComparerBuilder.For<string>().EquateBy(x => x.ToLowerInvariant(), null, false),
            () => EqualityComparerBuilder.For<object>().EquateBy(x => x.GetHashCode(), null, false),

            // Sequence
            () => EqualityComparerBuilder.For<int>().Default().EquateSequence(),
            () => EqualityComparerBuilder.For<int?>().Default().EquateSequence(),
            () => EqualityComparerBuilder.For<int[]>().Default().EquateSequence(),
            () => EqualityComparerBuilder.For<HierarchyBase>().Default().EquateSequence(),
            () => EqualityComparerBuilder.For<string>().Default().EquateSequence(),
            () => EqualityComparerBuilder.For<object>().Default().EquateSequence(),

            // Hierarchy
            () => HierarchyComparers.BaseEqualityComparer,
            () => HierarchyComparers.Derived1EqualityComparer,
        };

        public static readonly TheoryData<string> All = EqualityComparers.AllKeys();

        public static readonly TheoryData<string> AllExceptObject =
            EqualityComparers.Where(x => ComparedType(x.Value) != typeof(object))
                .Select(x => x.Key)
                .ToTheoryData();


        [Theory]
        [MemberData(nameof(ReflexiveData))]
        public void Equals_IsReflexive(string comparerKey, Type comparedType)
        {
            var comparer = EqualityComparers[comparerKey];
            var instance = Fake(comparedType);
            Assert.True(comparer.Equals(instance, instance));
            Assert.Equal(comparer.GetHashCode(instance), comparer.GetHashCode(instance));
        }
        public static readonly TheoryData<string, Type> ReflexiveData =
            EqualityComparers
                // Test doesn't work for reference comparers on value types
                .Where(x => x.Key != Key(() => EqualityComparerBuilder.For<int>().Reference()))
                .Where(x => x.Key != Key(() => EqualityComparerBuilder.For<int?>().Reference()))
                .Select(x => (x.Key, ComparedType(x.Value)))
                .Append((Key(() => HierarchyComparers.BaseEqualityComparer), typeof(HierarchyDerived1)))
                .ToTheoryData();

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
        public void Equals_OneInstanceIsIncompatible_ReturnsFalse(string comparerKey, Type comparedType)
        {
            var comparer = EqualityComparers[comparerKey];
            comparedType = comparedType ?? ComparedType(comparer);
            var a = Fake(comparedType);
            var b = FakeNot(comparedType);
            Assert.False(comparer.Equals(a, b));
            Assert.False(comparer.Equals(b, a));
        }
        public static readonly TheoryData<string, Type> DifferentTypesData =
            EqualityComparers
                .Where(x => x.Key != Key(() => EqualityComparerBuilder.For<object>().Null())) // Null comparer for object will always return true from Equals
                .Select(x => (x.Key, ComparedType(x.Value)))
                .Append((Key(() => HierarchyComparers.BaseEqualityComparer), typeof(HierarchyDerived1)))
                .ToTheoryData();

        [Theory]
        [MemberData(nameof(AllExceptObject))]
        public void Equals_BothInstancesAreIncompatible_Throws(string comparerKey)
        {
            var comparer = EqualityComparers[comparerKey];
            var comparedType = ComparedType(comparer);
            var a = FakeNot(comparedType);
            var b = FakeNot(comparedType);
            Assert.ThrowsAny<ArgumentException>(() => comparer.Equals(a, b));
        }

        [Theory]
        [MemberData(nameof(AllExceptObject))]
        public void GetHashCode_IncompatibleInstance_Throws(string comparerKey)
        {
            var comparer = EqualityComparers[comparerKey];
            var comparedType = ComparedType(comparer);
            var a = FakeNot(comparedType);
            Assert.ThrowsAny<ArgumentException>(() => comparer.GetHashCode(a));
        }


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
