using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Comparers;
using EqualityComparers;
using Comparers.Util;

namespace ComparerExtensions_
{
    [TestClass]
    public class _Reverse
    {
        [TestMethod]
        public void SubstitutesCompareDefaultForComparerDefault()
        {
            var comparer = Comparer<int>.Default.Reverse();
            Assert.AreSame(Compare<int>.Default(), (comparer as ReverseComparer<int>).Source);

            var list = Enumerable.Range(0, 5).ToList();
            list.Sort(comparer);
            CollectionAssert.AreEqual(new[] { 4, 3, 2, 1, 0 }, list);
        }

        [TestMethod]
        public void SubstitutesCompareDefaultForNull()
        {
            IComparer<int> source = null;
            var comparer = source.Reverse();
            Assert.AreSame(Compare<int>.Default(), (comparer as ReverseComparer<int>).Source);

            var list = Enumerable.Range(0, 5).ToList();
            list.Sort(comparer);
            CollectionAssert.AreEqual(new[] { 4, 3, 2, 1, 0 }, list);
        }

        [TestMethod]
        public void ReversesComparer()
        {
            var list = Enumerable.Range(0, 5).ToList();
            list.Sort(Compare<int>.Default().Reverse());
            CollectionAssert.AreEqual(new[] { 4, 3, 2, 1, 0 }, list);
        }

        [TestMethod]
        public void ReversesComparerWithNull()
        {
            var list = Enumerable.Repeat<int?>(null, 1).Concat(Enumerable.Range(0, 5).Cast<int?>()).ToList();
            list.Sort(Compare<int?>.Default().Reverse());
            CollectionAssert.AreEqual(new int?[] { 4, 3, 2, 1, 0, null }, list);
        }

        [TestMethod]
        public void PassesGetHashCodeThrough()
        {
            var comparer = Compare<int?>.Default().Reverse();
            var bclComparer = EqualityComparer<int?>.Default;
            Assert.AreEqual(bclComparer.GetHashCode(7) == bclComparer.GetHashCode(13), comparer.GetHashCode(7) == comparer.GetHashCode(13));
        }
    }
}
