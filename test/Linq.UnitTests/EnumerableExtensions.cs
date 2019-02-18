using System.Collections.Generic;
using System.Linq;
using Nito.Comparers.Linq;
using Xunit;
using System;
using Nito.Comparers;

namespace Linq.UnitTests
{
    public class EnumerableExtensionsUnitTests
    {
        [Fact]
        public void OrderBy_UsesComparer()
        {
            IEnumerable<int?> input = new int?[] { int.MaxValue, int.MinValue, null, 0, 1, 2, 3 };
            var result = input.OrderBy(x => x, c => c.OrderBy(x => x % 2 == 0).ThenBy(x => x));
            Assert.Equal(new int?[] { null, 1, 3, int.MaxValue, int.MinValue, 0, 2 }, result);
        }

        [Fact]
        public void OrderByDescending_UsesComparer()
        {
            IEnumerable<int?> input = new int?[] { int.MaxValue, int.MinValue, null, 0, 1, 2, 3 };
            var result = input.OrderByDescending(x => x, c => c.OrderBy(x => x % 2 == 0).ThenBy(x => x));
            Assert.Equal(new int?[] { 2, 0, int.MinValue, int.MaxValue, 3, 1, null }, result);
        }

        [Fact]
        public void ThenBy_UsesComparer()
        {
            IEnumerable<int?> input = new int?[] { int.MaxValue, int.MinValue, null, 0, 3, 2, 1 };
            var result = input.OrderBy(x => x, c => c.Null()).ThenBy(x => x, c => c.OrderBy(x => x));
            Assert.Equal(new int?[] { null, int.MinValue, 0, 1, 2, 3, int.MaxValue }, result);
        }

        [Fact]
        public void ThenByDescending_UsesComparer()
        {
            IEnumerable<int?> input = new int?[] { int.MaxValue, int.MinValue, null, 0, 3, 2, 1 };
            var result = input.OrderBy(x => x, c => c.Null()).ThenByDescending(x => x, c => c.OrderBy(x => x));
            Assert.Equal(new int?[] { int.MaxValue, 3, 2, 1, 0, int.MinValue, null }, result);
        }

        [Fact]
        public void Contains_UsesComparer()
        {
            IEnumerable<int> values = new int[] { 1, 2, 3, 4, 5 };
            Assert.True(values.Contains(14, c => c.EquateBy(x => x % 13)));
        }

        [Fact]
        public void Distinct_UsesComparer()
        {
            IEnumerable<int> values = new int[] { 1, 2, 3, 4, 5 };
            var result = values.Distinct(c => c.EquateBy(x => x % 3));
            Assert.Equal(new int[] { 1, 2, 3 }, result);
        }

        [Fact]
        public void Except_UsesComparer()
        {
            IEnumerable<int> values = new int[] { 3, 4, 5 };
            var result = values.Except(new int[] { 1 }, c => c.EquateBy(x => x % 3));
            Assert.Equal(new int[] { 3, 5 }, result);
        }

        [Fact]
        public void GroupBy_UsesComparer()
        {
            IEnumerable<int> values = new int[] { 1, 2, 3, 4, 5 };
            var result = values.GroupBy(x => x, c => c.EquateBy(x => x % 3)).ToList();
            Assert.Equal(3, result.Count);
            Assert.Equal(new[] { 1, 4 }, result[0]);
            Assert.Equal(new[] { 2, 5 }, result[1]);
            Assert.Equal(new[] { 3 }, result[2]);
        }

        [Fact]
        public void GroupBy_ElementSelector_UsesComparer()
        {
            IEnumerable<int> values = new int[] { 1, 2, 3, 4, 5 };
            var result = values.GroupBy(x => x, x => x, c => c.EquateBy(x => x % 3)).ToList();
            Assert.Equal(3, result.Count);
            Assert.Equal(new[] { 1, 4 }, result[0]);
            Assert.Equal(new[] { 2, 5 }, result[1]);
            Assert.Equal(new[] { 3 }, result[2]);
        }

        [Fact]
        public void GroupBy_ResultSelector_UsesComparer()
        {
            IEnumerable<int> values = new int[] { 1, 2, 3, 4, 5 };
            var result = values.GroupBy(x => x, (key, source) => source.ToList(), c => c.EquateBy(x => x % 3)).ToList();
            Assert.Equal(3, result.Count);
            Assert.Equal(new[] { 1, 4 }, result[0]);
            Assert.Equal(new[] { 2, 5 }, result[1]);
            Assert.Equal(new[] { 3 }, result[2]);
        }

