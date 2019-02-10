using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nito.Comparers;
using Xunit;
using UnitTests.Util;
using static UnitTests.Util.DataUtility;

namespace UnitTests
{
    /// <summary>
    /// Unit tests for (nongeneric) <see cref="IComparer.Compare(object,object)"/>.
    /// </summary>
    public class IComparerUnitTests
    {
        private static readonly ExpressionDictionary<IComparer> Comparers = new ExpressionDictionary<IComparer>
        {
            // Default
            () => ComparerBuilder.For<int>().Default(),
            () => ComparerBuilder.For<int?>().Default(),
            () => ComparerBuilder.For<int[]>().Default(),
            // Default comparer is not defined for HierarchyBase
            () => ComparerBuilder.For<string>().Default(),
            () => ComparerBuilder.For<object>().Default(),

            // Null
            () => ComparerBuilder.For<int>().Null(),
            () => ComparerBuilder.For<int?>().Null(),
            () => ComparerBuilder.For<int[]>().Null(),
            () => ComparerBuilder.For<HierarchyBase>().Null(),
            () => ComparerBuilder.For<string>().Null(),
            () => ComparerBuilder.For<object>().Null(),

            // Key
            () => ComparerBuilder.For<int>().OrderBy(x => x % 13, null, false, false),
            () => ComparerBuilder.For<int?>().OrderBy(x => x % 13, null, false, false),
            () => ComparerBuilder.For<int[]>().OrderBy(x => x.Length, null, false, false),
            () => ComparerBuilder.For<HierarchyBase>().OrderBy(x => x.Id % 13, null, false, false),
            () => ComparerBuilder.For<string>().OrderBy(x => x.ToLowerInvariant(), null, false, false),
            () => ComparerBuilder.For<object>().OrderBy(x => x.GetHashCode(), null, false, false),

            // Sequence
            () => ComparerBuilder.For<int>().Default().Sequence(),
            () => ComparerBuilder.For<int?>().Default().Sequence(),
            () => ComparerBuilder.For<int[]>().Default().Sequence(),
            () => ComparerBuilder.For<HierarchyBase>().OrderBy(x => x.Id, null, false, false).Sequence(),
            () => ComparerBuilder.For<string>().Default().Sequence(),
            () => ComparerBuilder.For<object>().Default().Sequence(),

            // Hierarchy
            () => HierarchyComparers.BaseComparer,
            () => HierarchyComparers.Derived1Comparer,

            // Reverse
            () => ComparerBuilder.For<int>().Default().Reverse(),
            () => ComparerBuilder.For<int?>().Default().Reverse(),
            () => ComparerBuilder.For<int[]>().Default().Reverse(),
            () => ComparerBuilder.For<HierarchyBase>().OrderBy(x => x.Id, null, false, false).Reverse(),
            () => ComparerBuilder.For<string>().Default().Reverse(),
            () => ComparerBuilder.For<object>().Default().Reverse(),
            () => ComparerBuilder.For<object>().OrderBy(x => 13, null, false, false).Reverse(),
        };

        public static readonly List<KeyValuePair<string, IComparer>> ComparersExceptObject =
            Comparers.Where(x => ComparedType(x.Value) != typeof(object)).ToList();

        public static readonly TheoryData<string> All = Comparers.Keys.ToTheoryData();

        public static readonly TheoryData<string> AllExceptObject = ComparersExceptObject.Select(x => x.Key).ToTheoryData();


        [Theory]
        [MemberData(nameof(ReflexiveData))]
        public void SameInstance_IsZero(string comparerKey, Type comparedType)
        {
            var comparer = Comparers[comparerKey];
            var instance = Fake(comparedType);
            Assert.Equal(0, comparer.Compare(instance, instance));
        }
        public static readonly TheoryData<string, Type> ReflexiveData =
            Comparers
                .Select(x => (x.Key, ComparedType(x.Value)))
                .Append((Key(() => HierarchyComparers.BaseComparer), typeof(HierarchyDerived1)))
                .ToTheoryData();

        [Theory]
        [MemberData(nameof(All))]
        public void BothParametersNull_IsZero(string comparerKey)
        {
            var comparer = Comparers[comparerKey];
            Assert.Equal(0, comparer.Compare(null, null));
        }

        [Theory]
        [MemberData(nameof(DifferentTypesData))]
        public void OneInstanceIsIncompatible_Throws(string comparerKey, Type comparedType)
        {
            var comparer = Comparers[comparerKey];
            comparedType = comparedType ?? ComparedType(comparer);
            var a = Fake(comparedType);
            var b = FakeNot(comparedType);
            Assert.ThrowsAny<ArgumentException>(() => comparer.Compare(a, b));
            Assert.ThrowsAny<ArgumentException>(() => comparer.Compare(b, a));
        }
        public static readonly TheoryData<string, Type> DifferentTypesData =
            ComparersExceptObject
                .Select(x => (x.Key, ComparedType(x.Value)))
                .Append((Key(() => HierarchyComparers.BaseComparer), typeof(HierarchyDerived1)))
                .ToTheoryData();

