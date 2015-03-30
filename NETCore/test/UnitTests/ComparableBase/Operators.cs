using System.Collections.Generic;
using System.Linq;
using Nito.Comparers;
using Xunit;

namespace ComparableBase_
{
    public class _Operators
    {
        private sealed class Person : ComparableBaseWithOperators<Person>
        {
            static Person()
            {
                DefaultComparer = ComparerBuilder.For<Person>().OrderBy(p => p.LastName).ThenBy(p => p.FirstName);
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
            var list = new List<Person> { JackAbrams, CaseyJohnson, AbeAbrams, WilliamAbrams };
            list.Sort();
            Assert.Equal(new[] { AbeAbrams, JackAbrams, WilliamAbrams, CaseyJohnson }, list);
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

        [Fact]
        public void ImplementsOpLessThan()
        {
            Assert.False(AbeAbrams < AbeAbrams2);
            Assert.True(AbeAbrams < JackAbrams);
            Assert.False(JackAbrams < AbeAbrams);
            Assert.True(AbeAbrams < CaseyJohnson);
            Assert.False(CaseyJohnson < AbeAbrams);
        }

        [Fact]
        public void ImplementsOpLessThanOrEqual()
        {
            Assert.True(AbeAbrams <= AbeAbrams2);
            Assert.True(AbeAbrams <= JackAbrams);
            Assert.False(JackAbrams <= AbeAbrams);
            Assert.True(AbeAbrams <= CaseyJohnson);
            Assert.False(CaseyJohnson <= AbeAbrams);
        }

        [Fact]
        public void ImplementsOpGreaterThan()
        {
            Assert.False(AbeAbrams > AbeAbrams2);
            Assert.False(AbeAbrams > JackAbrams);
            Assert.True(JackAbrams > AbeAbrams);
            Assert.False(AbeAbrams > CaseyJohnson);
            Assert.True(CaseyJohnson > AbeAbrams);
        }

        [Fact]
        public void ImplementsOpGreaterThanOrEqual()
        {
            Assert.True(AbeAbrams >= AbeAbrams2);
            Assert.False(AbeAbrams >= JackAbrams);
            Assert.True(JackAbrams >= AbeAbrams);
            Assert.False(AbeAbrams >= CaseyJohnson);
            Assert.True(CaseyJohnson >= AbeAbrams);
        }
    }
}
