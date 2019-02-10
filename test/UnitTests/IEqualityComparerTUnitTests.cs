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
    /// <summary>
    /// Unit tests for <see cref="IEqualityComparer{T}.Equals(T,T)"/> and <see cref="IEqualityComparer{T}.GetHashCode(T)"/>.
    /// </summary>
    public class IEqualityComparerTUnitTests
    {
        private static readonly ExpressionDictionary<object> EqualityComparers = new ExpressionDictionary<object>
        {
            // Default
            () => EqualityComparerBuilder.For<int>().Default(),
            () => EqualityComparerBuilder.For<int?>().Default(),
            () => EqualityComparerBuilder.For<int[]>().Default(),
            () => EqualityComparerBuilder.For<HierarchyBase>().Default(),
            () => EqualityComparerBuilder.For<string>().Default(),
            () => EqualityComparerBuilder.For<object>().Default(),
            () => ComparerBuilder.For<int>().Default(),
            () => ComparerBuilder.For<int?>().Default(),
            () => ComparerBuilder.For<int[]>().Default(),
            // Default comparer is not defined for HierarchyBase
            () => ComparerBuilder.For<string>().Default(),
            () => ComparerBuilder.For<object>().Default(),

            // Null
            () => EqualityComparerBuilder.For<int>().Null(),
            () => EqualityComparerBuilder.For<int?>().Null(),
            () => EqualityComparerBuilder.For<int[]>().Null(),
            () => EqualityComparerBuilder.For<HierarchyBase>().Null(),
            () => EqualityComparerBuilder.For<string>().Null(),
            () => EqualityComparerBuilder.For<object>().Null(),
            () => ComparerBuilder.For<int>().Null(),
            () => ComparerBuilder.For<int?>().Null(),
            () => ComparerBuilder.For<int[]>().Null(),
            () => ComparerBuilder.For<HierarchyBase>().Null(),
            () => ComparerBuilder.For<string>().Null(),
            () => ComparerBuilder.For<object>().Null(),

            // Key
            () => EqualityComparerBuilder.For<int>().EquateBy(x => x % 13, null, false),
            () => EqualityComparerBuilder.For<int?>().EquateBy(x => x % 13, null, false),
            () => EqualityComparerBuilder.For<int[]>().EquateBy(x => x.Length, null, false),
            () => EqualityComparerBuilder.For<HierarchyBase>().EquateBy(x => x.Id % 13, null, false),
            () => EqualityComparerBuilder.For<string>().EquateBy(x => x.ToLowerInvariant(), null, false),
            () => EqualityComparerBuilder.For<object>().EquateBy(x => x.GetHashCode(), null, false),
            () => ComparerBuilder.For<int>().OrderBy(x => x % 13, null, false, false),
            () => ComparerBuilder.For<int?>().OrderBy(x => x % 13, null, false, false),
            () => ComparerBuilder.For<int[]>().OrderBy(x => x.Length, null, false, false),
            () => ComparerBuilder.For<HierarchyBase>().OrderBy(x => x.Id % 13, null, false, false),
            () => ComparerBuilder.For<string>().OrderBy(x => x.ToLowerInvariant(), null, false, false),
            () => ComparerBuilder.For<object>().OrderBy(x => x.GetHashCode(), null, false, false),

            // Sequence
            () => EqualityComparerBuilder.For<int>().Default().EquateSequence(),
            () => EqualityComparerBuilder.For<int?>().Default().EquateSequence(),
            () => EqualityComparerBuilder.For<int[]>().Default().EquateSequence(),
            () => EqualityComparerBuilder.For<HierarchyBase>().EquateBy(x => x.Id, null, false).EquateSequence(),
            () => EqualityComparerBuilder.For<string>().Default().EquateSequence(),
            () => EqualityComparerBuilder.For<object>().Default().EquateSequence(),
            () => ComparerBuilder.For<int>().Default().Sequence(),
            () => ComparerBuilder.For<int?>().Default().Sequence(),
            () => ComparerBuilder.For<int[]>().Default().Sequence(),
            () => ComparerBuilder.For<HierarchyBase>().OrderBy(x => x.Id, null, false, false).Sequence(),
            () => ComparerBuilder.For<string>().Default().Sequence(),
            () => ComparerBuilder.For<object>().Default().Sequence(),

            // Hierarchy
            () => HierarchyComparers.BaseEqualityComparer,
            () => HierarchyComparers.Derived1EqualityComparer,
            () => HierarchyComparers.BaseComparer,
            () => HierarchyComparers.Derived1Comparer,

            // Reference EqualityComparer
            () => EqualityComparerBuilder.For<int[]>().Reference(),
            () => EqualityComparerBuilder.For<HierarchyBase>().Reference(),
            () => EqualityComparerBuilder.For<string>().Reference(),
            () => EqualityComparerBuilder.For<object>().Reference(),

            // Reverse Comparer
            () => ComparerBuilder.For<int>().Default().Reverse(),
            () => ComparerBuilder.For<int?>().Default().Reverse(),
            () => ComparerBuilder.For<int[]>().Default().Reverse(),
            () => ComparerBuilder.For<HierarchyBase>().OrderBy(x => x.Id, null, false, false).Reverse(),
            () => ComparerBuilder.For<string>().Default().Reverse(),
            () => ComparerBuilder.For<object>().Default().Reverse(),
            () => ComparerBuilder.For<object>().OrderBy(x => 13, null, false, false).Reverse(),
        };

        public static readonly List<KeyValuePair<string, dynamic>> EqualityComparersExceptObject =
            EqualityComparers.Where(x => ComparedType(x.Value as IEqualityComparer) != typeof(object)).ToList();

        public static readonly TheoryData<string> All = EqualityComparers.Keys.ToTheoryData();

        public static readonly TheoryData<string> AllExceptObject = EqualityComparersExceptObject.Select(x => x.Key).ToTheoryData();


        [Theory]
        [MemberData(nameof(ReflexiveData))]
        public void Equals_IsReflexive(string comparerKey, Type comparedType)
        {
            var comparer = EqualityComparers[comparerKey];
            var instance = Fake(comparedType);
            var equals = FindIComparerTEquals(comparer);
            var getHashCode = FindIComparerTGetHashCode(comparer);
            Assert.True(equals(instance, instance));
            Assert.Equal(getHashCode(instance), getHashCode(instance));
        }
        public static readonly TheoryData<string, Type> ReflexiveData =
            EqualityComparers
                .Select(x => (x.Key, ComparedType(x.Value as IEqualityComparer)))
                .Append((Key(() => HierarchyComparers.BaseEqualityComparer), typeof(HierarchyDerived1)))
                .Append((Key(() => HierarchyComparers.BaseComparer), typeof(HierarchyDerived1)))
                .ToTheoryData();

        [Theory]
        [MemberData(nameof(All))]
        public void Equals_BothParametersNull_IsReflexive(string comparerKey)
        {
            var comparer = EqualityComparers[comparerKey];
            var equals = FindIComparerTEquals(comparer);
            var getHashCode = FindIComparerTGetHashCode(comparer);
            Assert.True(equals(null, null));
            Assert.Equal(getHashCode(null), getHashCode(null));
        }

        [Theory]
        [MemberData(nameof(DifferentInstancesAndEqualData))]
        public void EqualsAndGetHashCode_DifferentEquivalentInstances_AreEqual(string comparerKey, object a, object b)
        {
            if (object.ReferenceEquals(a, b))
                throw new ArgumentException("Unit test error: objects must be different instances.");

            var comparer = EqualityComparers[comparerKey];
            var equals = FindIComparerTEquals(comparer);
            var getHashCode = FindIComparerTGetHashCode(comparer);
            Assert.True(equals(a, b));
            Assert.True(equals(b, a));
            Assert.Equal(getHashCode(a), getHashCode(b));
        }
        public static readonly TheoryData<string, object, object> DifferentInstancesAndEqualData = new TheoryData<string, object, object>
        {
            // Default comparer cannot test <HierarchyBase> or <object>, since they cannot have different instances that are equivalent.
            { Key(() => EqualityComparerBuilder.For<int>().Default()), 13, 13 },
            { Key(() => EqualityComparerBuilder.For<int?>().Default()), 13, 13 },
            { Key(() => EqualityComparerBuilder.For<int[]>().Default()), new[] { 13 }, new[] { 13 } },
            { Key(() => EqualityComparerBuilder.For<string>().Default()), "test", Duplicate("test") },
            { Key(() => ComparerBuilder.For<int>().Default()), 13, 13 },
            { Key(() => ComparerBuilder.For<int?>().Default()), 13, 13 },
            { Key(() => ComparerBuilder.For<int[]>().Default()), new[] { 13 }, new[] { 13 } },
            { Key(() => ComparerBuilder.For<string>().Default()), "test", Duplicate("test") },

            // Null
            { Key(() => EqualityComparerBuilder.For<int>().Null()), 13, 13 },
            { Key(() => EqualityComparerBuilder.For<int?>().Null()), 13, 13 },
            { Key(() => EqualityComparerBuilder.For<int[]>().Null()), new[] { 13 }, new[] { 13 } },
            { Key(() => EqualityComparerBuilder.For<HierarchyBase>().Null()), new HierarchyBase { Id = 13 }, new HierarchyBase { Id = 13 } },
            { Key(() => EqualityComparerBuilder.For<string>().Null()), "test", Duplicate("test") },
            { Key(() => EqualityComparerBuilder.For<object>().Null()), "test", Duplicate("test") },
            { Key(() => ComparerBuilder.For<int>().Null()), 13, 13 },
            { Key(() => ComparerBuilder.For<int?>().Null()), 13, 13 },
            { Key(() => ComparerBuilder.For<int[]>().Null()), new[] { 13 }, new[] { 13 } },
            { Key(() => ComparerBuilder.For<HierarchyBase>().Null()), new HierarchyBase { Id = 13 }, new HierarchyBase { Id = 13 } },
            { Key(() => ComparerBuilder.For<string>().Null()), "test", Duplicate("test") },
            { Key(() => ComparerBuilder.For<object>().Null()), "test", Duplicate("test") },

            // Key
            { Key(() => EqualityComparerBuilder.For<int>().EquateBy(x => x % 13, null, false)), 13, 13 },
            { Key(() => EqualityComparerBuilder.For<int?>().EquateBy(x => x % 13, null, false)), 13, 13 },
            { Key(() => EqualityComparerBuilder.For<int[]>().EquateBy(x => x.Length, null, false)), new[] { 13 }, new[] { 13 } },
            { Key(() => EqualityComparerBuilder.For<HierarchyBase>().EquateBy(x => x.Id % 13, null, false)), new HierarchyBase { Id = 13 }, new HierarchyBase { Id = 13 } },
            { Key(() => EqualityComparerBuilder.For<string>().EquateBy(x => x.ToLowerInvariant(), null, false)), "test", "Test" },
            { Key(() => EqualityComparerBuilder.For<object>().EquateBy(x => x.GetHashCode(), null, false)), "test", Duplicate("test") },
            { Key(() => ComparerBuilder.For<int>().OrderBy(x => x % 13, null, false, false)), 13, 13 },
            { Key(() => ComparerBuilder.For<int?>().OrderBy(x => x % 13, null, false, false)), 13, 13 },
            { Key(() => ComparerBuilder.For<int[]>().OrderBy(x => x.Length, null, false, false)), new[] { 13 }, new[] { 13 } },
            { Key(() => ComparerBuilder.For<HierarchyBase>().OrderBy(x => x.Id % 13, null, false, false)), new HierarchyBase { Id = 13 }, new HierarchyBase { Id = 13 } },
            { Key(() => ComparerBuilder.For<string>().OrderBy(x => x.ToLowerInvariant(), null, false, false)), "test", "Test" },
            { Key(() => ComparerBuilder.For<object>().OrderBy(x => x.GetHashCode(), null, false, false)), "test", Duplicate("test") },

            // Sequence
            { Key(() => EqualityComparerBuilder.For<int>().Default().EquateSequence()), new[] { 13 }, new[] { 13 } },
            { Key(() => EqualityComparerBuilder.For<int?>().Default().EquateSequence()), new int?[] { 13 }, new int?[] { 13 } },
            { Key(() => EqualityComparerBuilder.For<int[]>().Default().EquateSequence()), new[] { new[] { 13 } }, new[] { new[] { 13 } } },
            { Key(() => EqualityComparerBuilder.For<HierarchyBase>().EquateBy(x => x.Id, null, false).EquateSequence()), new[] { new HierarchyBase { Id = 13 } }, new[] { new HierarchyBase { Id = 13 } } },
            { Key(() => EqualityComparerBuilder.For<string>().Default().EquateSequence()), new[] { "test" }, new[] { "test" } },
            { Key(() => EqualityComparerBuilder.For<object>().Default().EquateSequence()), new[] { "test" }, new[] { "test" } },
            { Key(() => ComparerBuilder.For<int>().Default().Sequence()), new[] { 13 }, new[] { 13 } },
            { Key(() => ComparerBuilder.For<int?>().Default().Sequence()), new int?[] { 13 }, new int?[] { 13 } },
            { Key(() => ComparerBuilder.For<int[]>().Default().Sequence()), new[] { new[] { 13 } }, new[] { new[] { 13 } } },
            { Key(() => ComparerBuilder.For<HierarchyBase>().OrderBy(x => x.Id, null, false, false).Sequence()), new[] { new HierarchyBase { Id = 13 } }, new[] { new HierarchyBase { Id = 13 } } },
            { Key(() => ComparerBuilder.For<string>().Default().Sequence()), new[] { "test" }, new[] { "test" } },
            { Key(() => ComparerBuilder.For<object>().Default().Sequence()), new[] { "test" }, new[] { "test" } },

            // Hierarchy
            { Key(() => HierarchyComparers.BaseEqualityComparer), new HierarchyBase { Id = 13 }, new HierarchyBase { Id = 13 } },
            { Key(() => HierarchyComparers.BaseEqualityComparer), new HierarchyDerived1 { Id = 13 }, new HierarchyDerived2 { Id = 13 } },
            { Key(() => HierarchyComparers.Derived1EqualityComparer), new HierarchyDerived1 { Id = 13 }, new HierarchyDerived1 { Id = 13 } },
            { Key(() => HierarchyComparers.BaseComparer), new HierarchyBase { Id = 13 }, new HierarchyBase { Id = 13 } },
            { Key(() => HierarchyComparers.BaseComparer), new HierarchyDerived1 { Id = 13 }, new HierarchyDerived2 { Id = 13 } },
            { Key(() => HierarchyComparers.Derived1Comparer), new HierarchyDerived1 { Id = 13 }, new HierarchyDerived1 { Id = 13 } },

            // Reference comparer cannot be tested here, since no reference comparer can have different instances that are equivalent.

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
        public void Equals_NonequivalentInstances_ReturnsFalse(string comparerKey, object a, object b)
        {
            var comparer = EqualityComparers[comparerKey];
            var equals = FindIComparerTEquals(comparer);
            Assert.False(equals(a, b));
            Assert.False(equals(b, a));
        }
        public static readonly TheoryData<string, object, object> NotEqualData = new TheoryData<string, object, object>
        {
            // Default
            { Key(() => EqualityComparerBuilder.For<int>().Default()), 7, 13 },
            { Key(() => EqualityComparerBuilder.For<int?>().Default()), 7, 13 },
            { Key(() => EqualityComparerBuilder.For<int[]>().Default()), new[] { 7 }, new[] { 13 } },
            { Key(() => EqualityComparerBuilder.For<HierarchyBase>().Default()), new HierarchyBase { Id = 7 }, new HierarchyBase { Id = 13 } },
            { Key(() => EqualityComparerBuilder.For<string>().Default()), "test", "other" },
            { Key(() => EqualityComparerBuilder.For<object>().Default()), "test", "other" },
            { Key(() => ComparerBuilder.For<int>().Default()), 7, 13 },
            { Key(() => ComparerBuilder.For<int?>().Default()), 7, 13 },
            { Key(() => ComparerBuilder.For<int[]>().Default()), new[] { 7 }, new[] { 13 } },
            { Key(() => ComparerBuilder.For<string>().Default()), "test", "other" },
            { Key(() => ComparerBuilder.For<object>().Default()), "test", "other" },

            // Null comparer cannot be tested here, since no two instances are not equal.

            // Key
            { Key(() => EqualityComparerBuilder.For<int>().EquateBy(x => x % 13, null, false)), 7, 13 },
            { Key(() => EqualityComparerBuilder.For<int?>().EquateBy(x => x % 13, null, false)), 7, 13 },
            { Key(() => EqualityComparerBuilder.For<int[]>().EquateBy(x => x.Length, null, false)), new[] { 7, 13 }, new[] { 13 } },
            { Key(() => EqualityComparerBuilder.For<HierarchyBase>().EquateBy(x => x.Id % 13, null, false)), new HierarchyBase { Id = 7 }, new HierarchyBase { Id = 13 } },
            { Key(() => EqualityComparerBuilder.For<string>().EquateBy(x => x.ToLowerInvariant(), null, false)), "test", "other" },
            { Key(() => EqualityComparerBuilder.For<object>().EquateBy(x => x.GetHashCode(), null, false)), "test", "other" },
            { Key(() => ComparerBuilder.For<int>().OrderBy(x => x % 13, null, false, false)), 7, 13 },
            { Key(() => ComparerBuilder.For<int?>().OrderBy(x => x % 13, null, false, false)), 7, 13 },
            { Key(() => ComparerBuilder.For<int[]>().OrderBy(x => x.Length, null, false, false)), new[] { 7, 13 }, new[] { 13 } },
            { Key(() => ComparerBuilder.For<HierarchyBase>().OrderBy(x => x.Id % 13, null, false, false)), new HierarchyBase { Id = 7 }, new HierarchyBase { Id = 13 } },
            { Key(() => ComparerBuilder.For<string>().OrderBy(x => x.ToLowerInvariant(), null, false, false)), "test", "other" },
            { Key(() => ComparerBuilder.For<object>().OrderBy(x => x.GetHashCode(), null, false, false)), "test", "other" },

            // Sequence
            { Key(() => EqualityComparerBuilder.For<int>().Default().EquateSequence()), new[] { 7 }, new[] { 13 } },
            { Key(() => EqualityComparerBuilder.For<int?>().Default().EquateSequence()), new int?[] { 7 }, new int?[] { 13 } },
            { Key(() => EqualityComparerBuilder.For<int[]>().Default().EquateSequence()), new[] { new[] { 7 } }, new[] { new[] { 13 } } },
            { Key(() => EqualityComparerBuilder.For<HierarchyBase>().EquateBy(x => x.Id, null, false).EquateSequence()), new[] { new HierarchyBase { Id = 7 } }, new[] { new HierarchyBase { Id = 13 } } },
            { Key(() => EqualityComparerBuilder.For<string>().Default().EquateSequence()), new[] { "test" }, new[] { "other" } },
            { Key(() => EqualityComparerBuilder.For<object>().Default().EquateSequence()), new[] { "test" }, new[] { "other" } },
            { Key(() => ComparerBuilder.For<int>().Default().Sequence()), new[] { 7 }, new[] { 13 } },
            { Key(() => ComparerBuilder.For<int?>().Default().Sequence()), new int?[] { 7 }, new int?[] { 13 } },
            { Key(() => ComparerBuilder.For<int[]>().Default().Sequence()), new[] { new[] { 7 } }, new[] { new[] { 13 } } },
            { Key(() => ComparerBuilder.For<HierarchyBase>().OrderBy(x => x.Id, null, false, false).Sequence()), new[] { new HierarchyBase { Id = 7 } }, new[] { new HierarchyBase { Id = 13 } } },
            { Key(() => ComparerBuilder.For<string>().Default().Sequence()), new[] { "test" }, new[] { "other" } },
            { Key(() => ComparerBuilder.For<object>().Default().Sequence()), new[] { "test" }, new[] { "other" } },

            // Hierarchy
            { Key(() => HierarchyComparers.BaseEqualityComparer), new HierarchyBase { Id = 7 }, new HierarchyBase { Id = 13 } },
            { Key(() => HierarchyComparers.BaseEqualityComparer), new HierarchyDerived1 { Id = 7 }, new HierarchyDerived2 { Id = 13 } },
            { Key(() => HierarchyComparers.Derived1EqualityComparer), new HierarchyDerived1 { Id = 7 }, new HierarchyDerived1 { Id = 13 } },
            { Key(() => HierarchyComparers.BaseComparer), new HierarchyBase { Id = 7 }, new HierarchyBase { Id = 13 } },
            { Key(() => HierarchyComparers.BaseComparer), new HierarchyDerived1 { Id = 7 }, new HierarchyDerived2 { Id = 13 } },
            { Key(() => HierarchyComparers.Derived1Comparer), new HierarchyDerived1 { Id = 7 }, new HierarchyDerived1 { Id = 13 } },

            // Reference
            { Key(() => EqualityComparerBuilder.For<int[]>().Reference()), new[] { 7 }, new[] { 13 } },
            { Key(() => EqualityComparerBuilder.For<HierarchyBase>().Reference()), new HierarchyBase { Id = 7 }, new HierarchyBase { Id = 13 } },
            { Key(() => EqualityComparerBuilder.For<string>().Reference()), "test", "other" },
            { Key(() => EqualityComparerBuilder.For<object>().Reference()), "test", "other" },

            // Reverse
            { Key(() => ComparerBuilder.For<int>().Default().Reverse()), 7, 13 },
            { Key(() => ComparerBuilder.For<int?>().Default().Reverse()), 7, 13 },
            { Key(() => ComparerBuilder.For<int[]>().Default().Reverse()), new[] { 7 }, new[] { 13 } },
            { Key(() => ComparerBuilder.For<HierarchyBase>().OrderBy(x => x.Id, null, false, false).Reverse()), new HierarchyBase { Id = 7 }, new HierarchyBase { Id = 13 } },
            { Key(() => ComparerBuilder.For<string>().Default().Reverse()), "test", "other" },
            { Key(() => ComparerBuilder.For<object>().Default().Reverse()), "test", "other" },
        };
    }
}
