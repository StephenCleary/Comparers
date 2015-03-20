using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nito.Comparers;
using Nito.EqualityComparers;
using Nito.EqualityComparers.Util;

namespace EqualityComparerExtensions_
{
    [TestClass]
    public class _ThenEquateBy
    {
        private sealed class Person : EquatableBase<Person>
        {
            static Person()
            {
                DefaultComparer = EqualityCompare<Person>.EquateBy(p => p.LastName);
            }

            public string FirstName { get; set; }
            public string LastName { get; set; }
        }

        private static readonly Person AbeAbrams = new Person { FirstName = "Abe", LastName = "Abrams" };
        private static readonly Person JackAbrams = new Person { FirstName = "Jack", LastName = "Abrams" };
        private static readonly Person JackAbrams2 = new Person { FirstName = "Jack", LastName = "Abrams" };
        private static readonly Person WilliamAbrams = new Person { FirstName = "William", LastName = "Abrams" };
        private static readonly Person CaseyJohnson = new Person { FirstName = "Casey", LastName = "Johnson" };

        [TestMethod]
        public void SubstitutesCompareDefaultForComparerDefault()
        {
            IEqualityComparer<Person> thenByComparer = EqualityCompare<string>.Default().SelectEquateFrom((Person p) => p.FirstName);
            var comparer = EqualityComparer<Person>.Default.ThenEquateBy(thenByComparer);
            Assert.AreSame(EqualityCompare<Person>.Default(), (comparer as CompoundEqualityComparer<Person>).Source);
        }

        [TestMethod]
        public void SubstitutesCompareDefaultForNull()
        {
            IEqualityComparer<Person> thenByComparer = EqualityCompare<string>.Default().SelectEquateFrom((Person p) => p.FirstName);
            IEqualityComparer<Person> source = null;
            var comparer = source.ThenEquateBy(thenByComparer);
            Assert.AreSame(EqualityCompare<Person>.Default(), (comparer as CompoundEqualityComparer<Person>).Source);
        }

        [TestMethod]
        public void ThenByUsesComparer()
        {
            IEqualityComparer<Person> thenByComparer = EqualityCompare<string>.Default().SelectEquateFrom((Person p) => p.FirstName);
            var comparer = EqualityCompare<Person>.Default().ThenEquateBy(thenByComparer);
            var objectComparer = comparer as System.Collections.IEqualityComparer;
            Assert.IsFalse(comparer.Equals(AbeAbrams, JackAbrams));
            Assert.IsFalse(objectComparer.Equals(AbeAbrams, JackAbrams));
            Assert.IsTrue(comparer.Equals(JackAbrams, JackAbrams2));
            Assert.IsTrue(objectComparer.Equals(JackAbrams, JackAbrams2));
            Assert.AreEqual(comparer.GetHashCode(JackAbrams), comparer.GetHashCode(JackAbrams2));
            Assert.AreEqual(objectComparer.GetHashCode(JackAbrams), objectComparer.GetHashCode(JackAbrams2));
        }

        [TestMethod]
        public void ThenByIsAppliedAsTieBreaker()
        {
            IEqualityComparer<Person> thenByComparer = EqualityCompare<string>.Default().SelectEquateFrom((Person p) => p.FirstName);
            IEqualityComparer<Person> defaultComparer = EqualityCompare<Person>.Default();
            IEqualityComparer<Person> fullComparer = defaultComparer.ThenEquateBy(thenByComparer);
            Assert.IsTrue(defaultComparer.Equals(AbeAbrams, WilliamAbrams));
            Assert.AreEqual(defaultComparer.GetHashCode(AbeAbrams), defaultComparer.GetHashCode(WilliamAbrams));
            Assert.IsFalse(thenByComparer.Equals(AbeAbrams, WilliamAbrams));
            Assert.IsFalse(fullComparer.Equals(AbeAbrams, WilliamAbrams));
        }

        [TestMethod]
        public void ThenByIsOnlyAppliedAsTieBreaker()
        {
            IEqualityComparer<Person> thenByComparer = new AnonymousEqualityComparer<Person>
            {
                Equals = (x, y) => { Assert.Fail(); return false; },
            };
            var comparer = EqualityCompare<Person>.Default().ThenEquateBy(thenByComparer);
            Assert.IsFalse(comparer.Equals(AbeAbrams, CaseyJohnson));
            Assert.IsFalse(comparer.Equals(CaseyJohnson, AbeAbrams));
        }

        // The delegate overloads are tested by EqualityCompare_._Key.
    }
}
