using System;
using System.Collections.Generic;
using System.Text;
using Nito.Comparers;
using Xunit;
using static UnitTests.Util.ComparisonInvariantTests;

namespace UnitTests
{
    public class ComparableBase_Hierarchy_WithBaseComparer
    {
        private class Base : ComparableBase<Base>
        {
            static Base()
            {
                DefaultComparer = ComparerBuilder.For<Base>().OrderBy(x => x.Id);
            }

            public int Id { get; set; }
        }

        private class Derived1 : Base
        {
        }

        private class Derived2 : Base
        {
        }

        private static readonly Derived1 Smallest = new Derived1 { Id = -1 };
        private static readonly Base Middle = new Base { Id = 0 };
        private static readonly Derived2 Largest = new Derived2 { Id = 1 };
        private static readonly Derived2 Largest2 = new Derived2 { Id = 1 };

        [Fact]
        public void Invariants()
        {
            AssertIComparableTCompareTo(Smallest, Middle, Largest);
            AssertIComparableCompareTo(Smallest, Middle, Largest);
        }
    }
}
