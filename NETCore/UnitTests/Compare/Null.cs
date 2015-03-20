using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nito.Comparers;
using Nito.EqualityComparers;

namespace Compare_
{
    [TestClass]
    public class _Null
    {
        [TestMethod]
        public void ComparesUnequalElementsAsEqual()
        {
            var comparer = Compare<int>.Null();
            Assert.AreEqual(0, comparer.Compare(13, 17));
            Assert.IsTrue(comparer.Equals(19, 21));
            Assert.AreEqual(comparer.GetHashCode(13), comparer.GetHashCode(17));
        }

        [TestMethod]
        public void ComparesNullElementsAsEqualToValueElements()
        {
            var comparer = Compare<int?>.Null();
            Assert.AreEqual(0, comparer.Compare(null, 17));
            Assert.AreEqual(0, comparer.Compare(13, null));
            Assert.IsTrue(comparer.Equals(null, 21));
            Assert.IsTrue(comparer.Equals(19, null));
            Assert.AreEqual(comparer.GetHashCode(13), comparer.GetHashCode((object)null));
            Assert.AreEqual(comparer.GetHashCode(13), comparer.GetHashCode((int?)null));
        }

        [TestMethod]
        public void ComparesEqualElementsAsEqual()
        {
            var comparer = Compare<int>.Null();
            Assert.AreEqual(0, comparer.Compare(13, 13));
            Assert.IsTrue(comparer.Equals(13, 13));
            Assert.AreEqual(comparer.GetHashCode(13), comparer.GetHashCode(13));
        }

        [TestMethod]
        public void ComparesNullElementsAsEqual()
        {
            var comparer = Compare<int?>.Null();
            Assert.AreEqual(0, comparer.Compare(null, null));
            Assert.IsTrue(comparer.Equals(null, null));
            Assert.AreEqual(comparer.GetHashCode((object)null), comparer.GetHashCode((object)null));
            Assert.AreEqual(comparer.GetHashCode((int?)null), comparer.GetHashCode((int?)null));
        }
    }
}
