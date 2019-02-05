using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Nito.Comparers;
using Xunit;
using UnitTests.Util;
using static UnitTests.Util.DataUtility;
// ReSharper disable UseStringInterpolation

namespace UnitTests
{
    public class IEqualityComparer_Equals
    {
        [Theory]
        [MemberData(nameof(ReflexiveData))]
        public void Reflexive(IEqualityComparer comparer, object a)
        {
            Assert.True(comparer.Equals(a, a));
        }
        public static readonly TheoryData<IEqualityComparer, object> ReflexiveData = new TheoryData<IEqualityComparer, object>
        {
            { EqualityComparerBuilder.For<int>().Default(), 13 },
            { EqualityComparerBuilder.For<int>().Default(), null },
            { EqualityComparerBuilder.For<int?>().Default(), 13 },
            { EqualityComparerBuilder.For<int?>().Default(), null },
            { EqualityComparerBuilder.For<int[]>().Default(), new[] { 13 } },
            { EqualityComparerBuilder.For<int[]>().Default(), null },
            { EqualityComparerBuilder.For<string>().Default(), "test" },
            { EqualityComparerBuilder.For<string>().Default(), null },
            { HierarchyComparers.BaseEqualityComparer, new HierarchyBase { Id = 13 } },
            { HierarchyComparers.BaseEqualityComparer, new HierarchyDerived1 { Id = 13 } },
            { HierarchyComparers.BaseEqualityComparer, null },
            { HierarchyComparers.Derived1EqualityComparer, new HierarchyDerived1 { Id = 13 } },
            { EqualityComparer<int>.Default, new HierarchyBase { Id = 13 } }, // returns true due to reference equality check: https://github.com/dotnet/corefx/blob/53a33cf2662ac8c9a45d13067012d80cf0ba6956/src/Common/src/CoreLib/System/Collections/Generic/EqualityComparer.cs#L29
        };

        [Theory]
        [MemberData(nameof(DifferentInstancesAndEqualData))]
        public void DifferentInstancesAndEqual(IEqualityComparer comparer, object a, object b)
        {
            if (object.ReferenceEquals(a, b))
                throw new ArgumentException("Unit test error: objects must be different instances.");

            Assert.True(comparer.Equals(a, b));
            Assert.True(comparer.Equals(b, a));
        }
        public static readonly TheoryData<IEqualityComparer, object, object> DifferentInstancesAndEqualData = new TheoryData<IEqualityComparer, object, object>
        {
            { EqualityComparerBuilder.For<int>().Default(), 13, 13 },
            { EqualityComparerBuilder.For<int?>().Default(), 13, 13 },
            { EqualityComparerBuilder.For<int[]>().Default(), new[] { 13 }, new[] { 13 } },
            { EqualityComparerBuilder.For<string>().Default(), "test", Duplicate("test") },
            { HierarchyComparers.BaseEqualityComparer, new HierarchyBase { Id = 13 }, new HierarchyBase { Id = 13 } },
            { HierarchyComparers.BaseEqualityComparer, new HierarchyDerived1 { Id = 13 }, new HierarchyDerived2 { Id = 13 } },
        };

        [Theory]
        [MemberData(nameof(ThrowsData))]
        public void Throws(IEqualityComparer comparer, object a, object b)
        {
            Assert.ThrowsAny<ArgumentException>(() => comparer.Equals(a, b));
        }
        public static readonly TheoryData<IEqualityComparer, object, object> ThrowsData = new TheoryData<IEqualityComparer, object, object>
        {
            { EqualityComparer<int>.Default, new HierarchyBase { Id = 13 }, new HierarchyBase { Id = 13 } },
            { HierarchyComparers.Derived1EqualityComparer, new HierarchyBase { Id = 13 }, new HierarchyBase { Id = 13 } },
        };
    }
}
