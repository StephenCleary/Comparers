using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nito.Comparers;
using Nito.EqualityComparers;

namespace EqualityCompare_
{
    [TestClass]
    public class _Key
    {
        private static readonly Person AbeAbrams = new Person { FirstName = "Abe", LastName = "Abrams" };
        private static readonly Person JackAbrams = new Person { FirstName = "Jack", LastName = "Abrams" };
        private static readonly Person Williamabrams = new Person { FirstName = "William", LastName = "abrams" };
        private static readonly Person CaseyJohnson = new Person { FirstName = "Casey", LastName = "Johnson" };
        private static readonly Person JackAbrams2 = new Person { FirstName = "Jack", LastName = "Abrams" };
        private static readonly Person jackAbrams = new Person { FirstName = "jack", LastName = "Abrams" };
        private static readonly Person Jacknull = new Person { FirstName = "Jack", LastName = null };

        [TestMethod]
        public void OrderByComparesByKey()
        {
            var comparer = EqualityCompare<Person>.EquateBy(p => p.LastName);
            var objectComparer = comparer as System.Collections.IEqualityComparer;
            Assert.IsTrue(comparer.Equals(AbeAbrams, JackAbrams));
            Assert.IsTrue(objectComparer.Equals(AbeAbrams, JackAbrams));
            Assert.AreEqual(comparer.GetHashCode(AbeAbrams), comparer.GetHashCode(JackAbrams));
            Assert.AreEqual(objectComparer.GetHashCode(AbeAbrams), objectComparer.GetHashCode(JackAbrams));
            Assert.IsFalse(comparer.Equals(AbeAbrams, CaseyJohnson));
            Assert.IsFalse(objectComparer.Equals(AbeAbrams, CaseyJohnson));
        }

        [TestMethod]
        public void OrderByUsesKeyComparer()
        {
            var keyComparer = StringComparer.InvariantCultureIgnoreCase;
            var comparer = EqualityCompare<Person>.EquateBy(p => p.LastName, keyComparer);
            var objectComparer = comparer as System.Collections.IEqualityComparer;
            Assert.IsTrue(comparer.Equals(AbeAbrams, Williamabrams));
            Assert.IsTrue(objectComparer.Equals(AbeAbrams, Williamabrams));
            Assert.AreEqual(comparer.GetHashCode(AbeAbrams), comparer.GetHashCode(Williamabrams));
            Assert.AreEqual(objectComparer.GetHashCode(AbeAbrams), objectComparer.GetHashCode(Williamabrams));
            Assert.IsFalse(comparer.Equals(Williamabrams, CaseyJohnson));
            Assert.IsFalse(objectComparer.Equals(Williamabrams, CaseyJohnson));
            Assert.AreEqual(keyComparer.GetHashCode(Williamabrams.LastName) == keyComparer.GetHashCode(CaseyJohnson.LastName),
                comparer.GetHashCode(Williamabrams) == comparer.GetHashCode(CaseyJohnson));
        }

        [TestMethod]
        public void ThenBySortsByKey()
        {
            var comparer = EqualityCompare<Person>.EquateBy(p => p.LastName).ThenEquateBy(p => p.FirstName);
            var objectComparer = comparer as System.Collections.IEqualityComparer;
            Assert.IsTrue(comparer.Equals(JackAbrams, JackAbrams2));
            Assert.IsTrue(objectComparer.Equals(JackAbrams, JackAbrams2));
            Assert.AreEqual(comparer.GetHashCode(JackAbrams), comparer.GetHashCode(JackAbrams2));
            Assert.AreEqual(objectComparer.GetHashCode(JackAbrams), objectComparer.GetHashCode(JackAbrams2));
            Assert.IsFalse(comparer.Equals(AbeAbrams, JackAbrams));
            Assert.IsFalse(objectComparer.Equals(AbeAbrams, JackAbrams));
        }

        [TestMethod]
        public void ThenByUsesKeyComparer()
        {
            var comparer = EqualityCompare<Person>.EquateBy(p => p.LastName).ThenEquateBy(p => p.FirstName, StringComparer.InvariantCultureIgnoreCase);
            var objectComparer = comparer as System.Collections.IEqualityComparer;
            Assert.IsTrue(comparer.Equals(JackAbrams, jackAbrams));
            Assert.IsTrue(objectComparer.Equals(JackAbrams, jackAbrams));
            Assert.AreEqual(comparer.GetHashCode(JackAbrams), comparer.GetHashCode(jackAbrams));
            Assert.AreEqual(objectComparer.GetHashCode(JackAbrams), objectComparer.GetHashCode(jackAbrams));
            Assert.IsFalse(comparer.Equals(AbeAbrams, JackAbrams));
            Assert.IsFalse(objectComparer.Equals(AbeAbrams, JackAbrams));
        }

        [TestMethod]
        public void OrderBySortsNullsAsNotEqualToValues()
        {
            var comparer = EqualityCompare<Person>.EquateBy(p => p.LastName);
            var objectComparer = comparer as System.Collections.IEqualityComparer;
            Assert.IsFalse(comparer.Equals(JackAbrams, null));
            Assert.IsFalse(objectComparer.Equals(JackAbrams, null));
            Assert.IsFalse(comparer.Equals(JackAbrams, Jacknull));
            Assert.IsFalse(objectComparer.Equals(JackAbrams, Jacknull));
        }

        [TestMethod]
        public void OrderByWithNullPassesNullThrough()
        {
            var comparer = EqualityCompare<Person>.EquateBy(p => 0, allowNulls:true);
            var objectComparer = comparer as System.Collections.IEqualityComparer;
            Assert.IsTrue(comparer.Equals(JackAbrams, null));
            Assert.IsTrue(objectComparer.Equals(JackAbrams, null));
            Assert.AreEqual(comparer.GetHashCode(JackAbrams), comparer.GetHashCode(null));
            Assert.AreEqual(objectComparer.GetHashCode(JackAbrams), objectComparer.GetHashCode(null));
        }

        [TestMethod]
        public void OrderByWithNullThenByHandlesNull()
        {
            var comparer = EqualityCompare<Person>.EquateBy(p => p == null, allowNulls: true).ThenEquateBy(p => p.LastName).ThenEquateBy(p =>
            {
                Assert.Fail();
                return 0;
            });
            var objectComparer = comparer as System.Collections.IEqualityComparer;
            Assert.IsTrue(comparer.Equals(null, null));
            Assert.IsTrue(objectComparer.Equals(null, null));
            Assert.AreEqual(comparer.GetHashCode(null), comparer.GetHashCode(null));
            Assert.AreEqual(objectComparer.GetHashCode(null), objectComparer.GetHashCode(null));
        }
    }
}
