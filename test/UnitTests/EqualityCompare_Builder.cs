using System.Collections.Generic;
using Nito.Comparers;
using Xunit;
using System;

#pragma warning disable CS0162

namespace UnitTests
{
    public class EqualityCompare_BuilderUnitTests
    {
        [Fact]
        public void ForT_ReturnsBuilderForT()
        {
            var result = EqualityComparerBuilder.For<int>();
            Assert.IsType<EqualityComparerBuilderFor<int>>(result);
        }

        [Fact]
        public void ForFuncT_ReturnsBuilderForT_WithoutInvokingFunc()
        {
            var result = EqualityComparerBuilder.For(() =>
            {
                throw new Exception();
                return 3;
            });
            Assert.IsType<EqualityComparerBuilderFor<int>>(result);
        }

        [Fact]
        public void ForElementsOfT_ReturnsBuilderForT_WithoutEnumeratingSequence()
        {
            var result = EqualityComparerBuilder.ForElementsOf(ThrowEnumerable());
            Assert.IsType<EqualityComparerBuilderFor<int>>(result);
        }

        [Fact]
        public void ForElementsOfFuncT_ReturnsBuilderForT_WithoutInvokingFunc()
        {
            var result = EqualityComparerBuilder.ForElementsOf(() =>
            {
                throw new Exception();
                return ThrowEnumerable();
            });
            Assert.IsType<EqualityComparerBuilderFor<int>>(result);
        }

        private static IEnumerable<int> ThrowEnumerable()
        {
            throw new Exception();
            yield return 13;
        }
    }
}
