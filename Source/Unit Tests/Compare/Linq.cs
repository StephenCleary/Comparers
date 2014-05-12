using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Comparers;
using Comparers.Linq;
using EqualityComparers;

namespace Compare_
{
    [TestClass]
    public class _Linq
    {
        private static readonly Person AbeAbrams = new Person { FirstName = "Abe", LastName = "Abrams" };
        private static readonly Person JackAbrams = new Person { FirstName = "Jack", LastName = "Abrams" };
        private static readonly Person WilliamAbrams = new Person { FirstName = "William", LastName = "Abrams" };
        private static readonly Person CaseyJohnson = new Person { FirstName = "Casey", LastName = "Johnson" };

        [TestMethod]
        public void Anonymous_OrderBy_SortsByKey()
        {
            var list = new List<Person> { AbeAbrams, JackAbrams, WilliamAbrams, CaseyJohnson }.Select(x => new { GivenName = x.FirstName, Surname = x.LastName });
            var distinct = list.Distinct(c => c.EquateBy(x => x.Surname)).ToList();
            Assert.AreEqual(2, distinct.Count);
            Assert.AreEqual(AbeAbrams.FirstName, distinct[0].GivenName);
            Assert.AreEqual(AbeAbrams.LastName, distinct[0].Surname);
            Assert.AreEqual(CaseyJohnson.FirstName, distinct[1].GivenName);
            Assert.AreEqual(CaseyJohnson.LastName, distinct[1].Surname);
        }
    }
}
