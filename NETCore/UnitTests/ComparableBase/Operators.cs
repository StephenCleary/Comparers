using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nito.Comparers;
using Nito.EqualityComparers;

namespace ComparableBase_
{
    [TestClass]
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

        [TestMethod]
        public void ImplementsComparerDefault()
        {
            var list = new List<Person> { JackAbrams, CaseyJohnson, AbeAbrams, WilliamAbrams };
            list.Sort();
            CollectionAssert.AreEqual(new[] { AbeAbrams, JackAbrams, WilliamAbrams, CaseyJohnson }, list);
        }

        [TestMethod]
        public void ImplementsOpEquality()
        {
            Assert.IsTrue(AbeAbrams == AbeAbrams2);
            Assert.IsFalse(AbeAbrams == JackAbrams);
        }

        [TestMethod]
        public void ImplementsOpInequality()
        {
            Assert.IsFalse(AbeAbrams != AbeAbrams2);
            Assert.IsTrue(AbeAbrams != JackAbrams);
        }

        [TestMethod]
        public void ImplementsOpLessThan()
        {
            Assert.IsFalse(AbeAbrams < AbeAbrams2);
            Assert.IsTrue(AbeAbrams < JackAbrams);
            Assert.IsFalse(JackAbrams < AbeAbrams);
            Assert.IsTrue(AbeAbrams < CaseyJohnson);
            Assert.IsFalse(CaseyJohnson < AbeAbrams);
        }

        [TestMethod]
        public void ImplementsOpLessThanOrEqual()
        {
            Assert.IsTrue(AbeAbrams <= AbeAbrams2);
            Assert.IsTrue(AbeAbrams <= JackAbrams);
            Assert.IsFalse(JackAbrams <= AbeAbrams);
            Assert.IsTrue(AbeAbrams <= CaseyJohnson);
            Assert.IsFalse(CaseyJohnson <= AbeAbrams);
        }

        [TestMethod]
        public void ImplementsOpGreaterThan()
        {
            Assert.IsFalse(AbeAbrams > AbeAbrams2);
            Assert.IsFalse(AbeAbrams > JackAbrams);
            Assert.IsTrue(JackAbrams > AbeAbrams);
            Assert.IsFalse(AbeAbrams > CaseyJohnson);
            Assert.IsTrue(CaseyJohnson > AbeAbrams);
        }

        [TestMethod]
        public void ImplementsOpGreaterThanOrEqual()
        {
            Assert.IsTrue(AbeAbrams >= AbeAbrams2);
            Assert.IsFalse(AbeAbrams >= JackAbrams);
            Assert.IsTrue(JackAbrams >= AbeAbrams);
            Assert.IsFalse(AbeAbrams >= CaseyJohnson);
            Assert.IsTrue(CaseyJohnson >= AbeAbrams);
        }
    }
}
