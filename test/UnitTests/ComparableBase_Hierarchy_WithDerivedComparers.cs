using System;
using System.Collections.Generic;
using System.Text;
using Nito.Comparers;
using Nito.Comparers.Util;
using Xunit;

namespace UnitTests
{
    public class ComparableBase_Hierarchy_WithDerivedComparers
    {
        private abstract class Base : IComparable
        {
            public int Id { get; set; }
            public abstract int CompareTo(object obj);
        }

        private class Derived1 : Base, IComparable<Derived1>, IEquatable<Derived1>, IComparable
        {
            public static IFullComparer<Derived1> DefaultComparer = ComparerBuilder.For<Derived1>().OrderBy(x => x.Id);

            public int CompareTo(Derived1 other) => ComparableImplementations.ImplementCompareTo(DefaultComparer, this, other);
            public override int CompareTo(object obj) => ComparableImplementations.ImplementCompareTo(DefaultComparer, this, obj);
            public bool Equals(Derived1 other) => ComparableImplementations.ImplementEquals(DefaultComparer, this, other);
            public override bool Equals(object obj) => ComparableImplementations.ImplementEquals(DefaultComparer, this, obj);
            public override int GetHashCode() => ComparableImplementations.ImplementGetHashCode(DefaultComparer, this);
        }

        private class Derived2 : Base, IComparable<Derived2>, IEquatable<Derived2>, IComparable
        {
            public static IFullComparer<Derived2> DefaultComparer = ComparerBuilder.For<Derived2>().OrderBy(x => x.Id);

            public int CompareTo(Derived2 other) => ComparableImplementations.ImplementCompareTo(DefaultComparer, this, other);
            public override int CompareTo(object obj) => ComparableImplementations.ImplementCompareTo(DefaultComparer, this, obj);
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
        public void IComparableCompareTo()
        {
            int compare(Base a, Base b) => a.CompareTo(b);
            Assert.Equal(0, compare(Smallest, Smallest));
            Assert.Equal(0, compare(Middle, Middle));
            Assert.Equal(0, compare(Largest, Largest));
            Assert.Throws<ArgumentException>(() => compare(Smallest, Middle));
            Assert.Throws<ArgumentException>(() => compare(Middle, Largest));
            Assert.True(compare(Smallest, null) > 0);
            Assert.True(compare(Middle, null) > 0);
            Assert.True(compare(Largest, null) > 0);
        }

        [Fact]
        public void Derived1_IComparerCompare()
        {
            int compare(Base a, Base b) => Derived1.DefaultComparer.Compare(a, b);
            Assert.Equal(0, compare(Smallest, Smallest));
            Assert.Throws<ArgumentException>(() => compare(Middle, Middle));
            Assert.Equal(0, compare(Largest, Largest));
            Assert.Throws<ArgumentException>(() => compare(Smallest, Middle));
            Assert.Throws<ArgumentException>(() => compare(Middle, Largest));
            Assert.True(compare(Smallest, null) > 0);
            Assert.Throws<ArgumentException>(() => compare(Middle, null));
            Assert.True(compare(Largest, null) > 0);
            Assert.True(compare(null, Smallest) < 0);
            Assert.Throws<ArgumentException>(() => compare(null, Middle));
            Assert.True(compare(null, Largest) < 0);
        }

        [Fact]
        public void Derived2_IComparerCompare()
        {
            int compare(Base a, Base b) => Derived2.DefaultComparer.Compare(a, b);
            Assert.Throws<ArgumentException>(() => compare(Smallest, Smallest));
            Assert.Equal(0, compare(Middle, Middle));
            Assert.Throws<ArgumentException>(() => compare(Largest, Largest));
            Assert.Throws<ArgumentException>(() => compare(Smallest, Middle));
            Assert.Throws<ArgumentException>(() => compare(Middle, Largest));
            Assert.Throws<ArgumentException>(() => compare(Smallest, null) > 0);
            Assert.True(compare(Middle, null) > 0);
            Assert.Throws<ArgumentException>(() => compare(Largest, null));
            Assert.Throws<ArgumentException>(() => compare(null, Smallest));
            Assert.True(compare(null, Middle) < 0);
            Assert.Throws<ArgumentException>(() => compare(null, Largest));
        }
    }
}
