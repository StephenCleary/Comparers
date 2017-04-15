using System;
using System.Collections.Generic;
using System.Linq;
using Nito.Comparers;
using Xunit;
using System.Globalization;

namespace UnitTests
{
    public class EqualityCompare_EquateSequenceUnitTests
    {
        [Fact]
        public void SubstitutesCompareDefaultForComparerDefault()
        {
            var comparer = EqualityComparer<int>.Default.EquateSequence();
            Assert.Equal(EqualityComparerBuilder.For<int>().Default().EquateSequence().ToString(), comparer.ToString());
        }

        [Fact]
        public void SubstitutesCompareDefaultForNull()
        {
            IEqualityComparer<int> source = null;
            var comparer = source.EquateSequence();
            Assert.Equal(EqualityComparerBuilder.For<int>().Default().EquateSequence().ToString(), comparer.ToString());
        }

        [Fact]
        public void ShorterSequenceIsNotEqualToLongerSequenceIfElementsAreEqual()
        {
            Assert.False(EqualityComparerBuilder.For<int>().Default().EquateSequence().Equals(new[] { 3, 4 }, new[] { 3, 4, 5 }));
            Assert.False(EqualityComparerBuilder.For<int>().Default().EquateSequence().Equals(new[] { 3, 4, 5 }, new[] { 3, 4 }));
        }

        [Fact]
        public void ShorterEnumerableIsNotEqualToLongerEnumerableIfElementsAreEqual()
        {
            Assert.False(EqualityComparerBuilder.For<int>().Default().EquateSequence().Equals(E_3_4(), E_3_4_5()));
            Assert.False(EqualityComparerBuilder.For<int>().Default().EquateSequence().Equals(E_3_4_5(), E_3_4()));
        }

        [Fact]
        public void ShorterICollectionIsNotEqualToLongerICollectionIfElementsAreEqual()
        {
            var e34 = new NongenericICollection<int>(E_3_4().ToList());
            var e345 = new NongenericICollection<int>(E_3_4_5().ToList());
            Assert.False(EqualityComparerBuilder.For<int>().Default().EquateSequence().Equals(e34, e345));
            Assert.False(EqualityComparerBuilder.For<int>().Default().EquateSequence().Equals(e345, e34));
        }

        [Fact]
        public void ShorterGenericICollectionIsNotEqualToLongerGenericICollectionIfElementsAreEqual()
        {
            var e34 = new GenericICollection<int>(E_3_4().ToList());
            var e345 = new GenericICollection<int>(E_3_4_5().ToList());
            Assert.False(EqualityComparerBuilder.For<int>().Default().EquateSequence().Equals(e34, e345));
            Assert.False(EqualityComparerBuilder.For<int>().Default().EquateSequence().Equals(e345, e34));
        }

        private sealed class NongenericICollection<T> : IEnumerable<T>, System.Collections.ICollection
        {
            private readonly List<T> _source;

            public NongenericICollection(List<T> source)
            {
                _source = source;
            }

            public int Count
            {
                get
                {
                    return _source.Count;
                }
            }

            public bool IsSynchronized
            {
                get
                {
                    return ((System.Collections.ICollection)_source).IsSynchronized;
                }
            }

            public object SyncRoot
            {
                get
                {
                    return ((System.Collections.ICollection)_source).SyncRoot;
                }
            }

            public void CopyTo(Array array, int index)
            {
                ((System.Collections.ICollection)_source).CopyTo(array, index);
            }

            public IEnumerator<T> GetEnumerator()
            {
                return _source.GetEnumerator();
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return _source.GetEnumerator();
            }
        }

        private sealed class GenericICollection<T> : ICollection<T>
        {
            private readonly ICollection<T> _source;

            public GenericICollection(ICollection<T> source)
            {
                _source = source;
            }

            public int Count
            {
                get
                {
                    return _source.Count;
                }
            }

            public bool IsReadOnly
            {
                get
                {
                    return _source.IsReadOnly;
                }
            }

            public void Add(T item)
            {
                _source.Add(item);
            }

            public void Clear()
            {
                _source.Clear();
            }

            public bool Contains(T item)
            {
                return _source.Contains(item);
            }

