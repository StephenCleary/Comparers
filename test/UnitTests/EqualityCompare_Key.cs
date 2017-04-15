using System;
using Nito.Comparers;
using Xunit;
using System.Globalization;

#pragma warning disable CS0162

namespace UnitTests
{
    public class EqualityCompare_KeyUnitTests
    {
        private static readonly Person AbeAbrams = new Person { FirstName = "Abe", LastName = "Abrams" };
        private static readonly Person JackAbrams = new Person { FirstName = "Jack", LastName = "Abrams" };
        private static readonly Person Williamabrams = new Person { FirstName = "William", LastName = "abrams" };
        private static readonly Person CaseyJohnson = new Person { FirstName = "Casey", LastName = "Johnson" };
        private static readonly Person JackAbrams2 = new Person { FirstName = "Jack", LastName = "Abrams" };
        private static readonly Person jackAbrams = new Person { FirstName = "jack", LastName = "Abrams" };
        private static readonly Person Jacknull = new Person { FirstName = "Jack", LastName = null };

        [Fact]
        public void OrderByComparesByKey()
        {
            var comparer = EqualityComparerBuilder.For<Person>().EquateBy(p => p.LastName);
            var objectComparer = comparer as System.Collections.IEqualityComparer;
            Assert.True(comparer.Equals(AbeAbrams, JackAbrams));
            Assert.True(objectComparer.Equals(AbeAbrams, JackAbrams));
            Assert.Equal(comparer.GetHashCode(AbeAbrams), comparer.GetHashCode(JackAbrams));
            Assert.Equal(objectComparer.GetHashCode(AbeAbrams), objectComparer.GetHashCode(JackAbrams));
            Assert.False(comparer.Equals(AbeAbrams, CaseyJohnson));
            Assert.False(objectComparer.Equals(AbeAbrams, CaseyJohnson));
        }

        [Fact]
        public void OrderByUsesKeyComparer()
        {
#if NETCOREAPP1_1
            StringComparer invariantCultureComparerIgnoreCase = CultureInfo.InvariantCulture.CompareInfo.GetStringComparer(CompareOptions.IgnoreCase);
#else
            StringComparer invariantCultureComparerIgnoreCase = StringComparer.InvariantCultureIgnoreCase;
#endif
            var keyComparer = invariantCultureComparerIgnoreCase;
            var comparer = EqualityComparerBuilder.For<Person>().EquateBy(p => p.LastName, keyComparer);
            var objectComparer = comparer as System.Collections.IEqualityComparer;
            Assert.True(comparer.Equals(AbeAbrams, Williamabrams));
            Assert.True(objectComparer.Equals(AbeAbrams, Williamabrams));
            Assert.Equal(comparer.GetHashCode(AbeAbrams), comparer.GetHashCode(Williamabrams));
            Assert.Equal(objectComparer.GetHashCode(AbeAbrams), objectComparer.GetHashCode(Williamabrams));
            Assert.False(comparer.Equals(Williamabrams, CaseyJohnson));
            Assert.False(objectComparer.Equals(Williamabrams, CaseyJohnson));
            Assert.Equal(keyComparer.GetHashCode(Williamabrams.LastName) == keyComparer.GetHashCode(CaseyJohnson.LastName),
                comparer.GetHashCode(Williamabrams) == comparer.GetHashCode(CaseyJohnson));
        }

        [Fact]
        public void ThenBySortsByKey()
        {
            var comparer = EqualityComparerBuilder.For<Person>().EquateBy(p => p.LastName).ThenEquateBy(p => p.FirstName);
            var objectComparer = comparer as System.Collections.IEqualityComparer;
            Assert.True(comparer.Equals(JackAbrams, JackAbrams2));
            Assert.True(objectComparer.Equals(JackAbrams, JackAbrams2));
            Assert.Equal(comparer.GetHashCode(JackAbrams), comparer.GetHashCode(JackAbrams2));
            Assert.Equal(objectComparer.GetHashCode(JackAbrams), objectComparer.GetHashCode(JackAbrams2));
            Assert.False(comparer.Equals(AbeAbrams, JackAbrams));
            Assert.False(objectComparer.Equals(AbeAbrams, JackAbrams));
        }

        [Fact]
        public void ThenByUsesKeyComparer()
        {
#if NETCOREAPP1_1
            StringComparer invariantCultureComparerIgnoreCase = CultureInfo.InvariantCulture.CompareInfo.GetStringComparer(CompareOptions.IgnoreCase);
#else
            StringComparer invariantCultureComparerIgnoreCase = StringComparer.InvariantCultureIgnoreCase;
#endif
            var comparer = EqualityComparerBuilder.For<Person>().EquateBy(p => p.LastName).ThenEquateBy(p => p.FirstName, invariantCultureComparerIgnoreCase);
            var objectComparer = comparer as System.Collections.IEqualityComparer;
            Assert.True(comparer.Equals(JackAbrams, jackAbrams));
            Assert.True(objectComparer.Equals(JackAbrams, jackAbrams));
            Assert.Equal(comparer.GetHashCode(JackAbrams), comparer.GetHashCode(jackAbrams));
            Assert.Equal(objectComparer.GetHashCode(JackAbrams), objectComparer.GetHashCode(jackAbrams));
            Assert.False(comparer.Equals(AbeAbrams, JackAbrams));
            Assert.False(objectComparer.Equals(AbeAbrams, JackAbrams));
        }

        [Fact]
        public void OrderBySortsNullsAsNotEqualToValues()
        {
            var comparer = EqualityComparerBuilder.For<Person>().EquateBy(p => p.LastName);
            var objectComparer = comparer as System.Collections.IEqualityComparer;
            Assert.False(comparer.Equals(JackAbrams, null));
            Assert.False(objectComparer.Equals(JackAbrams, null));
            Assert.False(comparer.Equals(JackAbrams, Jacknull));
            Assert.False(objectComparer.Equals(JackAbrams, Jacknull));
        }

        [Fact]
        public void OrderByWithNullPassesNullThrough()
        {
            var comparer = EqualityComparerBuilder.For<Person>().EquateBy(p => 0, specialNullHandling:true);
            var objectComparer = comparer as System.Collections.IEqualityComparer;
            Assert.True(comparer.Equals(JackAbrams, null));
            Assert.True(objectComparer.Equals(JackAbrams, null));
            Assert.Equal(comparer.GetHashCode(JackAbrams), comparer.GetHashCode(null));
            Assert.Equal(objectComparer.GetHashCode(JackAbrams), objectComparer.GetHashCode(null));
        }

        [Fact]
        public void OrderByWithNullThenByHandlesNull()
        {
            var comparer = EqualityComparerBuilder.For<Person>().EquateBy(p => p == null, specialNullHandling: true).ThenEquateBy(p => p.LastName).ThenEquateBy(p =>
            {
                throw new Exception("ThenEquateBy comparer invoked.");
                return 0;
            });
            var objectComparer = comparer as System.Collections.IEqualityComparer;
            Assert.True(comparer.Equals(null, null));
            Assert.True(objectComparer.Equals(null, null));
            Assert.Equal(comparer.GetHashCode(null), comparer.GetHashCode(null));
            Assert.Equal(objectComparer.GetHashCode(null), objectComparer.GetHashCode(null));
        }

        [Fact]
        public void ToString_DumpsComparer()
        {
            Assert.Equal("Compound(Null, Select<String>(Default(String: IComparable<T>)))", EqualityComparerBuilder.For<Person>().EquateBy(p => p.LastName).ToString());
        }
    }
}
