using System;
using System.Collections.Generic;
using System.Linq;
using Nito.Comparers;
using Xunit;
using System.Globalization;

#pragma warning disable CS0162

namespace UnitTests
{
    public class Compare_KeyUnitTests
    {
        private static readonly Person AbeAbrams = new Person { FirstName = "Abe", LastName = "Abrams" };
        private static readonly Person JackAbrams = new Person { FirstName = "Jack", LastName = "Abrams" };
        private static readonly Person WilliamAbrams = new Person { FirstName = "William", LastName = "Abrams" };
        private static readonly Person CaseyJohnson = new Person { FirstName = "Casey", LastName = "Johnson" };
        private static readonly Person nullAbrams = new Person { FirstName = null, LastName = "Abrams" };

        [Fact]
        public void OrderBySortsByKey()
        {
            var list = new List<Person> { AbeAbrams, JackAbrams, WilliamAbrams, CaseyJohnson };
            list.Sort(ComparerBuilder.For<Person>().OrderBy(p => p.FirstName));
            Assert.Equal(new[] { AbeAbrams, CaseyJohnson, JackAbrams, WilliamAbrams }, list);
        }

        [Fact]
        public void OrderByUsesKeyComparer()
        {
            var list = new List<Person> { AbeAbrams, JackAbrams, WilliamAbrams, CaseyJohnson };
#if NETCOREAPP1_1
            StringComparer invariantCultureComparer = CultureInfo.InvariantCulture.CompareInfo.GetStringComparer(CompareOptions.None);
#else
            StringComparer invariantCultureComparer = StringComparer.InvariantCulture;
#endif
            list.Sort(ComparerBuilder.For<Person>().OrderBy(p => p.FirstName, invariantCultureComparer.Reverse()));
            Assert.Equal(new[] { WilliamAbrams, JackAbrams, CaseyJohnson, AbeAbrams }, list);
        }

        [Fact]
        public void OrderByDescendingSortsByKey()
        {
            var list = new List<Person> { AbeAbrams, JackAbrams, WilliamAbrams, CaseyJohnson };
            list.Sort(ComparerBuilder.For<Person>().OrderBy(p => p.FirstName, descending: true));
            Assert.Equal(new[] { WilliamAbrams, JackAbrams, CaseyJohnson, AbeAbrams }, list);
        }

        [Fact]
        public void OrderByDescendingUsesKeyComparer()
        {
            var list = new List<Person> { AbeAbrams, JackAbrams, WilliamAbrams, CaseyJohnson };
#if NETCOREAPP1_1
            StringComparer invariantCultureComparer = CultureInfo.InvariantCulture.CompareInfo.GetStringComparer(CompareOptions.None);
#else
            StringComparer invariantCultureComparer = StringComparer.InvariantCulture;
#endif
            list.Sort(ComparerBuilder.For<Person>().OrderBy(p => p.FirstName, invariantCultureComparer.Reverse(), descending: true));
            Assert.Equal(new[] { AbeAbrams, CaseyJohnson, JackAbrams, WilliamAbrams }, list);
        }

        [Fact]
        public void ThenBySortsByKey()
        {
            var list = new List<Person> { AbeAbrams, WilliamAbrams, CaseyJohnson, JackAbrams };
            list.Sort(ComparerBuilder.For<Person>().OrderBy(p => p.LastName).ThenBy(p => p.FirstName));
            Assert.Equal(new[] { AbeAbrams, JackAbrams, WilliamAbrams, CaseyJohnson }, list);
        }

        [Fact]
        public void ThenByUsesKeyComparer()
        {
            var list = new List<Person> { AbeAbrams, WilliamAbrams, CaseyJohnson, JackAbrams };
#if NETCOREAPP1_1
            StringComparer invariantCultureComparer = CultureInfo.InvariantCulture.CompareInfo.GetStringComparer(CompareOptions.None);
#else
            StringComparer invariantCultureComparer = StringComparer.InvariantCulture;
#endif
            list.Sort(ComparerBuilder.For<Person>().OrderBy(p => p.LastName).ThenBy(p => p.FirstName, invariantCultureComparer.Reverse()));
            Assert.Equal(new[] { WilliamAbrams, JackAbrams, AbeAbrams, CaseyJohnson }, list);
        }