        [Theory]
        [MemberData(nameof(AllExceptObject))]
        public void BothInstancesAreIncompatible_Throws(string comparerKey)
        {
            var comparer = Comparers[comparerKey];
            var comparedType = ComparedType(comparer);
            var a = FakeNot(comparedType);
            var b = FakeNot(comparedType);
            Assert.ThrowsAny<ArgumentException>(() => comparer.Compare(a, b));
        }

        [Theory]
        [MemberData(nameof(DifferentInstancesAndEqualData))]
        public void DifferentEquivalentInstances_IsZero(string comparerKey, object a, object b)
        {
            if (object.ReferenceEquals(a, b))
                throw new ArgumentException("Unit test error: objects must be different instances.");

            var comparer = Comparers[comparerKey];
            Assert.Equal(0, comparer.Compare(a, b));
            Assert.Equal(0, comparer.Compare(b, a));
        }
        public static readonly TheoryData<string, object, object> DifferentInstancesAndEqualData = new TheoryData<string, object, object>
        {
            // Default comparer cannot test <HierarchyBase> or <object>, since they cannot have different instances that are equivalent.
            { Key(() => ComparerBuilder.For<int>().Default()), 13, 13 },
            { Key(() => ComparerBuilder.For<int?>().Default()), 13, 13 },
            { Key(() => ComparerBuilder.For<int[]>().Default()), new[] { 13 }, new[] { 13 } },
            { Key(() => ComparerBuilder.For<string>().Default()), "test", Duplicate("test") },

            // Null
            { Key(() => ComparerBuilder.For<int>().Null()), 13, 13 },
            { Key(() => ComparerBuilder.For<int?>().Null()), 13, 13 },
            { Key(() => ComparerBuilder.For<int[]>().Null()), new[] { 13 }, new[] { 13 } },
            { Key(() => ComparerBuilder.For<HierarchyBase>().Null()), new HierarchyBase { Id = 13 }, new HierarchyBase { Id = 13 } },
            { Key(() => ComparerBuilder.For<string>().Null()), "test", Duplicate("test") },
            { Key(() => ComparerBuilder.For<object>().Null()), "test", Duplicate("test") },

            // Key
            { Key(() => ComparerBuilder.For<int>().OrderBy(x => x % 13, null, false, false)), 13, 13 },
            { Key(() => ComparerBuilder.For<int?>().OrderBy(x => x % 13, null, false, false)), 13, 13 },
            { Key(() => ComparerBuilder.For<int[]>().OrderBy(x => x.Length, null, false, false)), new[] { 13 }, new[] { 13 } },
            { Key(() => ComparerBuilder.For<HierarchyBase>().OrderBy(x => x.Id % 13, null, false, false)), new HierarchyBase { Id = 13 }, new HierarchyBase { Id = 13 } },
            { Key(() => ComparerBuilder.For<string>().OrderBy(x => x.ToLowerInvariant(), null, false, false)), "test", "Test" },
            { Key(() => ComparerBuilder.For<object>().OrderBy(x => x.GetHashCode(), null, false, false)), "test", Duplicate("test") },

            // Sequence
            { Key(() => ComparerBuilder.For<int>().Default().Sequence()), new[] { 13 }, new[] { 13 } },
            { Key(() => ComparerBuilder.For<int?>().Default().Sequence()), new int?[] { 13 }, new int?[] { 13 } },
            { Key(() => ComparerBuilder.For<int[]>().Default().Sequence()), new[] { new[] { 13 } }, new[] { new[] { 13 } } },
            { Key(() => ComparerBuilder.For<HierarchyBase>().OrderBy(x => x.Id, null, false, false).Sequence()), new[] { new HierarchyBase { Id = 13 } }, new[] { new HierarchyBase { Id = 13 } } },
            { Key(() => ComparerBuilder.For<string>().Default().Sequence()), new[] { "test" }, new[] { "test" } },
            { Key(() => ComparerBuilder.For<object>().Default().Sequence()), new[] { "test" }, new[] { "test" } },

            // Hierarchy
            { Key(() => HierarchyComparers.BaseComparer), new HierarchyBase { Id = 13 }, new HierarchyBase { Id = 13 } },
            { Key(() => HierarchyComparers.BaseComparer), new HierarchyDerived1 { Id = 13 }, new HierarchyDerived2 { Id = 13 } },
            { Key(() => HierarchyComparers.Derived1Comparer), new HierarchyDerived1 { Id = 13 }, new HierarchyDerived1 { Id = 13 } },

            // Reverse
            { Key(() => ComparerBuilder.For<int>().Default().Reverse()), 13, 13 },
            { Key(() => ComparerBuilder.For<int?>().Default().Reverse()), 13, 13 },
            { Key(() => ComparerBuilder.For<int[]>().Default().Reverse()), new[] { 13 }, new[] { 13 } },
            { Key(() => ComparerBuilder.For<HierarchyBase>().OrderBy(x => x.Id, null, false, false).Reverse()), new HierarchyBase { Id = 13 }, new HierarchyBase { Id = 13 } },
            { Key(() => ComparerBuilder.For<string>().Default().Reverse()), "test", Duplicate("test") },
            { Key(() => ComparerBuilder.For<object>().OrderBy(x => 13, null, false, false).Reverse()), "test", Duplicate("test") },
        };

