using System.Collections.Generic;
using Nito.Comparers;
using Xunit;

namespace EqualityComparerExtensions_
{
    public class _ThenEquateBy
    {
        private sealed class Person : EquatableBase<Person>
        {
            static Person()
            {
                DefaultComparer = EqualityComparerBuilder.For<Person>().EquateBy(p => p.LastName);
            }

            public string FirstName { get; set; }
            public string LastName { get; set; }
        }

        private static readonly Person AbeAbrams = new Person { FirstName = "Abe", LastName = "Abrams" };
        private static readonly Person JackAbrams = new Person { FirstName = "Jack", LastName = "Abrams" };
        private static readonly Person JackAbrams2 = new Person { FirstName = "Jack", LastName = "Abrams" };
        private static readonly Person WilliamAbrams = new Person { FirstName = "William", LastName = "Abrams" };
        private static readonly Person CaseyJohnson = new Person { FirstName = "Casey", LastName = "Johnson" };

        [Fact]
        public void SubstitutesCompareDefaultForComparerDefault()
        {
            IEqualityComparer<Person> thenByComparer = EqualityComparerBuilder.For<Person>().EquateBy(p => p.FirstName);
            var comparer = EqualityComparer<Person>.Default.ThenEquateBy(thenByComparer);
            Assert.Equal(EqualityComparerBuilder.For<Person>().Default().ThenEquateBy(thenByComparer).ToString(), comparer.ToString());
        }

        [Fact]
        public void SubstitutesCompareDefaultForNull()
        {
            IEqualityComparer<Person> thenByComparer = EqualityComparerBuilder.For<Person>().EquateBy(p => p.FirstName);
            IEqualityComparer<Person> source = null;
            var comparer = source.ThenEquateBy(thenByComparer);
            Assert.Equal(EqualityComparerBuilder.For<Person>().Default().ThenEquateBy(thenByComparer).ToString(), comparer.ToString());
        }

        [Fact]
        public void ThenByUsesComparer()
        {
            IEqualityComparer<Person> thenByComparer = EqualityComparerBuilder.For<Person>().EquateBy(p => p.FirstName);
            var comparer = EqualityComparerBuilder.For<Person>().Default().ThenEquateBy(thenByComparer);
            var objectComparer = comparer as System.Collections.IEqualityComparer;
            Assert.False(comparer.Equals(AbeAbrams, JackAbrams));
            Assert.False(objectComparer.Equals(AbeAbrams, JackAbrams));
            Assert.True(comparer.Equals(JackAbrams, JackAbrams2));
            Assert.True(objectComparer.Equals(JackAbrams, JackAbrams2));
            Assert.Equal(comparer.GetHashCode(JackAbrams), comparer.GetHashCode(JackAbrams2));
            Assert.Equal(objectComparer.GetHashCode(JackAbrams), objectComparer.GetHashCode(JackAbrams2));
        }

        [Fact]
        public void ThenByIsAppliedAsTieBreaker()
        {
            IEqualityComparer<Person> thenByComparer = EqualityComparerBuilder.For<Person>().EquateBy(p => p.FirstName);
            IEqualityComparer<Person> defaultComparer = EqualityComparerBuilder.For<Person>().Default();
            IEqualityComparer<Person> fullComparer = defaultComparer.ThenEquateBy(thenByComparer);
            Assert.True(defaultComparer.Equals(AbeAbrams, WilliamAbrams));
            Assert.Equal(defaultComparer.GetHashCode(AbeAbrams), defaultComparer.GetHashCode(WilliamAbrams));
            Assert.False(thenByComparer.Equals(AbeAbrams, WilliamAbrams));
            Assert.False(fullComparer.Equals(AbeAbrams, WilliamAbrams));
        }

        [Fact]
        public void ThenByIsOnlyAppliedAsTieBreaker()
        {
            IEqualityComparer<Person> thenByComparer = new FailComparer<Person>();
            var comparer = EqualityComparerBuilder.For<Person>().Default().ThenEquateBy(thenByComparer);
            Assert.False(comparer.Equals(AbeAbrams, CaseyJohnson));
            Assert.False(comparer.Equals(CaseyJohnson, AbeAbrams));
        }

        [Fact]
        public void ToString_DumpsComparer()
        {
            Assert.Equal("Compound(Default(Person: undefined), Select<String>(Default(String: IComparable<T>)))", EqualityComparerBuilder.For<Person>().Default().ThenEquateBy(p => p.LastName).ToString());
        }

        // The delegate overloads are tested by EqualityCompare_._Key.
    }
}
