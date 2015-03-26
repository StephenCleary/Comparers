using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nito.Comparers;
using Nito.EqualityComparers;
using Nito.EqualityComparers.Util;

namespace EqualityComparerExtensions_
{
    [TestClass]
    public class _EquateSequence
    {
        [TestMethod]
        public void SubstitutesCompareDefaultForComparerDefault()
        {
            var comparer = EqualityComparer<int>.Default.EquateSequence();
            Assert.AreEqual(EqualityComparerBuilder.For<int>().Default().EquateSequence().ToString(), comparer.ToString());
        }

        [TestMethod]
        public void SubstitutesCompareDefaultForNull()
        {
            IEqualityComparer<int> source = null;
            var comparer = source.EquateSequence();
            Assert.AreEqual(EqualityComparerBuilder.For<int>().Default().EquateSequence().ToString(), comparer.ToString());
        }

        [TestMethod]
        public void ShorterSequenceIsNotEqualToLongerSequenceIfElementsAreEqual()
        {
            Assert.IsFalse(EqualityComparerBuilder.For<int>().Default().EquateSequence().Equals(new[] { 3, 4 }, new[] { 3, 4, 5 }));
            Assert.IsFalse(EqualityComparerBuilder.For<int>().Default().EquateSequence().Equals(new[] { 3, 4, 5 }, new[] { 3, 4 }));
        }

        [TestMethod]
        public void SequencesAreEqualIfElementsAreEqual()
        {
            Assert.IsTrue(EqualityComparerBuilder.For<int>().Default().EquateSequence().Equals(new[] { 3, 4 }, new[] { 3, 4 }));
            Assert.IsTrue(EqualityComparerBuilder.For<int>().Default().EquateSequence().Equals(new[] { 3, 4, 5 }, new[] { 3, 4, 5 }));
            Assert.AreEqual(EqualityComparerBuilder.For<int>().Default().EquateSequence().GetHashCode(new[] { 3, 4 }), EqualityComparerBuilder.For<int>().Default().EquateSequence().GetHashCode(new[] { 3, 4 }));
        }

        [TestMethod]
        public void EqualLengthSequencesWithUnequalElementsAreNotEqual()
        {
            Assert.IsFalse(EqualityComparerBuilder.For<int>().Default().EquateSequence().Equals(new[] { 3, 4 }, new[] { 3, 5 }));
            Assert.IsFalse(EqualityComparerBuilder.For<int>().Default().EquateSequence().Equals(new[] { 3, 4, 5 }, new[] { 3, 3, 5 }));
        }

        [TestMethod]
        public void SequenceUsesSourceComparerForElementComparisons()
        {
            var comparer = StringComparer.InvariantCultureIgnoreCase.EquateSequence();
            Assert.IsTrue(comparer.Equals(new[] { "a" }, new[] { "A" }));
            Assert.AreEqual(comparer.GetHashCode(new[] { "a" }), comparer.GetHashCode(new[] { "A" }));
        }

        [TestMethod]
        public void NullIsNotEqualToEmpty()
        {
            Assert.IsFalse(EqualityComparerBuilder.For<int>().Default().EquateSequence().Equals(null, Enumerable.Empty<int>()));
            Assert.IsFalse(EqualityComparerBuilder.For<int>().Default().EquateSequence().Equals(Enumerable.Empty<int>(), null));
        }
    }
}
