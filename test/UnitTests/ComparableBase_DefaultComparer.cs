using System.Collections.Generic;
using System.Linq;
using Nito.Comparers;
using Xunit;

namespace UnitTests
{
    public class ComparableBase_DefaultComparerUnitTests
    {
        private sealed class Person : ComparableBase<Person>
        {
            static Person()
            {
                DefaultComparer = ComparerBuilder.For<Person>().OrderBy(p => p.LastName).ThenBy(p => p.FirstName);
            }

            public string FirstName { get; set; }
            public string LastName { get; set; }
        }

        private static readonly Person AbeAbrams = new Person { FirstName = "Abe", LastName = "Abrams" };
        private static readonly Person JackAbrams = new Person { FirstName = "Jack", LastName = "Abrams" };
        private static readonly Person WilliamAbrams = new Person { FirstName = "William", LastName = "Abrams" };
        private static readonly Person CaseyJohnson = new Person { FirstName = "Casey", LastName = "Johnson" };

        [Fact]
        public void ImplementsComparerDefault()
        {
            var list = new List<Person> { JackAbrams, CaseyJohnson, AbeAbrams, WilliamAbrams };
            list.Sort();
            Assert.Equal(new[] { AbeAbrams, JackAbrams, WilliamAbrams, CaseyJohnson }, list);
        }

        [Fact]
        public void ImplementsComparerDefault_Hash()
        {
            var set = new HashSet<Person> { JackAbrams, CaseyJohnson, AbeAbrams };
            Assert.Contains(new Person { FirstName = AbeAbrams.FirstName, LastName = AbeAbrams.LastName }, set);
            Assert.DoesNotContain(WilliamAbrams, set);
        }

        [Fact]
        public void ImplementsComparerDefault_NonGeneric()
        {
            var set = new System.Collections.ArrayList() { JackAbrams, CaseyJohnson, AbeAbrams };
            Assert.True(set.Contains(new Person { FirstName = AbeAbrams.FirstName, LastName = AbeAbrams.LastName }));
            Assert.False(set.Contains(WilliamAbrams));
        }
    }
}
