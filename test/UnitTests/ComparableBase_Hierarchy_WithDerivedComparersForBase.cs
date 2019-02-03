using System;
using System.Collections.Generic;
using System.Text;
using Nito.Comparers;
using Nito.Comparers.Util;
using Xunit;
using static UnitTests.Util.ComparisonInvariantTests;
using static UnitTests.Util.EqualityInvariantTests;

namespace UnitTests
{
    public class ComparableBase_Hierarchy_WithDerivedComparersForBase
    {
        private abstract class Base : IComparable, IComparable<Base>
        {
            public int Id { get; set; }
            public abstract int CompareTo(object obj);
            public abstract int CompareTo(Base other);
        }

        private class Derived1 : Base, IComparable<Derived1>, IEquatable<Derived1>, IComparable
        {
            public static IFullComparer<Base> BaseComparer = ComparerBuilder.For<Base>().OrderBy(x => x.Id);
            public static IFullComparer<Derived1> DefaultComparer = BaseComparer;

            public int CompareTo(Derived1 other) => ComparableImplementations.ImplementCompareTo(DefaultComparer, this, other);
            public override int CompareTo(object obj) => ComparableImplementations.ImplementCompareTo(DefaultComparer, this, obj);
            public override int CompareTo(Base other) => ComparableImplementations.ImplementCompareTo(BaseComparer, this, other);
            public bool Equals(Derived1 other) => ComparableImplementations.ImplementEquals(DefaultComparer, this, other);
            public override bool Equals(object obj) => ComparableImplementations.ImplementEquals(DefaultComparer, this, obj);
            public override int GetHashCode() => ComparableImplementations.ImplementGetHashCode(DefaultComparer, this);
        }

        private class Derived2 : Base, IComparable<Derived2>, IEquatable<Derived2>, IComparable
        {
            public static IFullComparer<Base> BaseComparer = ComparerBuilder.For<Base>().OrderBy(x => x.Id);
            public static IFullComparer<Derived2> DefaultComparer = BaseComparer;

            public int CompareTo(Derived2 other) => ComparableImplementations.ImplementCompareTo(DefaultComparer, this, other);
            public override int CompareTo(object obj) => ComparableImplementations.ImplementCompareTo(DefaultComparer, this, obj);
            public override int CompareTo(Base other) => ComparableImplementations.ImplementCompareTo(BaseComparer, this, other);
            public bool Equals(Derived2 other) => ComparableImplementations.ImplementEquals(DefaultComparer, this, other);
            public override bool Equals(object obj) => ComparableImplementations.ImplementEquals(DefaultComparer, this, obj);
            public override int GetHashCode() => ComparableImplementations.ImplementGetHashCode(DefaultComparer, this);
        }

        private static readonly Derived1 Smallest = new Derived1 { Id = -1 };
        private static readonly Derived2 Middle = new Derived2 { Id = 0 };
        private static readonly Derived1 Largest = new Derived1 { Id = 1 };
        private static readonly Derived1 Largest2 = new Derived1 { Id = 1 };

        [Fact]
        public void Invariants()
        {
            AssertIComparableTCompareTo<Base>(Smallest, Middle, Largest);
            AssertIComparableCompareTo<Base>(Smallest, Middle, Largest);
            AssertIFullComparerT(Derived1.BaseComparer, Smallest, Middle, Largest, Largest2);
            AssertIFullComparerT(Derived2.BaseComparer, Smallest, Middle, Largest, Largest2);
        }
    }
}
