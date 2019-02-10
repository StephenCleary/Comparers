using System.Collections.Generic;
using Nito.Comparers;
using Xunit;
using static UnitTests.Util.EqualityInvariantTests;

namespace UnitTests
{
    public class EquatableBase_DefaultComparerUnitTests
    {
        private sealed class Person : EquatableBase<Person>
        {
            static Person()
            {
                DefaultComparer = EqualityComparerBuilder.For<Person>().EquateBy(p => p.LastName).ThenEquateBy(p => p.FirstName);
            }

            public string FirstName { get; set; }
            public string LastName { get; set; }
        }

        private static readonly Person AbeAbrams = new Person { FirstName = "Abe", LastName = "Abrams" };
        private static readonly Person AbeAbrams2 = new Person { FirstName = "Abe", LastName = "Abrams" };
        private static readonly Person JackAbrams = new Person { FirstName = "Jack", LastName = "Abrams" };

        [Fact]
        public void ImplementsComparerDefault()
        {
            var netDefault = EqualityComparer<Person>.Default;
            Assert.True(netDefault.Equals(AbeAbrams, AbeAbrams2));
            Assert.Equal(netDefault.GetHashCode(AbeAbrams), netDefault.GetHashCode(AbeAbrams2));
            Assert.False(netDefault.Equals(AbeAbrams, JackAbrams));
        }

        [Fact]
        public void ImplementsComparerDefault_NonGeneric()
        {
            object abe = AbeAbrams;
            Assert.True(abe.Equals(AbeAbrams2));
        }

        [Fact]
        public void Invariants()
        {
            AssertObjectEquals(AbeAbrams, JackAbrams, AbeAbrams2);
            AssertIEquatableTEquals(AbeAbrams, JackAbrams, AbeAbrams2);
        }
    }
}
