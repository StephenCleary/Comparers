using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Nito.Comparers;
using UnitTests.Util;
using Xunit;
using static UnitTests.Util.DataUtility;

namespace UnitTests
{
    /// <summary>
    /// Unit tests for <see cref="IComparer{T}.Compare(T,T)"/>.
    /// </summary>
    public class IComparerTUnitTests
    {
        private static readonly ExpressionDictionary<object> Comparers = new ExpressionDictionary<object>
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
            () => ComparerBuilder.For<int>().OrderBy(x => x % 17, (IComparer<int>)null, false, false),
            () => ComparerBuilder.For<int?>().OrderBy(x => x % 17, (IComparer<int?>)null, false, false),
            () => ComparerBuilder.For<int[]>().OrderBy(x => x[0], (IComparer<int>)null, false, false),
            () => ComparerBuilder.For<HierarchyBase>().OrderBy(x => x.Id % 17, (IComparer<int>)null, false, false),
            () => ComparerBuilder.For<string>().OrderBy(x => x.ToLowerInvariant(), (IComparer<string>)null, false, false),
            () => ComparerBuilder.For<object>().OrderBy(x => (string)x, (IComparer<string>)null, false, false),

            // Sequence
            () => ComparerBuilder.For<int>().Default().Sequence(),
            () => ComparerBuilder.For<int?>().Default().Sequence(),
            () => ComparerBuilder.For<int[]>().Default().Sequence(),
            () => ComparerBuilder.For<HierarchyBase>().OrderBy(x => x.Id, (IComparer<int>)null, false, false).Sequence(),
            () => ComparerBuilder.For<string>().Default().Sequence(),
            () => ComparerBuilder.For<object>().Default().Sequence(),

            // Hierarchy
            () => HierarchyComparers.BaseComparer,
            () => HierarchyComparers.Derived1Comparer,

            // Reverse
            () => ComparerBuilder.For<int>().Default().Reverse(),
            () => ComparerBuilder.For<int?>().Default().Reverse(),
            () => ComparerBuilder.For<int[]>().Default().Reverse(),
            () => ComparerBuilder.For<HierarchyBase>().OrderBy(x => x.Id, (IComparer<int>)null, false, false).Reverse(),
            () => ComparerBuilder.For<string>().Default().Reverse(),
            () => ComparerBuilder.For<object>().Default().Reverse(),
            () => ComparerBuilder.For<object>().OrderBy(x => (string)x, (IComparer<string>)null, false, false).Reverse()
        };

        public static readonly List<KeyValuePair<string, object>> ComparersExceptObject =
            Comparers.Where(x => ComparedType(x.Value as IComparer) != typeof(object)).ToList();

        public static readonly TheoryData<string> All = Comparers.Keys.ToTheoryData();


        [Theory]
        [MemberData(nameof(ReflexiveData))]
        public void SameInstance_IsZero(string comparerKey, Type comparedType)
        {
            var comparer = Comparers[comparerKey];
            var instance = Fake(comparedType);
            var compare = FindIComparerTCompare(comparer);
            Assert.Equal(0, compare(instance, instance));
        }
        public static readonly TheoryData<string, Type> ReflexiveData =
            Comparers
                .Select(x => (x.Key, ComparedType(x.Value as IComparer)))
                .Append((Key(() => HierarchyComparers.BaseComparer), typeof(HierarchyDerived1)))
                .ToTheoryData();

        [Theory]
        [MemberData(nameof(All))]
        public void BothParametersNull_IsZero(string comparerKey)
        {
            var comparer = Comparers[comparerKey];
            var compare = FindIComparerTCompare(comparer);
            Assert.Equal(0, compare(null, null));
        }

        [Theory]
        [MemberData(nameof(DifferentInstancesAndEqualData))]
        public void DifferentEquivalentInstances_IsZero(string comparerKey, object a, object b)
        {
            if (ReferenceEquals(a, b))
                throw new ArgumentException("Unit test error: objects must be different instances.");

            var comparer = Comparers[comparerKey];
            var compare = FindIComparerTCompare(comparer);
            Assert.Equal(0, compare(a, b));
            Assert.Equal(0, compare(b, a));
        }
        public static readonly TheoryData<string, object, object> DifferentInstancesAndEqualData = Comparers
            // Default comparer cannot test <HierarchyBase> or <object>, since they cannot have different instances that are equivalent.
            .Where(x => !(x.Key.StartsWith("Default") && (ComparedType(x.Value as IComparer) == typeof(object) || ComparedType(x.Value as IComparer) == typeof(HierarchyBase))))
            // Reference comparer cannot be tested here, since no reference comparer can have different instances that are equivalent.
            .Where(x => !x.Key.StartsWith("Reference"))
            .Select(x =>
            {
                var data = FakeEquivalent(ComparedType(x.Value as IComparer));
                return (x.Key, data.Item1, data.Item2);
            })
            .Append((Key(() => HierarchyComparers.BaseComparer), new HierarchyDerived1 { Id = 13 }, new HierarchyDerived2 { Id = 13 }))
            .ToTheoryData();

        [Theory]
        [MemberData(nameof(NotEqualData))]
        public void NonequivalentInstances_AreOrdered(string comparerKey, object smaller, object larger)
        {
            var comparer = Comparers[comparerKey];
            var compare = FindIComparerTCompare(comparer);
            if (comparerKey.StartsWith("Reverse"))
            {
                Assert.True(compare(smaller, larger) > 0);
                Assert.True(compare(larger, smaller) < 0);
            }
            else
            {
                Assert.True(compare(smaller, larger) < 0);
                Assert.True(compare(larger, smaller) > 0);
            }
        }
        public static readonly TheoryData<string, object, object> NotEqualData = Comparers
            // Null comparer cannot be tested here, since no two instances are not equal.
            .Where(x => !x.Key.StartsWith("Null"))
            .Select(x =>
            {
                var data = FakeDifferent(ComparedType(x.Value as IComparer));
                return (x.Key, data.Item1, data.Item2);
            })
            .Append((Key(() => HierarchyComparers.BaseComparer), new HierarchyDerived1 { Id = 7 }, new HierarchyDerived2 { Id = 13 }))
            .ToTheoryData();

        [Theory]
        [MemberData(nameof(NotEqualData))]
#pragma warning disable xUnit1026 // Theory methods should use all of their parameters
        public void OneInstanceNull_IsOrdered(string comparerKey, object smaller, object _)
#pragma warning restore xUnit1026 // Theory methods should use all of their parameters
        {
            var comparer = Comparers[comparerKey];
            var compare = FindIComparerTCompare(comparer);
            if (comparerKey.StartsWith("Reverse"))
            {
                Assert.True(compare(smaller, null) < 0);
                Assert.True(compare(null, smaller) > 0);
            }
            else
            {
                Assert.True(compare(smaller, null) > 0);
                Assert.True(compare(null, smaller) < 0);
            }
        }
    }
}
