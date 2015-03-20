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
    public class _Null
    {
        [TestMethod]
        public void ComparesUnequalElementsAsEqual()
        {
            var comparer = EqualityComparerBuilder.For<int>().Null();
            var objectComparer = comparer as System.Collections.IEqualityComparer;
            Assert.IsTrue(comparer.Equals(13, 17));
            Assert.IsTrue(objectComparer.Equals(13, 17));
            Assert.AreEqual(comparer.GetHashCode(13), comparer.GetHashCode(17));
            Assert.AreEqual(objectComparer.GetHashCode(13), objectComparer.GetHashCode(17));
        }

        [TestMethod]
        public void ComparesNullElementsAsEqualToValueElements()
        {
            var comparer = EqualityComparerBuilder.For<int?>().Null();
            var objectComparer = comparer as System.Collections.IEqualityComparer;
            Assert.IsTrue(comparer.Equals(null, 21));
            Assert.IsTrue(objectComparer.Equals(null, 21));
            Assert.IsTrue(comparer.Equals(19, null));
            Assert.IsTrue(objectComparer.Equals(19, null));
            Assert.AreEqual(comparer.GetHashCode(13), comparer.GetHashCode(null));
            Assert.AreEqual(objectComparer.GetHashCode(13), objectComparer.GetHashCode(null));
        }

        [TestMethod]
        public void ComparesEqualElementsAsEqual()
        {
            var comparer = EqualityComparerBuilder.For<int>().Null();
            var objectComparer = comparer as System.Collections.IEqualityComparer;
            Assert.IsTrue(comparer.Equals(13, 13));
            Assert.IsTrue(objectComparer.Equals(13, 13));
            Assert.AreEqual(comparer.GetHashCode(13), comparer.GetHashCode(13));
            Assert.AreEqual(objectComparer.GetHashCode(13), objectComparer.GetHashCode(13));
        }

        [TestMethod]
        public void ComparesNullElementsAsEqual()
        {
            var comparer = EqualityComparerBuilder.For<int?>().Null();
            var objectComparer = comparer as System.Collections.IEqualityComparer;
            Assert.IsTrue(comparer.Equals(null, null));
            Assert.IsTrue(objectComparer.Equals(null, null));
            Assert.AreEqual(comparer.GetHashCode(null), comparer.GetHashCode(null));
            Assert.AreEqual(objectComparer.GetHashCode(null), objectComparer.GetHashCode(null));
        }
    }
}
