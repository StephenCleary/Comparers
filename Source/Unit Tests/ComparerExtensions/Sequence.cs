using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Comparers;
using EqualityComparers;
using Comparers.Util;

namespace ComparerExtensions_
{
    [TestClass]
    public class _Sequence
    {
        private static readonly int[] three = new[] { 3 };
        private static readonly int[] four = new[] { 4 };
        private static readonly int[] five = new[] { 5 };

        [TestMethod]
        public void SubstitutesCompareDefaultForComparerDefault()
        {
            var comparer = Comparer<int>.Default.Sequence();
            Assert.AreSame(Compare<int>.Default(), (comparer as SequenceComparer<int>).Source);

            var list = new[] { three, five, four }.ToList();
            list.Sort(comparer);
            CollectionAssert.AreEqual(new[] { three, four, five }, list);
        }

        [TestMethod]
        public void SubstitutesCompareDefaultForNull()
        {
            IComparer<int> source = null;
            var comparer = source.Sequence();
            Assert.AreSame(Compare<int>.Default(), (comparer as SequenceComparer<int>).Source);

            var list = new[] { three, five, four }.ToList();
            list.Sort(comparer);
            CollectionAssert.AreEqual(new[] { three, four, five }, list);
        }

        [TestMethod]
        public void ShorterSequenceIsSmallerIfElementsAreEqual()
        {
            Assert.IsTrue(Compare<int>.Default().Sequence().Compare(new[] { 3, 4 }, new[] { 3, 4, 5 }) < 0);
            Assert.IsTrue(Compare<int>.Default().Sequence().Compare(new[] { 3, 4, 5 }, new[] { 3, 4 }) > 0);
        }

        [TestMethod]
        public void EqualSequencesAreEqualIfElementsAreEqual()
        {
            Assert.IsTrue(Compare<int>.Default().Sequence().Compare(new[] { 3, 4 }, new[] { 3, 4 }) == 0);
            Assert.IsTrue(Compare<int>.Default().Sequence().Compare(new[] { 3, 4, 5 }, new[] { 3, 4, 5 }) == 0);
            Assert.AreEqual(Compare<int>.Default().Sequence().GetHashCode(new[] { 3, 4 }), Compare<int>.Default().Sequence().GetHashCode(new[] { 3, 4 }));
        }

        [TestMethod]
        public void SequenceOrderDeterminedByElementsIfElementsAreNotEqual()
        {
            Assert.IsTrue(Compare<int>.Default().Sequence().Compare(new[] { 3, 4 }, new[] { 3, 5 }) < 0);
            Assert.IsTrue(Compare<int>.Default().Sequence().Compare(new[] { 3, 4, 5 }, new[] { 3, 3, 5 }) > 0);
        }

        [TestMethod]
        public void SequenceUsesSourceComparerForElementComparisons()
        {
            Assert.IsTrue(Compare<int>.Default().Reverse().Sequence().Compare(new[] { 3, 4 }, new[] { 3, 5 }) > 0);
            Assert.IsTrue(Compare<int>.Default().Reverse().Sequence().Compare(new[] { 3, 4, 5 }, new[] { 3, 3, 5 }) < 0);
        }

        [TestMethod]
        public void NullIsSmallerThanEmpty()
        {
            Assert.IsTrue(Compare<int>.Default().Sequence().Compare(null, Enumerable.Empty<int>()) < 0);
            Assert.IsTrue(Compare<int>.Default().Sequence().Compare(Enumerable.Empty<int>(), null) > 0);
        }
    }
}
