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
    public class _Default
    {
        [TestMethod]
        public void IsEquivalentToEqualityComparerDefault()
        {
            var a = 3;
            var b = 3;
            var c = 5;

            var defaultComparer = EqualityCompare<int>.Default();
            var netDefaultComparer = EqualityComparer<int>.Default;
            var defaultObjectComparer = defaultComparer as System.Collections.IEqualityComparer;
            Assert.AreEqual(netDefaultComparer.GetHashCode(a), defaultComparer.GetHashCode(a));
            Assert.AreEqual(defaultComparer.GetHashCode(a), defaultObjectComparer.GetHashCode(a));
            Assert.AreEqual(netDefaultComparer.GetHashCode(b), defaultComparer.GetHashCode(b));
            Assert.AreEqual(defaultComparer.GetHashCode(b), defaultObjectComparer.GetHashCode(b));
            Assert.AreEqual(netDefaultComparer.GetHashCode(c), defaultComparer.GetHashCode(c));
            Assert.AreEqual(defaultComparer.GetHashCode(c), defaultObjectComparer.GetHashCode(c));
            Assert.AreEqual(netDefaultComparer.GetHashCode(7) == netDefaultComparer.GetHashCode(13), defaultComparer.GetHashCode(7) == defaultComparer.GetHashCode(13));
            Assert.AreEqual(defaultComparer.GetHashCode(7) == defaultComparer.GetHashCode(13), defaultObjectComparer.GetHashCode(7) == defaultObjectComparer.GetHashCode(13));
            Assert.AreEqual(netDefaultComparer.Equals(a, b), defaultComparer.Equals(a, b));
            Assert.AreEqual(defaultComparer.Equals(a, b), defaultObjectComparer.Equals(a, b));
            Assert.AreEqual(netDefaultComparer.Equals(a, c), defaultComparer.Equals(a, c));
            Assert.AreEqual(defaultComparer.Equals(a, c), defaultObjectComparer.Equals(a, c));
        }

        [TestMethod]
        public void UsesSequenceEqualityComparerForSequences()
        {
            var threeA = new[] { 3 };
            var threeB = new[] { 3 };
            var five = new[] { 5 };
            var comparer = EqualityCompare<int[]>.Default();
            var objectComparer = comparer as System.Collections.IEqualityComparer;
            Assert.AreEqual(comparer.GetHashCode(threeA), comparer.GetHashCode(threeB));
            Assert.AreEqual(objectComparer.GetHashCode(threeA), objectComparer.GetHashCode(threeB));
            Assert.IsTrue(comparer.Equals(threeA, threeB));
            Assert.IsTrue(objectComparer.Equals(threeA, threeB));
            Assert.IsFalse(comparer.Equals(threeB, five));
            Assert.IsFalse(objectComparer.Equals(threeB, five));
        }

        [TestMethod]
        public void NullIsNotEqualToValue()
        {
            var comparer = EqualityCompare<int?>.Default();
            var objectComparer = comparer as System.Collections.IEqualityComparer;
            Assert.IsFalse(comparer.Equals(3, null));
            Assert.IsFalse(objectComparer.Equals(3, null));
        }

        [TestMethod]
        public void NullSequenceNotEqualToEmptySequence()
        {
            var comparer = EqualityCompare<int[]>.Default();
            var objectComparer = comparer as System.Collections.IEqualityComparer;
            Assert.IsFalse(comparer.Equals(null, new int[0]));
            Assert.IsFalse(objectComparer.Equals(null, new int[0]));
        }

        [TestMethod]
        public void NullIsEqualToNull()
        {
            var comparer = EqualityCompare<int?>.Default();
            var objectComparer = comparer as System.Collections.IEqualityComparer;
            Assert.AreEqual(comparer.GetHashCode(null), comparer.GetHashCode(null));
            Assert.AreEqual(objectComparer.GetHashCode(null), objectComparer.GetHashCode(null));
            Assert.IsTrue(comparer.Equals(null, null));
            Assert.IsTrue(objectComparer.Equals(null, null));
        }

        [TestMethod]
        public void NullSequenceIsEqualToNullSequence()
        {
            var comparer = EqualityCompare<int[]>.Default();
            var objectComparer = comparer as System.Collections.IEqualityComparer;
            Assert.AreEqual(comparer.GetHashCode(null), comparer.GetHashCode(null));
            Assert.AreEqual(objectComparer.GetHashCode(null), objectComparer.GetHashCode(null));
            Assert.IsTrue(comparer.Equals(null, null));
            Assert.IsTrue(objectComparer.Equals(null, null));
        }

        [TestMethod]
        public void DefaultForString_IsDefaultComparer()
        {
            // Ensure string default comparer is not a sequence comparer over chars.
            Assert.AreSame(Nito.Comparers.Util.DefaultComparer<string>.Instance, EqualityCompare<string>.Default());
        }
    }
}
