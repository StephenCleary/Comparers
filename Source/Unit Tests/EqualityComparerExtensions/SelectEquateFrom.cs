using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Comparers;
using EqualityComparers;
using EqualityComparers.Util;

namespace EqualityComparerExtensions_
{
    [TestClass]
    public class _SelectEquateFrom
    {
        [TestMethod]
        public void SubstitutesCompareDefaultForComparerDefault()
        {
            var comparer = EqualityComparer<int>.Default.SelectEquateFrom((Person p) => p.Priority);
            Assert.AreSame(EqualityCompare<int>.Default(), (comparer as SelectEqualityComparer<Person, int>).Source);
        }

        [TestMethod]
        public void SubstitutesCompareDefaultForNull()
        {
            IEqualityComparer<int> source = null;
            var comparer = source.SelectEquateFrom((Person p) => p.Priority);
            Assert.AreSame(EqualityCompare<int>.Default(), (comparer as SelectEqualityComparer<Person, int>).Source);
        }
    }
}
