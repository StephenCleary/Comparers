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
    public class _Reference
    {
        [TestMethod]
        public void IdenticalObjectsAreEqual()
        {
            var comparer = EqualityCompare<object>.Reference();
            var objectComparer = comparer as System.Collections.IEqualityComparer;
            var obj = new object();
            Assert.AreEqual(comparer.GetHashCode(obj), objectComparer.GetHashCode(obj));
            Assert.IsTrue(comparer.Equals(obj, obj));
            Assert.IsTrue(objectComparer.Equals(obj, obj));
        }

        [TestMethod]
        public void EqualValueTypesAreNotEqual()
        {
            var comparer = EqualityCompare<int>.Reference();
            var objectComparer = comparer as System.Collections.IEqualityComparer;
            var value = 13;
            Assert.IsFalse(comparer.Equals(value, value));
            Assert.IsFalse(objectComparer.Equals(value, value));
        }

        [TestMethod]
        public void UsesReferenceEqualityComparerForSequences()
        {
            var threeA = new[] { 3 };
            var threeB = new[] { 3 };
            var comparer = EqualityCompare<int[]>.Reference();
            var objectComparer = comparer as System.Collections.IEqualityComparer;
            Assert.IsFalse(comparer.Equals(threeA, threeB));
            Assert.IsFalse(objectComparer.Equals(threeA, threeB));
        }

        [TestMethod]
        public void NullIsNotEqualToValue()
        {
            var comparer = EqualityCompare<object>.Reference();
            var objectComparer = comparer as System.Collections.IEqualityComparer;
            var obj = new object();
            Assert.IsFalse(comparer.Equals(obj, null));
            Assert.IsFalse(objectComparer.Equals(obj, null));
        }

        [TestMethod]
        public void NullSequenceNotEqualToEmptySequence()
        {
            var comparer = EqualityCompare<int[]>.Reference();
            var objectComparer = comparer as System.Collections.IEqualityComparer;
            Assert.IsFalse(comparer.Equals(null, new int[0]));
            Assert.IsFalse(objectComparer.Equals(null, new int[0]));
        }

        [TestMethod]
        public void NullIsEqualToNull()
        {
            var comparer = EqualityCompare<int?>.Reference();
            var objectComparer = comparer as System.Collections.IEqualityComparer;
            Assert.AreEqual(comparer.GetHashCode(null), comparer.GetHashCode(null));
            Assert.AreEqual(objectComparer.GetHashCode(null), objectComparer.GetHashCode(null));
            Assert.IsTrue(comparer.Equals(null, null));
            Assert.IsTrue(objectComparer.Equals(null, null));
        }

        [TestMethod]
        public void NullSequenceIsEqualToNullSequence()
        {
            var comparer = EqualityCompare<int[]>.Reference();
            var objectComparer = comparer as System.Collections.IEqualityComparer;
            Assert.AreEqual(comparer.GetHashCode(null), comparer.GetHashCode(null));
            Assert.AreEqual(objectComparer.GetHashCode(null), objectComparer.GetHashCode(null));
            Assert.IsTrue(comparer.Equals(null, null));
            Assert.IsTrue(objectComparer.Equals(null, null));
        }
    }
}
