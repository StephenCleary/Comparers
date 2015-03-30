using System.Collections.Generic;
using Nito.Comparers;
using Xunit;

namespace EquatableBase_
{
    public class _Operators
    {
        private sealed class Person : EquatableBaseWithOperators<Person>
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
        private static readonly Person WilliamAbrams = new Person { FirstName = "William", LastName = "Abrams" };
        private static readonly Person CaseyJohnson = new Person { FirstName = "Casey", LastName = "Johnson" };

        [Fact]
        public void ImplementsComparerDefault()
        {
            var netDefault = EqualityComparer<Person>.Default;
            Assert.True(netDefault.Equals(AbeAbrams, AbeAbrams2));
            Assert.Equal(netDefault.GetHashCode(AbeAbrams), netDefault.GetHashCode(AbeAbrams2));
            Assert.False(netDefault.Equals(AbeAbrams, JackAbrams));
        }

        [Fact]
        public void ImplementsOpEquality()
        {
            Assert.True(AbeAbrams == AbeAbrams2);
            Assert.False(AbeAbrams == JackAbrams);
        }

        [Fact]
        public void ImplementsOpInequality()
        {
            Assert.False(AbeAbrams != AbeAbrams2);
            Assert.True(AbeAbrams != JackAbrams);
        }
    }
}