        [Theory]
        [MemberData(nameof(NotEqualData))]
        public void NonequivalentInstances_AreOrdered(string comparerKey, object smaller, object larger)
        {
            var comparer = Comparers[comparerKey];
            Assert.True(comparer.Compare(smaller, larger) < 0);
            Assert.True(comparer.Compare(larger, smaller) > 0);
        }
        public static readonly TheoryData<string, object, object> NotEqualData = new TheoryData<string, object, object>
        {
            // Default
            { Key(() => ComparerBuilder.For<int>().Default()), 7, 13 },
            { Key(() => ComparerBuilder.For<int?>().Default()), 7, 13 },
            { Key(() => ComparerBuilder.For<int[]>().Default()), new[] { 7 }, new[] { 13 } },
            { Key(() => ComparerBuilder.For<string>().Default()), "1test", "2other" },
            { Key(() => ComparerBuilder.For<object>().Default()), "1test", "2other" },

            // Null comparer cannot be tested here, since no two instances are not equal.

            // Key
            { Key(() => ComparerBuilder.For<int>().OrderBy(x => x % 13, null, false, false)), 5, 7 },
            { Key(() => ComparerBuilder.For<int?>().OrderBy(x => x % 13, null, false, false)), 5, 7 },
            { Key(() => ComparerBuilder.For<int[]>().OrderBy(x => x.Length, null, false, false)), new[] { 7 }, new[] { 7, 13 } },
            { Key(() => ComparerBuilder.For<HierarchyBase>().OrderBy(x => x.Id % 13, null, false, false)), new HierarchyBase { Id = 5 }, new HierarchyBase { Id = 7 } },
            { Key(() => ComparerBuilder.For<string>().OrderBy(x => x.ToLowerInvariant(), null, false, false)), "1test", "2other" },
            { Key(() => ComparerBuilder.For<object>().OrderBy(x => x.GetHashCode(), null, false, false)), 7, 13 },

            // Sequence
            { Key(() => ComparerBuilder.For<int>().Default().Sequence()), new[] { 7 }, new[] { 13 } },
            { Key(() => ComparerBuilder.For<int?>().Default().Sequence()), new int?[] { 7 }, new int?[] { 13 } },
            { Key(() => ComparerBuilder.For<int[]>().Default().Sequence()), new[] { new[] { 7 } }, new[] { new[] { 13 } } },
            { Key(() => ComparerBuilder.For<HierarchyBase>().OrderBy(x => x.Id, null, false, false).Sequence()), new[] { new HierarchyBase { Id = 7 } }, new[] { new HierarchyBase { Id = 13 } } },
            { Key(() => ComparerBuilder.For<string>().Default().Sequence()), new[] { "1test" }, new[] { "2other" } },
            { Key(() => ComparerBuilder.For<object>().Default().Sequence()), new[] { "1test" }, new[] { "2other" } },

            // Hierarchy
            { Key(() => HierarchyComparers.BaseComparer), new HierarchyBase { Id = 7 }, new HierarchyBase { Id = 13 } },
            { Key(() => HierarchyComparers.BaseComparer), new HierarchyDerived1 { Id = 7 }, new HierarchyDerived2 { Id = 13 } },
            { Key(() => HierarchyComparers.Derived1Comparer), new HierarchyDerived1 { Id = 7 }, new HierarchyDerived1 { Id = 13 } },

            // Reverse
            { Key(() => ComparerBuilder.For<int>().Default().Reverse()), 13, 7 },
            { Key(() => ComparerBuilder.For<int?>().Default().Reverse()), 13, 7 },
            { Key(() => ComparerBuilder.For<int[]>().Default().Reverse()), new[] { 13 }, new[] { 7 } },
            { Key(() => ComparerBuilder.For<HierarchyBase>().OrderBy(x => x.Id, null, false, false).Reverse()), new HierarchyBase { Id = 13 }, new HierarchyBase { Id = 7 } },
            { Key(() => ComparerBuilder.For<string>().Default().Reverse()), "2test", "1other" },
            { Key(() => ComparerBuilder.For<object>().Default().Reverse()), "2test", "1other" },
        };
    }
}