            public void CopyTo(T[] array, int arrayIndex)
            {
                _source.CopyTo(array, arrayIndex);
            }

            public IEnumerator<T> GetEnumerator()
            {
                return _source.GetEnumerator();
            }

            public bool Remove(T item)
            {
                return _source.Remove(item);
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return _source.GetEnumerator();
            }
        }

        [Fact]
        public void SequencesAreEqualIfElementsAreEqual()
        {
            Assert.True(EqualityComparerBuilder.For<int>().Default().EquateSequence().Equals(new[] { 3, 4 }, new[] { 3, 4 }));
            Assert.True(EqualityComparerBuilder.For<int>().Default().EquateSequence().Equals(new[] { 3, 4, 5 }, new[] { 3, 4, 5 }));
            Assert.Equal(EqualityComparerBuilder.For<int>().Default().EquateSequence().GetHashCode(new[] { 3, 4 }), EqualityComparerBuilder.For<int>().Default().EquateSequence().GetHashCode(new[] { 3, 4 }));
        }

        [Fact]
        public void EnumerablesAreEqualIfElementsAreEqual()
        {
            Assert.True(EqualityComparerBuilder.For<int>().Default().EquateSequence().Equals(E_3_4(), E_3_4()));
            Assert.True(EqualityComparerBuilder.For<int>().Default().EquateSequence().Equals(E_3_4_5(), E_3_4_5()));
            Assert.Equal(EqualityComparerBuilder.For<int>().Default().EquateSequence().GetHashCode(E_3_4()), EqualityComparerBuilder.For<int>().Default().EquateSequence().GetHashCode(E_3_4()));
        }

        [Fact]
        public void EqualLengthSequencesWithUnequalElementsAreNotEqual()
        {
            Assert.False(EqualityComparerBuilder.For<int>().Default().EquateSequence().Equals(new[] { 3, 4 }, new[] { 3, 5 }));
            Assert.False(EqualityComparerBuilder.For<int>().Default().EquateSequence().Equals(new[] { 3, 4, 5 }, new[] { 3, 3, 5 }));
        }

        [Fact]
        public void EqualLengthEnumerableWithUnequalElementsAreNotEqual()
        {
            Assert.False(EqualityComparerBuilder.For<int>().Default().EquateSequence().Equals(E_3_4(), E_3_5()));
            Assert.False(EqualityComparerBuilder.For<int>().Default().EquateSequence().Equals(E_3_4_5(), E_3_3_5()));
        }

        [Fact]
        public void SequenceUsesSourceComparerForElementComparisons()
        {
#if NETCOREAPP1_1
            StringComparer invariantCultureComparerIgnoreCase = CultureInfo.InvariantCulture.CompareInfo.GetStringComparer(CompareOptions.IgnoreCase);
#else
            StringComparer invariantCultureComparerIgnoreCase = StringComparer.InvariantCultureIgnoreCase;
#endif
            var comparer = invariantCultureComparerIgnoreCase.EquateSequence();
            Assert.True(comparer.Equals(new[] { "a" }, new[] { "A" }));
            Assert.Equal(comparer.GetHashCode(new[] { "a" }), comparer.GetHashCode(new[] { "A" }));
        }

        [Fact]
        public void NullIsNotEqualToEmpty()
        {
            Assert.False(EqualityComparerBuilder.For<int>().Default().EquateSequence().Equals(null, Enumerable.Empty<int>()));
            Assert.False(EqualityComparerBuilder.For<int>().Default().EquateSequence().Equals(Enumerable.Empty<int>(), null));
        }

        private static IEnumerable<int> E_3_4()
        {
            yield return 3;
            yield return 4;
        }

        private static IEnumerable<int> E_3_4_5()
        {
            yield return 3;
            yield return 4;
            yield return 5;
        }

        private static IEnumerable<int> E_3_5()
        {
            yield return 3;
            yield return 5;
        }

        private static IEnumerable<int> E_3_3_5()
        {
            yield return 3;
            yield return 3;
            yield return 5;
        }

        [Fact]
        public void ToString_DumpsComparer()
        {
            Assert.Equal("Sequence<Int32>(Default(Int32: IComparable<T>))", EqualityComparerBuilder.For<int>().Default().EquateSequence().ToString());
        }
    }
}