        [Fact]
        public void ThenByDescendingSortsByKey()
        {
            var list = new List<Person> { AbeAbrams, WilliamAbrams, CaseyJohnson, JackAbrams };
            list.Sort(ComparerBuilder.For<Person>().OrderBy(p => p.LastName).ThenBy(p => p.FirstName, descending: true));
            Assert.Equal(new[] { WilliamAbrams, JackAbrams, AbeAbrams, CaseyJohnson }, list);
        }

        [Fact]
        public void ThenByDescendingUsesKeyComparer()
        {
            var list = new List<Person> { AbeAbrams, WilliamAbrams, CaseyJohnson, JackAbrams };
#if NETCOREAPP1_1
            StringComparer invariantCultureComparer = CultureInfo.InvariantCulture.CompareInfo.GetStringComparer(CompareOptions.None);
#else
            StringComparer invariantCultureComparer = StringComparer.InvariantCulture;
#endif
            list.Sort(ComparerBuilder.For<Person>().OrderBy(p => p.LastName).ThenBy(p => p.FirstName, invariantCultureComparer.Reverse(), descending: true));
            Assert.Equal(new[] { AbeAbrams, JackAbrams, WilliamAbrams, CaseyJohnson }, list);
        }

        [Fact]
        public void OrderBySortsNullsAsLowest()
        {
            var list = new List<Person> { AbeAbrams, JackAbrams, null, WilliamAbrams, nullAbrams, CaseyJohnson };
            list.Sort(ComparerBuilder.For<Person>().OrderBy(p => p.FirstName));
            Assert.Equal(new[] { null, nullAbrams, AbeAbrams, CaseyJohnson, JackAbrams, WilliamAbrams }, list);
        }

        [Fact]
        public void OrderByWithNullPassesNullThrough()
        {
            var list = new List<Person> { null, WilliamAbrams };
            list.Sort(ComparerBuilder.For<Person>().OrderBy(p => p == null, specialNullHandling: true));
            Assert.Equal(new[] { WilliamAbrams, null }, list);
        }

        [Fact]
        public void OrderByWithNullThenByHandlesNull()
        {
            var list = new List<Person> { AbeAbrams, JackAbrams, null, WilliamAbrams, CaseyJohnson };
            list.Sort(ComparerBuilder.For<Person>().OrderBy(p => p == null, specialNullHandling: true).ThenBy(p => p.FirstName));
            Assert.Equal(new[] { AbeAbrams, CaseyJohnson, JackAbrams, WilliamAbrams, null }, list);
        }

        [Fact]
        public void OrderByWithNullThenByComparesNullsAsEqual()
        {
            var comparer = ComparerBuilder.For<Person>().OrderBy(p => p == null, specialNullHandling: true).ThenBy(p => p.FirstName).ThenBy(p =>
            {
                throw new Exception("Should not be called");
                return 0;
            });
            Assert.True(comparer.Compare(null, null) == 0);
            Assert.True(comparer.Equals(null, null));
            Assert.Equal(comparer.GetHashCode((object)null), comparer.GetHashCode((object)null));
            Assert.Equal(comparer.GetHashCode((Person)null), comparer.GetHashCode((Person)null));
        }

        [Fact]
        public void OrderByImplementsGetHashCode()
        {
            var comparer = ComparerBuilder.For<Person>().OrderBy(p => p.LastName);
            var bclDefault = EqualityComparer<string>.Default;
            Assert.Equal(comparer.GetHashCode(AbeAbrams), comparer.GetHashCode(JackAbrams));
            Assert.Equal(bclDefault.GetHashCode(AbeAbrams.LastName) == bclDefault.GetHashCode(CaseyJohnson.LastName),
                comparer.GetHashCode(AbeAbrams) == comparer.GetHashCode(CaseyJohnson));
        }

        [Fact]
        public void OrderByEnumerableUsesDefaultSequenceComparison()
        {
            var list = new List<Person> { AbeAbrams, JackAbrams, WilliamAbrams, CaseyJohnson };
            var comparer = ComparerBuilder.For<Person>().OrderBy(p => p.FirstName.SelectMany(x => new[] { x }));
            list.Sort(comparer);
            Assert.Equal(new[] { AbeAbrams, CaseyJohnson, JackAbrams, WilliamAbrams }, list);
        }

        [Fact]
        public void ToString_DumpsComparer()
        {
            Assert.Equal("Compound(Null, Select<String>(Default(String: IComparable<T>)))", ComparerBuilder.For<Person>().OrderBy(p => p.LastName).ToString());
        }
    }
}
