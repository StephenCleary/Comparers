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
            () => ComparerBuilder.For<int>().OrderBy(x => x % 17, null, false, false),
            () => ComparerBuilder.For<int?>().OrderBy(x => x % 17, null, false, false),
            () => ComparerBuilder.For<int[]>().OrderBy(x => x[0], null, false, false),
            () => ComparerBuilder.For<HierarchyBase>().OrderBy(x => x.Id % 17, null, false, false),
            () => ComparerBuilder.For<string>().OrderBy(x => x.ToLowerInvariant(), null, false, false),
            () => ComparerBuilder.For<object>().OrderBy(x => (string)x, null, false, false),

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
            () => ComparerBuilder.For<object>().OrderBy(x => (string)x, null, false, false).Reverse(),
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
        public static readonly TheoryData<string, object, object> DifferentInstancesAndEqualData = Comparers
            // Default comparer cannot test <HierarchyBase> or <object>, since they cannot have different instances that are equivalent.
            .Where(x => !(x.Key.StartsWith("Default") && (ComparedType(x.Value) == typeof(object) || ComparedType(x.Value) == typeof(HierarchyBase))))
            // Reference comparer cannot be tested here, since no reference comparer can have different instances that are equivalent.
            .Where(x => !x.Key.StartsWith("Reference"))
            .Select(x =>
            {
                var data = FakeEquivalent(ComparedType(x.Value));
                return (x.Key, data.Item1, data.Item2);
            })
            .Append((Key(() => HierarchyComparers.BaseComparer), new HierarchyDerived1 { Id = 13 }, new HierarchyDerived2 { Id = 13 }))
            .ToTheoryData();

        [Theory]
        [MemberData(nameof(NotEqualData))]
        public void NonequivalentInstances_AreOrdered(string comparerKey, object smaller, object larger)
        {
            var comparer = Comparers[comparerKey];
            if (comparerKey.StartsWith("Reverse"))
            {
                Assert.True(comparer.Compare(smaller, larger) > 0);
                Assert.True(comparer.Compare(larger, smaller) < 0);
            }
            else
            {
                Assert.True(comparer.Compare(smaller, larger) < 0);
                Assert.True(comparer.Compare(larger, smaller) > 0);
            }
        }
        public static readonly TheoryData<string, object, object> NotEqualData = Comparers
            // Null comparer cannot be tested here, since no two instances are not equal.
            .Where(x => !x.Key.StartsWith("Null"))
            .Select(x =>
            {
                var data = FakeDifferent(ComparedType(x.Value));
                return (x.Key, data.Item1, data.Item2);
            })
            .Append((Key(() => HierarchyComparers.BaseComparer), new HierarchyDerived1 { Id = 7 }, new HierarchyDerived2 { Id = 13 }))
            .ToTheoryData();

        [Theory]
        [MemberData(nameof(NotEqualData))]
        public void OneInstanceNull_IsOrdered(string comparerKey, object smaller, object _)
        {
            var comparer = Comparers[comparerKey];
            if (comparerKey.StartsWith("Reverse") && IsNullValid(ComparedType(comparer)))
            {
                Assert.True(comparer.Compare(smaller, null) < 0);
                Assert.True(comparer.Compare(null, smaller) > 0);
            }
            else
            {
                Assert.True(comparer.Compare(smaller, null) > 0);
                Assert.True(comparer.Compare(null, smaller) < 0);
            }
        }
    }
}
