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
    public class _ThenBy
    {
        private sealed class Person : ComparableBase<Person>
        {
            static Person()
            {
                DefaultComparer = Compare<Person>.OrderBy(p => p.LastName);
            }

            public string FirstName { get; set; }
            public string LastName { get; set; }
        }

        private static readonly Person AbeAbrams = new Person { FirstName = "Abe", LastName = "Abrams" };
        private static readonly Person JackAbrams = new Person { FirstName = "Jack", LastName = "Abrams" };
        private static readonly Person WilliamAbrams = new Person { FirstName = "William", LastName = "Abrams" };
        private static readonly Person CaseyJohnson = new Person { FirstName = "Casey", LastName = "Johnson" };

        [TestMethod]
        public void SubstitutesCompareDefaultForComparerDefault()
        {
            IComparer<Person> thenByComparer = Compare<string>.Default().SelectFrom((Person p) => p.FirstName);
            var comparer = Comparer<Person>.Default.ThenBy(thenByComparer);
            Assert.AreSame(Compare<Person>.Default(), (comparer as CompoundComparer<Person>).Source);

            var list = new List<Person> { AbeAbrams, WilliamAbrams, CaseyJohnson, JackAbrams };
            list.Sort(comparer);
            CollectionAssert.AreEqual(new[] { AbeAbrams, JackAbrams, WilliamAbrams, CaseyJohnson }, list);
        }

        [TestMethod]
        public void SubstitutesCompareDefaultForNull()
        {
            IComparer<Person> thenByComparer = Compare<string>.Default().SelectFrom((Person p) => p.FirstName);
            IComparer<Person> source = null;
            var comparer = source.ThenBy(thenByComparer);
            Assert.AreSame(Compare<Person>.Default(), (comparer as CompoundComparer<Person>).Source);

            var list = new List<Person> { AbeAbrams, WilliamAbrams, CaseyJohnson, JackAbrams };
            list.Sort(comparer);
            CollectionAssert.AreEqual(new[] { AbeAbrams, JackAbrams, WilliamAbrams, CaseyJohnson }, list);
        }

        [TestMethod]
        public void ThenByUsesComparer()
        {
            IComparer<Person> thenByComparer = Compare<string>.Default().SelectFrom((Person p) => p.FirstName);
            var list = new List<Person> { AbeAbrams, WilliamAbrams, CaseyJohnson, JackAbrams };
            list.Sort(Compare<Person>.Default().ThenBy(thenByComparer));
            CollectionAssert.AreEqual(new[] { AbeAbrams, JackAbrams, WilliamAbrams, CaseyJohnson }, list);
        }

        [TestMethod]
        public void ThenByIsAppliedAsTieBreaker()
        {
            IComparer<Person> thenByComparer = Compare<string>.Default().SelectFrom((Person p) => p.FirstName);
            IComparer<Person> defaultComparer = Compare<Person>.Default();
            IComparer<Person> fullComparer = defaultComparer.ThenBy(thenByComparer);
            Assert.IsTrue(defaultComparer.Compare(AbeAbrams, WilliamAbrams) == 0);
            Assert.IsTrue(thenByComparer.Compare(AbeAbrams, WilliamAbrams) < 0);
            Assert.IsTrue(fullComparer.Compare(AbeAbrams, WilliamAbrams) < 0);
        }

        [TestMethod]
        public void ThenByIsOnlyAppliedAsTieBreaker()
        {
            IComparer<Person> thenByComparer = new AnonymousComparer<Person>
            {
                Compare = (x, y) => { Assert.Fail(); return 0; },
            };
            var comparer = Compare<Person>.Default().ThenBy(thenByComparer);
            Assert.IsTrue(comparer.Compare(AbeAbrams, CaseyJohnson) < 0);
            Assert.IsTrue(comparer.Compare(CaseyJohnson, AbeAbrams) > 0);
        }

        // The delegate overloads are tested by Compare_._Key.
    }
}
