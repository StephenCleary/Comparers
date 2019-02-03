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
    public class EquatableBase_Hierarchy_WithDerivedComparers
    {
        private abstract class Base
        {
            public int Id { get; set; }
        }

        private class Derived1 : Base, IEquatable<Derived1>
        {
            public static IFullEqualityComparer<Derived1> DefaultComparer = EqualityComparerBuilder.For<Derived1>().EquateBy(x => x.Id);

            public bool Equals(Derived1 other) => ComparableImplementations.ImplementEquals(DefaultComparer, this, other);
            public override bool Equals(object obj) => ComparableImplementations.ImplementEquals(DefaultComparer, this, obj);
            public override int GetHashCode() => ComparableImplementations.ImplementGetHashCode(DefaultComparer, this);
        }

        private class Derived2 : Base, IEquatable<Derived2>
        {
            public static IFullEqualityComparer<Derived2> DefaultComparer = EqualityComparerBuilder.For<Derived2>().EquateBy(x => x.Id);

            public bool Equals(Derived2 other) => ComparableImplementations.ImplementEquals(DefaultComparer, this, other);
            public override bool Equals(object obj) => ComparableImplementations.ImplementEquals(DefaultComparer, this, obj);
            public override int GetHashCode() => ComparableImplementations.ImplementGetHashCode(DefaultComparer, this);
        }

        private static readonly Derived1 Smallest = new Derived1 { Id = -1 };
        private static readonly Derived2 Middle = new Derived2 { Id = 0 };
        private static readonly Derived2 Middle2 = new Derived2 { Id = 0 };
        private static readonly Derived1 Largest = new Derived1 { Id = 1 };
        private static readonly Derived1 Largest2 = new Derived1 { Id = 1 };

        [Fact]
        public void ObjectEquality()
        {
            AssertObjectEquals(Largest, Middle, Largest2);
            AssertIEqualityComparer(Derived1.DefaultComparer, Largest, Middle, Largest2);
            AssertIEqualityComparer(Derived2.DefaultComparer, Middle, Largest, Middle2);
        }
    }
}
