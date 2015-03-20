using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nito.Comparers;
using Nito.EqualityComparers;
using Nito.Comparers.Util;

namespace ComparerExtensions_
{
    [TestClass]
    public class _SelectFrom
    {
        private static List<Person> GetPeople()
        {
            return new List<Person>
            {
                new Person { Priority = 3 },
                new Person { Priority = 4 },
                new Person { Priority = 2 },
                new Person { Priority = 5 },
            };
        }

        [TestMethod]
        public void SubstitutesCompareDefaultForComparerDefault()
        {
            var comparer = Comparer<int>.Default.SelectFrom((Person p) => p.Priority);
            Assert.AreSame(Compare<int>.Default(), (comparer as SelectComparer<Person, int>).Source);

            var list = GetPeople();
            list.Sort(comparer);
            CollectionAssert.AreEqual(new[] { 2, 3, 4, 5 }, list.Select(x => x.Priority).ToList());
        }

        [TestMethod]
        public void SubstitutesCompareDefaultForNull()
        {
            IComparer<int> source = null;
            var comparer = source.SelectFrom((Person p) => p.Priority);
            Assert.AreSame(Compare<int>.Default(), (comparer as SelectComparer<Person, int>).Source);

            var list = GetPeople();
            list.Sort(comparer);
            CollectionAssert.AreEqual(new[] { 2, 3, 4, 5 }, list.Select(x => x.Priority).ToList());
        }

        [TestMethod]
        public void SortsByKey()
        {
            var list = GetPeople();
            list.Sort(Compare<int>.Default().SelectFrom((Person p) => p.Priority));
            CollectionAssert.AreEqual(new[] { 2, 3, 4, 5 }, list.Select(x => x.Priority).ToList());
        }
    }
}
