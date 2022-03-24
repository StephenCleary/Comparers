﻿using System;
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
            () => EqualityComparerBuilder.For<int>().EquateBy(x => x % 17, (IEqualityComparer<int>)null, false),
            () => EqualityComparerBuilder.For<int?>().EquateBy(x => x % 17, (IEqualityComparer<int?>)null, false),
            () => EqualityComparerBuilder.For<int[]>().EquateBy(x => x[0], (IEqualityComparer<int>)null, false),
            () => EqualityComparerBuilder.For<HierarchyBase>().EquateBy(x => x.Id % 17, (IEqualityComparer<int>)null, false),
            () => EqualityComparerBuilder.For<string>().EquateBy(x => x.ToLowerInvariant(), (IEqualityComparer<string>)null, false),
            () => EqualityComparerBuilder.For<object>().EquateBy(x => (string)x, (IEqualityComparer<string>)null, false),
            () => ComparerBuilder.For<int>().OrderBy(x => x % 17, (IComparer<int>)null, false, false),
            () => ComparerBuilder.For<int?>().OrderBy(x => x % 17, (IComparer<int?>)null, false, false),
            () => ComparerBuilder.For<int[]>().OrderBy(x => x[0], (IComparer<int>)null, false, false),
            () => ComparerBuilder.For<HierarchyBase>().OrderBy(x => x.Id % 17, (IComparer<int>)null, false, false),
            () => ComparerBuilder.For<string>().OrderBy(x => x.ToLowerInvariant(), (IComparer<string>)null, false, false),
            () => ComparerBuilder.For<object>().OrderBy(x => (string)x, (IComparer<string>)null, false, false),

            // Sequence
            () => EqualityComparerBuilder.For<int>().Default().EquateSequence(),
            () => EqualityComparerBuilder.For<int?>().Default().EquateSequence(),
            () => EqualityComparerBuilder.For<int[]>().Default().EquateSequence(),
            () => EqualityComparerBuilder.For<HierarchyBase>().EquateBy(x => x.Id, (IEqualityComparer<int>)null, false).EquateSequence(),
            () => EqualityComparerBuilder.For<string>().Default().EquateSequence(),
            () => EqualityComparerBuilder.For<object>().Default().EquateSequence(),
            () => EqualityComparerBuilder.For<int>().Default().EquateSequence(true),
            () => EqualityComparerBuilder.For<int?>().Default().EquateSequence(true),
            () => EqualityComparerBuilder.For<int[]>().Default().EquateSequence(true),
            () => EqualityComparerBuilder.For<HierarchyBase>().EquateBy(x => x.Id, (IEqualityComparer<int>)null, false).EquateSequence(true),
            () => EqualityComparerBuilder.For<string>().Default().EquateSequence(true),
            () => EqualityComparerBuilder.For<object>().Default().EquateSequence(true),
            () => ComparerBuilder.For<int>().Default().Sequence(),
            () => ComparerBuilder.For<int?>().Default().Sequence(),
            () => ComparerBuilder.For<int[]>().Default().Sequence(),
            () => ComparerBuilder.For<HierarchyBase>().OrderBy(x => x.Id, (IComparer<int>)null, false, false).Sequence(),
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
            () => ComparerBuilder.For<HierarchyBase>().OrderBy(x => x.Id, (IComparer<int>)null, false, false).Reverse(),
            () => ComparerBuilder.For<string>().Default().Reverse(),
            () => ComparerBuilder.For<object>().Default().Reverse(),
            () => ComparerBuilder.For<object>().OrderBy(x => (string)x, (IComparer<string>)null, false, false).Reverse(),

            // Natural String Comparers
            () => EqualityComparerBuilder.For<string>().Natural(StringComparison.Ordinal),
            () => EqualityComparerBuilder.For<string>().Natural(StringComparison.OrdinalIgnoreCase),
            () => EqualityComparerBuilder.For<string>().Natural(StringComparison.InvariantCulture),
            () => EqualityComparerBuilder.For<string>().Natural(StringComparison.InvariantCultureIgnoreCase),
            () => EqualityComparerBuilder.For<string>().Natural(StringComparison.CurrentCulture),
            () => EqualityComparerBuilder.For<string>().Natural(StringComparison.CurrentCultureIgnoreCase),
            () => ComparerBuilder.For<string>().Natural(StringComparison.Ordinal),
            () => ComparerBuilder.For<string>().Natural(StringComparison.OrdinalIgnoreCase),
            () => ComparerBuilder.For<string>().Natural(StringComparison.InvariantCulture),
            () => ComparerBuilder.For<string>().Natural(StringComparison.InvariantCultureIgnoreCase),
            () => ComparerBuilder.For<string>().Natural(StringComparison.CurrentCulture),
            () => ComparerBuilder.For<string>().Natural(StringComparison.CurrentCultureIgnoreCase),
        };

        public static readonly List<KeyValuePair<string, dynamic>> EqualityComparersExceptObject =
            EqualityComparers.Where(x => ComparedType(x.Value as IEqualityComparer) != typeof(object)).ToList();

        public static readonly TheoryData<string> All = EqualityComparers.Keys.ToTheoryData();


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
        public static readonly TheoryData<string, object, object> DifferentInstancesAndEqualData = EqualityComparers
            // Default comparer cannot test <HierarchyBase> or <object>, since they cannot have different instances that are equivalent.
            .Where(x => !(x.Key.StartsWith("Default") && (ComparedType(x.Value as IEqualityComparer) == typeof(object) || ComparedType(x.Value as IEqualityComparer) == typeof(HierarchyBase))))
            // Reference comparer cannot be tested here, since no reference comparer can have different instances that are equivalent.
            .Where(x => !x.Key.StartsWith("Reference"))
            .Select(x =>
            {
                var data = FakeEquivalent(ComparedType(x.Value as IEqualityComparer));
                return (x.Key, data.Item1, data.Item2);
            })
            .Append((Key(() => HierarchyComparers.BaseEqualityComparer), new HierarchyDerived1 { Id = 13 }, new HierarchyDerived2 { Id = 13 }))
            .Append((Key(() => HierarchyComparers.BaseComparer), new HierarchyDerived1 { Id = 13 }, new HierarchyDerived2 { Id = 13 }))
            .ToTheoryData();

        [Theory]
        [MemberData(nameof(NotEqualData))]
        public void Equals_NonequivalentInstances_ReturnsFalse(string comparerKey, object a, object b)
        {
            var comparer = EqualityComparers[comparerKey];
            var equals = FindIComparerTEquals(comparer);
            Assert.False(equals(a, b));
            Assert.False(equals(b, a));
        }
        public static readonly TheoryData<string, object, object> NotEqualData = EqualityComparers
            // Null comparer cannot be tested here, since no two instances are not equal.
            .Where(x => !x.Key.StartsWith("Null"))
            .Select(x =>
            {
                var data = FakeDifferent(ComparedType(x.Value as IEqualityComparer));
                return (x.Key, data.Item1, data.Item2);
            })
            .Append((Key(() => HierarchyComparers.BaseEqualityComparer), new HierarchyDerived1 { Id = 7 }, new HierarchyDerived2 { Id = 13 }))
            .Append((Key(() => HierarchyComparers.BaseComparer), new HierarchyDerived1 { Id = 7 }, new HierarchyDerived2 { Id = 13 }))
            .ToTheoryData();

        [Theory]
        [MemberData(nameof(NotEqualData))]
#pragma warning disable xUnit1026 // Theory methods should use all of their parameters
        public void Equals_OneInstanceNull_ReturnsFalse(string comparerKey, object a, object _)
#pragma warning restore xUnit1026 // Theory methods should use all of their parameters
        {
            var comparer = EqualityComparers[comparerKey];
            var equals = FindIComparerTEquals(comparer);
            Assert.False(equals(a, null));
            Assert.False(equals(null, a));
        }
    }
}