        [Fact]
        public void GroupBy_ElementAndResultSelector_UsesComparer()
        {
            IEnumerable<int> values = new int[] { 1, 2, 3, 4, 5 };
            var result = values.GroupBy(x => x, x => x, (key, source) => source.ToList(), c => c.EquateBy(x => x % 3)).ToList();
            Assert.Equal(3, result.Count);
            Assert.Equal(new[] { 1, 4 }, result[0]);
            Assert.Equal(new[] { 2, 5 }, result[1]);
            Assert.Equal(new[] { 3 }, result[2]);
        }

        [Fact]
        public void GroupJoin_UsesComparer()
        {
            IEnumerable<int> outer = new int[] { 0, 1, 2 };
            IEnumerable<int> inner = new int[] { 1, 2, 3, 4, 5 };
            var result = outer.GroupJoin(inner, x => x, x => x, (outerValue, innerValues) => innerValues.ToList(), c => c.EquateBy(x => x % 3)).ToList();
            Assert.Equal(3, result.Count);
            Assert.Equal(new[] { 3 }, result[0]);
            Assert.Equal(new[] { 1, 4 }, result[1]);
            Assert.Equal(new[] { 2, 5 }, result[2]);
        }

        [Fact]
        public void Intersect_UsesComparer()
        {
            IEnumerable<int> first = new int[] { 0, 1 };
            IEnumerable<int> second = new int[] { 1, 2, 3, 4, 5 };
            var result = first.Intersect(second, c => c.EquateBy(x => x % 3));
            Assert.Equal(new[] { 0, 1 }, result);
        }

        [Fact]
        public void Join_UsesComparer()
        {
            IEnumerable<int> outer = new int[] { 0, 2 };
            IEnumerable<int> inner = new int[] { 1, 2, 3, 4, 5 };
            var result = outer.Join(inner, x => x, x => x, (outerValue, innerValue) => innerValue, c => c.EquateBy(x => x % 3)).ToList();
            Assert.Equal(new[] { 3, 2, 5 }, result);
        }

        [Fact]
        public void SequenceEqual_UsesComparer()
        {
            IEnumerable<int> first = new int[] { 0, 1, 2 };
            IEnumerable<int> second = new int[] { 15, 4, 2 };
            var result = first.SequenceEqual(second, c => c.EquateBy(x => x % 3));
            Assert.True(result);
        }

        [Fact]
        public void ToDictionary_UsesComparer()
        {
            IEnumerable<int> values = new int[] { 1, 2, 3, 4 };
            Assert.ThrowsAny<ArgumentException>(() => values.ToDictionary(x => x, c => c.EquateBy(x => x % 3)));
        }

        [Fact]
        public void ToDictionary_ElementSelector_UsesComparer()
        {
            IEnumerable<int> values = new int[] { 1, 2, 3, 4 };
            Assert.ThrowsAny<ArgumentException>(() => values.ToDictionary(x => x, x => x, c => c.EquateBy(x => x % 3)));
        }

        [Fact]
        public void ToLookup_UsesComparer()
        {
            IEnumerable<int> values = new int[] { 1, 2, 3, 4 };
            var result = values.ToLookup(x => x, c => c.EquateBy(x => x % 3));
            Assert.Equal(new[] { 3 }, result[0]);
            Assert.Equal(new[] { 1, 4 }, result[1]);
            Assert.Equal(new[] { 2 }, result[2]);
        }

        [Fact]
        public void ToLookup_ElementSelector_UsesComparer()
        {
            IEnumerable<int> values = new int[] { 1, 2, 3, 4 };
            var result = values.ToLookup(x => x, x => x, c => c.EquateBy(x => x % 3));
            Assert.Equal(new[] { 3 }, result[0]);
            Assert.Equal(new[] { 1, 4 }, result[1]);
            Assert.Equal(new[] { 2 }, result[2]);
        }

        [Fact]
        public void Union_UsesComparer()
        {
            IEnumerable<int> first = new int[] { 0, 1 };
            IEnumerable<int> second = new int[] { 1, 2, 3, 4, 5 };
            var result = first.Union(second, c => c.EquateBy(x => x % 3));
            Assert.Equal(new[] { 0, 1, 2 }, result);
        }
    }
}
