using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Comparers;
using EqualityComparers;

namespace Compare_
{
    [TestClass]
    public class _Key
    {
        private static readonly Person AbeAbrams = new Person { FirstName = "Abe", LastName = "Abrams" };
        private static readonly Person JackAbrams = new Person { FirstName = "Jack", LastName = "Abrams" };
        private static readonly Person WilliamAbrams = new Person { FirstName = "William", LastName = "Abrams" };
        private static readonly Person CaseyJohnson = new Person { FirstName = "Casey", LastName = "Johnson" };
        private static readonly Person nullAbrams = new Person { FirstName = null, LastName = "Abrams" };

        [TestMethod]
        public void OrderBySortsByKey()
        {
            var list = new List<Person> { AbeAbrams, JackAbrams, WilliamAbrams, CaseyJohnson };
            list.Sort(Compare<Person>.OrderBy(p => p.FirstName));
            CollectionAssert.AreEqual(new[] { AbeAbrams, CaseyJohnson, JackAbrams, WilliamAbrams }, list);
        }

        [TestMethod]
        public void OrderByUsesKeyComparer()
        {
            var list = new List<Person> { AbeAbrams, JackAbrams, WilliamAbrams, CaseyJohnson };
            list.Sort(Compare<Person>.OrderBy(p => p.FirstName, StringComparer.InvariantCulture.Reverse()));
            CollectionAssert.AreEqual(new[] { WilliamAbrams, JackAbrams, CaseyJohnson, AbeAbrams }, list);
        }

        [TestMethod]
        public void OrderByDescendingSortsByKey()
        {
            var list = new List<Person> { AbeAbrams, JackAbrams, WilliamAbrams, CaseyJohnson };
            list.Sort(Compare<Person>.OrderByDescending(p => p.FirstName));
            CollectionAssert.AreEqual(new[] { WilliamAbrams, JackAbrams, CaseyJohnson, AbeAbrams }, list);
        }

        [TestMethod]
        public void OrderByDescendingUsesKeyComparer()
        {
            var list = new List<Person> { AbeAbrams, JackAbrams, WilliamAbrams, CaseyJohnson };
            list.Sort(Compare<Person>.OrderByDescending(p => p.FirstName, StringComparer.InvariantCulture.Reverse()));
            CollectionAssert.AreEqual(new[] { AbeAbrams, CaseyJohnson, JackAbrams, WilliamAbrams }, list);
        }

        [TestMethod]
        public void ThenBySortsByKey()
        {
            var list = new List<Person> { AbeAbrams, WilliamAbrams, CaseyJohnson, JackAbrams };
            list.Sort(Compare<Person>.OrderBy(p => p.LastName).ThenBy(p => p.FirstName));
            CollectionAssert.AreEqual(new[] { AbeAbrams, JackAbrams, WilliamAbrams, CaseyJohnson }, list);
        }

        [TestMethod]
        public void ThenByUsesKeyComparer()
        {
            var list = new List<Person> { AbeAbrams, WilliamAbrams, CaseyJohnson, JackAbrams };
            list.Sort(Compare<Person>.OrderBy(p => p.LastName).ThenBy(p => p.FirstName, StringComparer.InvariantCulture.Reverse()));
            CollectionAssert.AreEqual(new[] { WilliamAbrams, JackAbrams, AbeAbrams, CaseyJohnson }, list);
        }

        [TestMethod]
        public void ThenByDescendingSortsByKey()
        {
            var list = new List<Person> { AbeAbrams, WilliamAbrams, CaseyJohnson, JackAbrams };
            list.Sort(Compare<Person>.OrderBy(p => p.LastName).ThenByDescending(p => p.FirstName));
            CollectionAssert.AreEqual(new[] { WilliamAbrams, JackAbrams, AbeAbrams, CaseyJohnson }, list);
        }

        [TestMethod]
        public void ThenByDescendingUsesKeyComparer()
        {
            var list = new List<Person> { AbeAbrams, WilliamAbrams, CaseyJohnson, JackAbrams };
            list.Sort(Compare<Person>.OrderBy(p => p.LastName).ThenByDescending(p => p.FirstName, StringComparer.InvariantCulture.Reverse()));
            CollectionAssert.AreEqual(new[] { AbeAbrams, JackAbrams, WilliamAbrams, CaseyJohnson }, list);
        }

        [TestMethod]
        public void OrderBySortsNullsAsLowest()
        {
            var list = new List<Person> { AbeAbrams, JackAbrams, null, WilliamAbrams, nullAbrams, CaseyJohnson };
            list.Sort(Compare<Person>.OrderBy(p => p.FirstName));
            CollectionAssert.AreEqual(new[] { null, nullAbrams, AbeAbrams, CaseyJohnson, JackAbrams, WilliamAbrams }, list);
        }

        [TestMethod]
        public void OrderByWithNullPassesNullThrough()
        {
            var list = new List<Person> { null, WilliamAbrams };
            list.Sort(Compare<Person>.OrderBy(p => p == null, allowNulls:true));
            CollectionAssert.AreEqual(new[] { WilliamAbrams, null }, list);
        }

        [TestMethod]
        public void OrderByWithNullThenByHandlesNull()
        {
            var list = new List<Person> { AbeAbrams, JackAbrams, null, WilliamAbrams, CaseyJohnson };
            list.Sort(Compare<Person>.OrderBy(p => p == null, allowNulls: true).ThenBy(p => p.FirstName));
            CollectionAssert.AreEqual(new[] { AbeAbrams, CaseyJohnson, JackAbrams, WilliamAbrams, null }, list);
        }

        [TestMethod]
        public void OrderByWithNullThenByComparesNullsAsEqual()
        {
            var comparer = Compare<Person>.OrderBy(p => p == null, allowNulls: true).ThenBy(p => p.FirstName).ThenBy(p =>
            {
                Assert.Fail();
                return 0;
            });
            Assert.IsTrue(comparer.Compare(null, null) == 0);
            Assert.IsTrue(comparer.Equals(null, null));
            Assert.AreEqual(comparer.GetHashCode((object)null), comparer.GetHashCode((object)null));
            Assert.AreEqual(comparer.GetHashCode((Person)null), comparer.GetHashCode((Person)null));
        }

        [TestMethod]
        public void OrderByImplementsGetHashCode()
        {
            var comparer = Compare<Person>.OrderBy(p => p.LastName);
            var bclDefault = EqualityComparer<string>.Default;
            Assert.AreEqual(comparer.GetHashCode(AbeAbrams), comparer.GetHashCode(JackAbrams));
            Assert.AreEqual(bclDefault.GetHashCode(AbeAbrams.LastName) == bclDefault.GetHashCode(CaseyJohnson.LastName),
                comparer.GetHashCode(AbeAbrams) == comparer.GetHashCode(CaseyJohnson));
        }
    }
}
